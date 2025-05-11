using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/External/Wheel Steer")]
[RequireComponent(typeof(WheelCollider))]
public class WheelSteer : WheelBase
{
    public Transform steerModel;
    public RotationAxis steerRotationAxis;
    public float steerMaxAngle = 30.0f;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void WheelLogicUpdate()
    {
        bool wheelCollider_IsValid = m_WheelCollider.enabled == true && m_WheelCollider.isGrounded == true;
        if (wheelCollider_IsValid == true)
        {
            //-----------------------------------------------------------------
            float thrustZ = this.transform.InverseTransformDirection(m_Owner.TrustTotal).z;
            float throttle = m_Owner.ThrottleInput;
            float engageValue = Mathf.Clamp01(throttle * thrustZ);

            m_WheelCollider.motorTorque = engageValue;
            m_WheelCollider.brakeTorque = 0.0f;

            //-----------------------------------------------------------------

            float speed = this.transform.InverseTransformDirection(m_Velocity).z;
            float length = 2.0f * Mathf.PI * m_WheelCollider.radius;
            float rpm = m_WheelCollider.radius > 0.0f ? speed * 60.0f / length : 0.0f;

            if (wheelModel != null)
            {
                Vector3 wheelModel_Rotate_Axis_Angle;

                if (wheelRotationAxis == RotationAxis.X)
                {
                    wheelModel_Rotate_Axis_Angle = Vector3.right * rpm * 6.0f * Time.deltaTime;
                }
                else if (wheelRotationAxis == RotationAxis.Y)
                {
                    wheelModel_Rotate_Axis_Angle = Vector3.up * rpm * 6.0f * Time.deltaTime;
                }
                else
                {
                    wheelModel_Rotate_Axis_Angle = Vector3.forward * rpm * 6.0f * Time.deltaTime;
                }

                wheelModel.rotation = wheelModel.rotation * Quaternion.Euler(wheelModel_Rotate_Axis_Angle);
            }

            float volume = (rpm / rpmForMaxVolume) * maxVolume;
            m_AudioSource.volume = Mathf.Clamp(volume, 0.0f, 1.0f);
        }
        else
        {
            m_WheelCollider.motorTorque = 0.0f;
            m_WheelCollider.brakeTorque = 0.0f;

            m_AudioSource.volume = 0.0f;
        }

        float steer = Controller.GetAxisInput() * steerMaxAngle;
        m_WheelCollider.steerAngle = steer;

        if (steerModel != null)
        {
            Vector3 steerModel_EularAngle = steerModel.transform.localEulerAngles;

            if (steerRotationAxis == RotationAxis.X)
            {
                steerModel_EularAngle.x = steer;
            }
            else if (steerRotationAxis == RotationAxis.Y)
            {
                steerModel_EularAngle.y = steer;
            }
            else if (steerRotationAxis == RotationAxis.Z)
            {
                steerModel_EularAngle.z = steer;
            }

            steerModel.transform.localEulerAngles = steerModel_EularAngle;
        }
    }

}
