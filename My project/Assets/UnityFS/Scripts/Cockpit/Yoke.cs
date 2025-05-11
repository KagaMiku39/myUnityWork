using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Cockpit/Yoke")]
public class Yoke : AircraftAttachment
{
    [HideInInspector]
    public InputController PitchController;
    [HideInInspector]
    public InputController RollController;

    public Vector3 PitchAxis = Vector3.forward;
    public float MaxPitchTranslationMeters = 0.3f;

    public Vector3 RollAxis = Vector3.forward;
    public float MaxRollDeflectionDegrees = 30.0f;

    private Vector3 m_InitialPosition;
    private Quaternion m_InitialRotation;

    void Start()
    {
        PitchAxis.Normalize();
        RollAxis.Normalize();

        m_InitialPosition = this.transform.localPosition;
        m_InitialRotation = this.transform.localRotation;
    }

    void Update()
    {
        if (m_Controllable == true)
        {        
            //Pitch translation.
            float pitch = PitchController.GetAxisInput() * MaxPitchTranslationMeters;
            this.transform.localPosition = m_InitialPosition;
            this.transform.localPosition += PitchAxis * pitch;

            //Roll rotation
            float roll = RollController.GetAxisInput() * MaxRollDeflectionDegrees;
            this.transform.localRotation = m_InitialRotation;
            this.transform.Rotate(RollAxis, roll);
        }
    }
}
