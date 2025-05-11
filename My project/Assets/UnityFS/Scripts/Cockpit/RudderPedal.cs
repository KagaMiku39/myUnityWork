using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Cockpit/Rudder Pedal")]
public class RudderPedal : MonoBehaviour
{
    [HideInInspector]
    public InputController Controller;

    public Vector3 TranslateAxis = Vector3.forward;
    public float DeflectionMeters = 0.1f;

    private Vector3 m_InitialPosition;

    void Start()
    {
        m_InitialPosition = transform.localPosition;
        TranslateAxis.Normalize();
    }

    void Update()
    {
        float rudderDeflection = Controller.GetAxisInput() * DeflectionMeters;
        this.transform.localPosition = m_InitialPosition;
        this.transform.localPosition += TranslateAxis * rudderDeflection;
    }
}
