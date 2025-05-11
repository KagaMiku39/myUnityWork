using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Dynamics/Engine")]
public class Engine : AircraftAttachment
{
    public enum EngineState
    {
        Off = 0,
        Starting,
        Running
    }

    public Transform AnimatedPropellerPivot;
    public Vector3 AnimatedPropellerPivotRotateAxis = Vector3.forward;
    public GameObject SlowPropeller;
    public GameObject FastPropeller;
    public float RPMToUseFastProp = 300.0f;

    public EngineState CurrentEngineState = EngineState.Off;
    public float IdleRPM = 400.0f;
    public float MaxRPM = 2800.0f;
    public float ForceAtMaxRPM = 4000.0f;
    public AnimationCurve PercentageForceAppliedVSAirspeedKTS;
    public float RPMToAddPerKTOfSpeed = 10.0f;
    public float RPMLerpSpeed = 1.5f;

    public AudioClip EngineStartClip;
    public AudioClip EngineRunClip;
    public float PitchAtIdleRPM = 0.5f;
    public float PitchAtMaxRPM = 1.0f;

    [HideInInspector]
    public Vector3 Thrust;

    [HideInInspector]
    public InputController ThrottleController;
    public InputController EngineStartController;

    public float GetRPM()
    {
        return m_CurrentRPM;
    }
    private float m_CurrentRPM;
    private float m_DesiredRPM = 0.0f;

    private Rigidbody m_Root_Rigidbody;

    private AudioSource m_EngineStart;
    private AudioSource m_EngineRun;
    private float m_EngineRunVolume;

    public void Start()
    {
        m_Root_Rigidbody = transform.root.gameObject.GetComponent<Rigidbody>();

        if (EngineStartClip != null)
        {
            m_EngineStart = gameObject.AddComponent<AudioSource>();
            m_EngineStart.clip = EngineStartClip;
            m_EngineStart.loop = false;
            m_EngineStart.dopplerLevel = 0.0f;
        }

        if (EngineRunClip != null)
        {
            m_EngineRun = gameObject.AddComponent<AudioSource>();
            m_EngineRun.clip = EngineRunClip;
            m_EngineRun.loop = true;
            m_EngineRun.Play();
            m_EngineRunVolume = m_EngineRun.volume;
            m_EngineRun.dopplerLevel = 0.0f;
        }
    }

    public void Update()
    {
        if (m_Root_Rigidbody != null)
        {
            switch (CurrentEngineState)
            {
                case EngineState.Off:
                    {
                        UpdateOff();
                    }
                    break;

                case EngineState.Starting:
                    {
                        UpdateStarting();
                    }
                    break;

                case EngineState.Running:
                    {
                        UpdateRunning();
                    }
                    break;
            }

            //Lerp current rpm to desired rpm.
            m_CurrentRPM = Mathf.Lerp(m_CurrentRPM, m_DesiredRPM, RPMLerpSpeed * Time.deltaTime);

            //Update audio based on RPM. (Doesn't matter if it's not playing i.e engine off )
            if (m_EngineRun != null)
            {
                float velocity = m_Root_Rigidbody.velocity.magnitude;
                float velocityKTS = velocity * Conversions.MetersPerSecondToKnots;

                float CurrentPitchRPM = m_CurrentRPM + velocityKTS * RPMToAddPerKTOfSpeed;

                float rpmOffset = (CurrentPitchRPM - IdleRPM) / (MaxRPM - IdleRPM);

                float enginePitch = PitchAtIdleRPM + ((PitchAtMaxRPM - PitchAtIdleRPM) * rpmOffset);
                m_EngineRun.pitch = enginePitch;

                //If below idle fade out engine run sound..
                if (m_CurrentRPM < IdleRPM)
                {
                    m_EngineRun.volume = m_EngineRunVolume * (m_CurrentRPM / IdleRPM);

                    if (m_CurrentRPM < IdleRPM * 0.1f)
                    {
                        m_EngineRun.volume = 0.0f;
                    }
                }
                else
                {
                    m_EngineRun.volume = m_EngineRunVolume;
                }

            }

            //Set the correct propeller visibility.
            if (SlowPropeller != null && FastPropeller != null)
            {
                if (m_CurrentRPM > RPMToUseFastProp)
                {
                    SlowPropeller.GetComponent<Renderer>().enabled = false;
                    FastPropeller.GetComponent<Renderer>().enabled = true;
                }
                else
                {
                    SlowPropeller.GetComponent<Renderer>().enabled = true;
                    FastPropeller.GetComponent<Renderer>().enabled = false;
                }
            }

            //Rotate the propeller hub.
            if (AnimatedPropellerPivot != null)
            {
                float rotationThisFrame = ((m_CurrentRPM * 360.0f) / 60.0f) * Time.deltaTime;
                AnimatedPropellerPivot.transform.Rotate(AnimatedPropellerPivotRotateAxis, rotationThisFrame);
            }
        }
    }


    public void FixedUpdate()
    {
        if (m_Root_Rigidbody != null)
        {
            float forceMultiplier = (m_CurrentRPM - IdleRPM) / (MaxRPM - IdleRPM);
            forceMultiplier = Mathf.Clamp(forceMultiplier, 0.0f, 1.0f);

            float velocity = m_Root_Rigidbody.velocity.magnitude;
            float velocityKTS = velocity * Conversions.MetersPerSecondToKnots;
            float thrustPercent = PercentageForceAppliedVSAirspeedKTS.Evaluate(velocityKTS) * 0.01f; //Convert to zero to one.

            Thrust = (transform.forward * ForceAtMaxRPM * forceMultiplier) * thrustPercent;
            m_Root_Rigidbody.AddForceAtPosition(Thrust, transform.position, ForceMode.Force);
        }
    }

    private void UpdateOff()
    {
        if (m_EngineStart.isPlaying == true)
        {
            m_EngineStart.Stop();
        }

        if (m_Controllable == true)
        {
            //Listen for engine start and trigger start sound..
            if (EngineStartController.GetButton() == true)
            {
                m_EngineStart.Play();
                CurrentEngineState = EngineState.Starting;
            }
        }

        //Spin down blades.
        m_DesiredRPM = 0.0f;
    }

    private void UpdateStarting()
    {
        //If start is held througout start procedure run engine else stop.
        if (m_Controllable == true && EngineStartController.GetButton())
        {
            if (m_EngineStart.isPlaying == false)
            {
                CurrentEngineState = EngineState.Running;
            }
        }
        else
        {
            m_EngineStart.Stop();
            CurrentEngineState = EngineState.Off;
        }

        //Spin up blades to idle.
        m_DesiredRPM = IdleRPM;
    }

    private void UpdateRunning()
    {
        if (m_EngineStart.isPlaying == true)
        {
            m_EngineStart.Stop();
        }

        if (m_Controllable == true)
        {
            //Control throttle.
            float input = ThrottleController.GetAxisInput();
            input = Mathf.Clamp(input, 0.0f, 1.0f);
            m_DesiredRPM = IdleRPM + ((MaxRPM - IdleRPM) * input);

            //Start stop engine.
            if (EngineStartController.GetButtonPressed() == true)
            {
                //EngineRun.Stop();
                CurrentEngineState = EngineState.Off;
            }
        }
    }



    public void OnDrawGizmos()
    {
        //Draw icon.
        Gizmos.DrawIcon(transform.position, "prop.png", true);

        if (PercentageForceAppliedVSAirspeedKTS != null)
        {
            //Add two keys at the start and end..
            if (PercentageForceAppliedVSAirspeedKTS.keys.Length < 2)
            {
                PercentageForceAppliedVSAirspeedKTS.AddKey(0.0f, 100.0f);
                PercentageForceAppliedVSAirspeedKTS.AddKey(300.0f, 100.0f);
            }
        }
    }

}
