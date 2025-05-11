using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("UnityFS/Aircraft")]
[RequireComponent(typeof(Rigidbody))]
public class Aircraft : MonoBehaviour
{
    [HideInInspector]
    public InputController ChangeCameraController;

    public bool AircraftEnabledAtStart = true;
    public bool OverrideInertiaTensor;
    public Vector3 InertiaTensor;
    public float RollwiseDamping = 1.0f;

    private int m_CurrentCameraIndex = 0;

    private Rigidbody m_Rigidbody;

    private Engine[] m_EngineArray;
    private AircraftAttachment[] m_AircraftAttachments;
    private AircraftCamera[] m_AircraftCameras;

    public float ThrottleInput { get; set; }
    public Vector3 TrustTotal { get; set; }
    public bool AircraftEnabled { get; set; }
    public virtual void Awake()
    {
        //Register a list of all attached parts and cameras..
        m_AircraftAttachments = GetComponentsInChildren<AircraftAttachment>();
        m_AircraftCameras = GetComponentsInChildren<AircraftCamera>();
        m_EngineArray = GetComponentsInChildren<Engine>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Start()
    {
        //Enable control if requested at start.
        if (AircraftEnabledAtStart == true)
        {
            AircraftEnabled = true;
            EnableControl(true);
        }

        //Override inertia tensor if so desired.
        if (OverrideInertiaTensor == true)
        {
            m_Rigidbody.inertiaTensor = InertiaTensor;
        }

        //Clamp rollwise damping.
        RollwiseDamping = Mathf.Clamp(RollwiseDamping, 0.0f, 1.0f);

        ThrottleInput = GetThrottleInput();

        TrustTotal = GetThrustTotal();
    }

    public virtual void Update()
    {
        if (AircraftEnabled == true)
        {
            //Listen for input to swap cameras..
            if (ChangeCameraController.GetButtonPressed() == true)
            {
                int previousCameraIndex = m_CurrentCameraIndex;

                m_CurrentCameraIndex++;
                if (m_CurrentCameraIndex >= m_AircraftCameras.Length)
                {
                    m_CurrentCameraIndex = 0;
                }

                m_AircraftCameras[previousCameraIndex].SetCameraActive(false);
                m_AircraftCameras[m_CurrentCameraIndex].SetCameraActive(true);
            }
        }

        ThrottleInput = GetThrottleInput();

        TrustTotal = GetThrustTotal();
    }

    public void EnableControl(bool enable)
    {
        //Set all parts enabled.
        if (m_AircraftAttachments != null)
        {
            for (int i = 0; i < m_AircraftAttachments.Length; i++)
            {
                m_AircraftAttachments[i].SetControllable(enable);
            }
        }

        //Enable start camera.
        m_CurrentCameraIndex = 0;
        if (m_AircraftCameras != null)
        {
            for (int i = 0; i < m_AircraftCameras.Length; i++)
            {
                //Always disable all cameras.
                m_AircraftCameras[i].SetCameraActive(false);

                if (i == m_CurrentCameraIndex)
                {
                    m_AircraftCameras[i].SetCameraActive(enable);
                }
            }
        }
    }

    public Vector3 GetThrustTotal()
    {
        Vector3 thrust = Vector3.zero;
        for (int i = 0; i < m_EngineArray.Length; i++)
        {
            Engine engine = m_EngineArray[i];
            thrust = thrust + engine.Thrust;
        }

        return thrust;
    }

    public float GetThrottleInput()
    {
        float throttleInput = 0.0f;
        for (int i = 0; i < m_EngineArray.Length; i++)
        {
            Engine engine = m_EngineArray[i];
            throttleInput = throttleInput + Mathf.Clamp01(engine.ThrottleController.GetAxisInput());
        }

        throttleInput = m_EngineArray.Length > 0 ? throttleInput / (float)m_EngineArray.Length : 0.0f;

        return throttleInput;
    }

    public float GetAirspeedKnots()
    {
        return m_Rigidbody.velocity.magnitude * Conversions.MetersPerSecondToKnots;
    }

    public float GetAltitudeThousandsFeet()
    {
        return (gameObject.transform.position.y * Conversions.MetersToFeet) / 1000.0f;
    }

    public float GetAltitudeHundredsFeet()
    {
        return (gameObject.transform.position.y * Conversions.MetersToFeet) / 100.0f;
    }

    public float GetAltitude()
    {
        return (gameObject.transform.position.y * Conversions.MetersToFeet);
    }

    public float GetHeadingDegrees()
    {
        return gameObject.transform.eulerAngles.y;
    }

    public float GetBankDegrees()
    {
        return gameObject.transform.localEulerAngles.z;
    }

    public float GetRateOfClimbFPM()
    {
        float yRate = m_Rigidbody.velocity.y;
        yRate *= Conversions.MetersToFeet;
        yRate *= 60.0f;
        return yRate;
    }

    public float GetEngineRPM(int engineIndex)
    {
        float rpm = 0.0f;
        if (m_EngineArray != null)
        {
            engineIndex = Mathf.Clamp(engineIndex, 0, m_EngineArray.Length - 1);

            if (m_EngineArray[engineIndex] != null)
            {
                rpm = m_EngineArray[engineIndex].GetRPM();
            }
        }
        return rpm;
    }

    public float GetEngineRPM()
    {
        return GetEngineRPM(0);
    }
}