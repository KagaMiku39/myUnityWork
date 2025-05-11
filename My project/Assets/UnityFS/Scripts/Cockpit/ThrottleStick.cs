using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Cockpit/Throttle Stick")]
public class ThrottleStick : MonoBehaviour
{
    [HideInInspector]
    public InputController Controller;

    public Vector3 ThrottleAxis = new Vector3(1.0f, 0.0f, 0.0f);
    public float MaxDeflectionDegrees = 15.0f;

    private Quaternion m_InitialRotation;

    void Start()
    {
        ThrottleAxis.Normalize();

        m_InitialRotation = this.transform.localRotation;
    }

    void Update()
    {
        float throttleAmount = Controller.GetAxisInput();
        throttleAmount = Mathf.Clamp(throttleAmount, 0.0f, 1.0f);
        throttleAmount *= MaxDeflectionDegrees;
        this.transform.localRotation = m_InitialRotation;
        this.transform.Rotate(ThrottleAxis, throttleAmount);
    }
}
