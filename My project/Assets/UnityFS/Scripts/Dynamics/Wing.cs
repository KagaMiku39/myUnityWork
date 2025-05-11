using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Dynamics/Wing")]
[RequireComponent(typeof(BoxCollider))]
public class Wing : AircraftAttachment
{
    public Aerofoil Aerofoil = null;

    public int SectionCount = 10;
    public float WingTipWidthZeroToOne = 1.0f;
    public float WingTipSweep = 0.0f;
    public float WingTipAngle = 0.0f;

    public float CDOverride = 0.045f;

    [HideInInspector]
    public float WingArea = 0.0f;
    [HideInInspector]
    public float AngleOfAttack = 0.0f;

    private BoxCollider m_WingBoxCollider = null;
    private Vector3 m_WingRootLeadingEdge = Vector3.zero;
    private Vector3 m_WingRootTrailingEdge = Vector3.zero;
    private Vector3 m_WingTipLeadingEdge = Vector3.zero;
    private Vector3 m_WingTipTrailingEdge = Vector3.zero;
    private Vector3 m_RootLiftPosition = Vector3.zero;
    private Vector3 m_TipLiftPosition = Vector3.zero;
    private float m_LiftLineChordPosition = 0.75f;

    private Rigidbody m_Root_Rigidbody = null;
    private Aircraft m_Root_Aircraft = null;
    private ControlSurface m_AttachedControlSurface = null;
    private PropWash m_AttachedPropWash = null;
    private GroundEffect m_AttachedGroundEffect = null;

    // Use this for initialization
    public void Start()
    {
        m_WingBoxCollider = this.GetComponent<BoxCollider>();
        m_Root_Rigidbody = transform.root.gameObject.GetComponent<Rigidbody>();
        m_Root_Aircraft = transform.root.gameObject.GetComponent<Aircraft>();
        m_AttachedControlSurface = this.GetComponent<ControlSurface>();
        m_AttachedPropWash = this.GetComponent<PropWash>();
        m_AttachedGroundEffect = this.GetComponent<GroundEffect>();
    }

    public void FixedUpdate()
    {
        float debugLineScale = 1.0f / 30.0f;

        //Calculate position of wing points in worldspace this frame.
        UpdateWingGeometry();


        //Per section update.
        for (int i = 0; i < SectionCount; i++)
        {
            //R A-----------------B (Leading edge)
            //O |                 |  
            //O |                 |
            //T D-----------------C (Trailing edge


            //Find points a,b,c & d for this chunk of wing.
            Vector3 a = m_WingRootLeadingEdge + ((m_WingTipLeadingEdge - m_WingRootLeadingEdge) * (float)i / (float)SectionCount);
            Vector3 b = m_WingRootLeadingEdge + ((m_WingTipLeadingEdge - m_WingRootLeadingEdge) * (float)(i + 1) / (float)SectionCount);
            Vector3 c = m_WingRootTrailingEdge + ((m_WingTipTrailingEdge - m_WingRootTrailingEdge) * (float)(i + 1) / (float)SectionCount);
            Vector3 d = m_WingRootTrailingEdge + ((m_WingTipTrailingEdge - m_WingRootTrailingEdge) * (float)i / (float)SectionCount);

            //Draw premodified wing.
            //			Debug.DrawLine( a, b );
            //			Debug.DrawLine( b, c );
            //			Debug.DrawLine( c, d );
            //			Debug.DrawLine( d, a );

            //If we have a control surface attached update the geometry based on control surface inputs.
            if (m_AttachedControlSurface != null)
            {
                m_AttachedControlSurface.ModifyWingGeometry(i, ref a, ref b, ref c, ref d);
            }

            //Draw modified wing.
            Debug.DrawLine(a, b);
            Debug.DrawLine(b, c);
            Debug.DrawLine(c, d);
            Debug.DrawLine(d, a);

            //Recalculate lift positions..
            Vector3 sectionRootLiftPosition = d + ((a - d) * m_LiftLineChordPosition);
            Vector3 sectionTipLiftPosition = c + ((b - c) * m_LiftLineChordPosition);

            //Find the aerodynamic center.
            Vector3 aerodynamicCenter = sectionRootLiftPosition + ((sectionTipLiftPosition - sectionRootLiftPosition) * 0.5f);

            //Find the chord line.
            Vector3 chordLine = (a + ((b - a) * 0.5f)) - (d + ((c - d) * 0.5f));
            float chordLength = chordLine.magnitude;
            chordLine.Normalize();

            Debug.DrawLine(aerodynamicCenter, aerodynamicCenter + chordLine, Color.blue);

            //Get relative wind.
            Vector3 relativeWind = -m_Root_Rigidbody.velocity;

            //Calculate angular to linear velocity of any rotation and add to relative wind.
            Vector3 fromCOMToAerodynamicCenter = aerodynamicCenter - m_Root_Rigidbody.worldCenterOfMass;
            Vector3 angularVelocity = m_Root_Rigidbody.angularVelocity;

            Vector3 localRelativeWind = Vector3.Cross(angularVelocity.normalized, fromCOMToAerodynamicCenter.normalized);
            localRelativeWind *= -((angularVelocity.magnitude) * fromCOMToAerodynamicCenter.magnitude);

            //Tweak rollwise damping based on parent aircraft if it exists.
            if (m_Root_Aircraft != null)
            {
                localRelativeWind *= m_Root_Aircraft.RollwiseDamping;
            }

            //Apply
            relativeWind += localRelativeWind;

            //Propwash
            if (m_AttachedPropWash != null)
            {
                if (m_AttachedPropWash.PropWashSource != null && m_AttachedPropWash.AffectedSections[i] == true)
                {
                    relativeWind += -m_AttachedPropWash.PropWashSource.Thrust * m_AttachedPropWash.PropWashStrength;
                }
            }

            //Tweak relative wind so we only consider that which is flowing over the wing.
            Debug.DrawLine(aerodynamicCenter - (relativeWind.normalized), aerodynamicCenter, Color.grey);
            Vector3 correction = this.transform.right;
            float perpChordDotRelativeWind = Vector3.Dot(correction, relativeWind);
            correction *= perpChordDotRelativeWind;
            relativeWind -= correction;
            Debug.DrawLine(aerodynamicCenter - relativeWind.normalized, aerodynamicCenter, Color.white);


            //Find the angle of attack.	
            Vector3 relativeWindNormalized = relativeWind.normalized;
            AngleOfAttack = Vector3.Dot(chordLine, -relativeWindNormalized);
            AngleOfAttack = Mathf.Clamp(AngleOfAttack, -1.0f, 1.0f);
            AngleOfAttack = Mathf.Acos(AngleOfAttack);
            AngleOfAttack *= Mathf.Rad2Deg;


            Vector3 up = Vector3.Cross(chordLine, (sectionTipLiftPosition - sectionRootLiftPosition).normalized);
            up.Normalize();

            if (transform.localScale.x < 0.0f)
            {
                up = -up;
            }

            float yAxisDotRelativeWind = Vector3.Dot(up, relativeWindNormalized);
            if (yAxisDotRelativeWind < 0.0f)
            {
                AngleOfAttack = -AngleOfAttack;
            }

            float totalLift = 0.0f;
            float totalDrag = 0.0f;
            float cM = 0.0f;

            float clGroundEffectMult = 1.0f;
            float cdGroundEffectMult = 1.0f;

            if ( m_AttachedGroundEffect != null)
            {
                m_AttachedGroundEffect.GetGroundEffectCoefficients(a, b, c, d, out clGroundEffectMult, out cdGroundEffectMult);
            }

            if (Aerofoil != null)
            {
                //Use aerofoil..

                //L = cl * a * 0.5f * r * v^2
                float cL = Aerofoil.CL.Evaluate(AngleOfAttack);
                cL *= clGroundEffectMult;

                float area = CalculateArea(a, b, c, d);
                float r = 1.29f;
                float v = relativeWind.magnitude;
                totalLift = cL * area * 0.5f * r * (v * v);

                //D = 0.5f * cd * r * v2 * a;
                float cD = Aerofoil.CD.Evaluate(AngleOfAttack);
                cD *= cdGroundEffectMult;

                totalDrag = 0.5f * cD * r * (v * v) * area;

                cM = Aerofoil.CM.Evaluate(AngleOfAttack);
            }
            else
            {
                //Fall back to basic l/d equations..
                //L = cl * a * 0.5f * r * v^2
                //Approximate Cl using the following formula - Cl = 2 * pi * angle (in radians)
                float cL = 2.0f * Mathf.PI * (AngleOfAttack * Mathf.Deg2Rad);
                cL *= clGroundEffectMult;

                float area = CalculateArea(a, b, c, d);
                float r = 1.29f;
                float v = relativeWind.magnitude;
                totalLift = cL * area * 0.5f * r * (v * v);


                //D = 0.5f * cd * r * v2 * a;
                //Typical aerofoil drag co efficient is .045;
                float cD = CDOverride; //Typical aerofoil drag co efficient
                cD *= cdGroundEffectMult;

                totalDrag = 0.5f * cD * r * (v * v) * area;

                cM = 0.0f;
            }


            //Build Lift vector.
            Vector3 liftForce = Vector3.Cross(this.transform.right, relativeWind);
            liftForce.Normalize();
            liftForce *= totalLift;
            Debug.DrawLine(aerodynamicCenter, aerodynamicCenter + (liftForce * Time.deltaTime * debugLineScale), Color.green);


            //Build Drag vector.
            Vector3 dragForce = relativeWind;
            dragForce.Normalize();
            dragForce *= totalDrag;
            Debug.DrawLine(aerodynamicCenter, aerodynamicCenter + (dragForce * Time.deltaTime * debugLineScale), Color.red);

            //LiftMoment.
            Vector3 liftDragPoint = aerodynamicCenter;

            //Find wing pitching moment...
            float wingPitchingMoment = cM * chordLength * (0.5f * 1.29f * (relativeWind.magnitude * relativeWind.magnitude)) * CalculateArea(a, b, c, d);
            Vector3 pitchAxis = Vector3.Cross(chordLine, liftForce.normalized);
            pitchAxis.Normalize();
            pitchAxis *= wingPitchingMoment;

            //Apply forces.
            m_Root_Rigidbody.AddForceAtPosition(liftForce, liftDragPoint, ForceMode.Force);
            m_Root_Rigidbody.AddForceAtPosition(dragForce, liftDragPoint, ForceMode.Force);
            m_Root_Rigidbody.AddTorque(pitchAxis, ForceMode.Force);
        }

    }

    public void OnDrawGizmos()
    {
        //Draw icon.
        Gizmos.DrawIcon(transform.position, "wing.png", true);

        m_WingBoxCollider = this.GetComponent<BoxCollider>();
        if (m_WingBoxCollider != null)
        {
            //Clamp box collider scales.
            m_WingBoxCollider.size = new Vector3(1.0f, 0.1f, 1.0f);

            UpdateWingGeometry();

            //Wing geometry.
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(m_WingRootLeadingEdge, m_WingTipLeadingEdge);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(m_WingTipTrailingEdge, m_WingRootTrailingEdge);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(m_WingRootTrailingEdge, m_WingRootLeadingEdge);
            Gizmos.DrawLine(m_WingTipLeadingEdge, m_WingTipTrailingEdge);


            //Sections.
            Gizmos.color = Color.blue;
            for (int i = 0; i < SectionCount; i++)
            {
                Vector3 sectionStart = m_WingRootTrailingEdge + ((m_WingTipTrailingEdge - m_WingRootTrailingEdge) * (float)i / (float)SectionCount);
                Vector3 sectionEnd = m_WingRootLeadingEdge + ((m_WingTipLeadingEdge - m_WingRootLeadingEdge) * (float)i / (float)SectionCount);
                Gizmos.DrawLine(sectionStart, sectionEnd);
            }

            //Lift line.
            Gizmos.color = Color.green;
            Gizmos.DrawLine(m_RootLiftPosition, m_TipLiftPosition);

            //Aileron hinge
            m_AttachedControlSurface = this.GetComponent<ControlSurface>();
            if (m_AttachedControlSurface != null)
            {
                float rootHingeOffset = m_AttachedControlSurface.RootHingeDistanceFromTrailingEdge;
                float tipHingeOffset = m_AttachedControlSurface.TipHingeDistanceFromTrailingEdge;

                Vector3 wingRootAileronHingePos = m_WingRootTrailingEdge + ((m_WingRootLeadingEdge - m_WingRootTrailingEdge) * rootHingeOffset);
                Vector3 wingTipAileronHingePos = m_WingTipTrailingEdge + ((m_WingTipLeadingEdge - m_WingTipTrailingEdge) * tipHingeOffset);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(wingRootAileronHingePos, wingTipAileronHingePos);

                //Control surface - Draw crosses over each control surface section which is affected.
                if (m_AttachedControlSurface.AffectedSections != null)
                {
                    for (int i = 0; i < m_AttachedControlSurface.AffectedSections.Length; i++)
                    {
                        if (m_AttachedControlSurface.AffectedSections[i] == true)
                        {
                            Vector3 hingeLeft = wingRootAileronHingePos + ((wingTipAileronHingePos - wingRootAileronHingePos) * ((float)i / (float)m_AttachedControlSurface.AffectedSections.Length));
                            Vector3 hingeRight = wingRootAileronHingePos + ((wingTipAileronHingePos - wingRootAileronHingePos) * ((float)(i + 1) / (float)m_AttachedControlSurface.AffectedSections.Length));

                            Vector3 backLeft = m_WingRootTrailingEdge + ((m_WingTipTrailingEdge - m_WingRootTrailingEdge) * ((float)i / (float)m_AttachedControlSurface.AffectedSections.Length));
                            Vector3 backRight = m_WingRootTrailingEdge + ((m_WingTipTrailingEdge - m_WingRootTrailingEdge) * ((float)(i + 1) / (float)m_AttachedControlSurface.AffectedSections.Length));

                            Gizmos.DrawLine(hingeLeft, backRight);
                            Gizmos.DrawLine(hingeRight, backLeft);
                        }
                    }
                }
            }

            //Prop wash - Draw crosses over each control surface section which is affected.
            m_AttachedPropWash = this.GetComponent<PropWash>();
            if (m_AttachedPropWash != null)
            {
                Gizmos.color = Color.cyan;
                if (m_AttachedPropWash.AffectedSections != null)
                {
                    for (int i = 0; i < m_AttachedPropWash.AffectedSections.Length; i++)
                    {
                        if (m_AttachedPropWash.AffectedSections[i] == true)
                        {
                            Vector3 frontLeft = m_WingRootLeadingEdge + ((m_WingTipLeadingEdge - m_WingRootLeadingEdge) * ((float)i / (float)m_AttachedPropWash.AffectedSections.Length));
                            Vector3 frontRight = m_WingRootLeadingEdge + ((m_WingTipLeadingEdge - m_WingRootLeadingEdge) * ((float)(i + 1) / (float)m_AttachedPropWash.AffectedSections.Length));

                            Vector3 backLeft = m_WingRootTrailingEdge + ((m_WingTipTrailingEdge - m_WingRootTrailingEdge) * ((float)i / (float)m_AttachedPropWash.AffectedSections.Length));
                            Vector3 backRight = m_WingRootTrailingEdge + ((m_WingTipTrailingEdge - m_WingRootTrailingEdge) * ((float)(i + 1) / (float)m_AttachedPropWash.AffectedSections.Length));

                            //Vector3 topCenter = hingeLeft + ( (hingeRight-hingeLeft) * 0.5f );
                            //Vector3 bottomCenter = backLeft + ( (backRight-backLeft) * 0.5f );
                            ///Vector3 leftCenter = backLeft + ( (hingeLeft-backLeft) * 0.5f );
                            //Vector3 rightCenter = backRight + ( (hingeRight-backRight) * 0.5f );

                            Gizmos.DrawLine(frontLeft, backRight);
                            Gizmos.DrawLine(frontRight, backLeft);
                        }
                    }
                }
            }

        }
    }

    private void UpdateWingGeometry()
    {
        //Calculate root and tip center points.
        Vector3 wingRootCenter = transform.position - (transform.right * (transform.localScale.x * 0.5f));
        Vector3 wingTipCenter = transform.position + (transform.right * (transform.localScale.x * 0.5f));
        wingTipCenter += transform.forward * WingTipSweep;

        //Calculate corners.
        m_WingRootLeadingEdge = wingRootCenter + (transform.forward * (transform.localScale.z * 0.5f));
        m_WingRootTrailingEdge = wingRootCenter - (transform.forward * (transform.localScale.z * 0.5f));
        m_WingTipLeadingEdge = wingTipCenter + (transform.forward * ((transform.localScale.z * 0.5f) * WingTipWidthZeroToOne));
        m_WingTipTrailingEdge = wingTipCenter - (transform.forward * ((transform.localScale.z * 0.5f) * WingTipWidthZeroToOne));


        //Tweak tip corners based on the angle between them.
        Vector3 tipTrailingEdgeToTipLeadingEdge = m_WingTipLeadingEdge - m_WingTipTrailingEdge;
        Quaternion rotation = Quaternion.AngleAxis(WingTipAngle, transform.rotation * new Vector3(1.0f, 0.0f, 0.0f));
        tipTrailingEdgeToTipLeadingEdge = rotation * tipTrailingEdgeToTipLeadingEdge;
        m_WingTipTrailingEdge = wingTipCenter - (tipTrailingEdgeToTipLeadingEdge * 0.5f);
        m_WingTipLeadingEdge = wingTipCenter + (tipTrailingEdgeToTipLeadingEdge * 0.5f);

        m_RootLiftPosition = m_WingRootTrailingEdge + ((m_WingRootLeadingEdge - m_WingRootTrailingEdge) * m_LiftLineChordPosition);
        m_TipLiftPosition = m_WingTipTrailingEdge + ((m_WingTipLeadingEdge - m_WingTipTrailingEdge) * m_LiftLineChordPosition);

        //Calculate wing area.
        WingArea = CalculateArea(m_WingRootLeadingEdge, m_WingTipLeadingEdge, m_WingTipTrailingEdge, m_WingRootTrailingEdge);
    }

    private float CalculateArea(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD)
    {
        float ab = (pointB - pointA).magnitude;
        float bc = (pointC - pointB).magnitude;
        float cd = (pointD - pointC).magnitude;
        float da = (pointA - pointD).magnitude;

        float s = (ab + bc + cd + da) * 0.5f;
        float squareArea = (s - ab) * (s - bc) * (s - cd) * (s - da);
        float area = Mathf.Sqrt(squareArea);

        return area;
    }
}