using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Dynamics/Control Surface")]
[RequireComponent(typeof(Wing))]
public class ControlSurface : AircraftAttachment
{
    public float MaxDeflectionDegrees = 30.0f;
    public float RootHingeDistanceFromTrailingEdge = 0.25f;
    public float TipHingeDistanceFromTrailingEdge = 0.25f;
    public bool[] AffectedSections;
    public GameObject Model = null;
    public Vector3 ModelRotationAxis = Vector3.left;
    public AnimationCurve InputCurve;

    [HideInInspector]
    public InputController Controller;

    private Wing m_Wing;
    private Vector3 m_WingRootAileronHingePos;
    private Vector3 m_WingTipAileronHingePos;
    private Quaternion m_InitialModelRotation;
    private float m_CurrentDeflection = 0.0f;

    public void Start()
    {
        m_Wing = this.GetComponent<Wing>();

        ModelRotationAxis.Normalize();

        if (Model != null)
        {
            m_InitialModelRotation = Model.transform.localRotation;
        }
    }

    public void Update()
    {
        float input = Controller.GetAxisInput();

        //Only move if control is enabled..
        if (m_Controllable == true)
        {
            float curveValue = InputCurve.Evaluate(Mathf.Abs(input));
            curveValue *= Mathf.Sign(input);
            m_CurrentDeflection = curveValue * MaxDeflectionDegrees;
        }

        //Apply rotation to model.	
        if (Model != null)
        {
            Model.transform.localRotation = m_InitialModelRotation;
            Model.transform.Rotate(ModelRotationAxis, m_CurrentDeflection);
        }
    }

    public void ModifyWingGeometry(int SectionIndex, ref Vector3 PointA, ref Vector3 PointB, ref Vector3 PointC, ref Vector3 PointD)
    {
        if (SectionIndex < AffectedSections.Length)
        {
            if (AffectedSections[SectionIndex] == true)
            {
                //return;
                //R A-----------------B (Leading edge)
                //O |                 |  
                //O |                 |
                //T D-----------------C (Trailing edge

                //First step is to work out the aileron position and offset on the wing.
                m_WingRootAileronHingePos = PointD + ((PointA - PointD) * RootHingeDistanceFromTrailingEdge);
                m_WingTipAileronHingePos = PointC + ((PointB - PointC) * TipHingeDistanceFromTrailingEdge);
                Vector3 aileronHinge = m_WingTipAileronHingePos - m_WingRootAileronHingePos;

                Vector3 rootAileronAngle = PointD - m_WingRootAileronHingePos;
                Vector3 tipAileronAngle = PointC - m_WingTipAileronHingePos;

                //Deflect ailerons.
                Quaternion hingeRotation = Quaternion.AngleAxis(m_CurrentDeflection, aileronHinge.normalized);
                rootAileronAngle = hingeRotation * rootAileronAngle;
                tipAileronAngle = hingeRotation * tipAileronAngle;

                //Once we know the deflection of the aileron and where are new trailing edge is, we can use this to tweak the
                //wing chord line.
                PointD = m_WingRootAileronHingePos + rootAileronAngle;
                PointC = m_WingTipAileronHingePos + tipAileronAngle;
            }
        }
    }

    public void OnDrawGizmos()
    {
        ClampEditorValues();
    }


    private void ClampEditorValues()
    {
        MaxDeflectionDegrees = Mathf.Clamp(MaxDeflectionDegrees, 0.0f, 90.0f);
        RootHingeDistanceFromTrailingEdge = Mathf.Clamp(RootHingeDistanceFromTrailingEdge, 0.0f, 1.0f);
        TipHingeDistanceFromTrailingEdge = Mathf.Clamp(TipHingeDistanceFromTrailingEdge, 0.0f, 1.0f);

        if (m_Wing != null)
        {
            if (AffectedSections == null || m_Wing.SectionCount != AffectedSections.Length)
            {
                AffectedSections = new bool[m_Wing.SectionCount];
            }
        }
    }
}
