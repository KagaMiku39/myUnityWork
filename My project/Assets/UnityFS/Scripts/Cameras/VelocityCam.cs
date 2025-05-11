using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Cameras/Velocity Cam")]
[RequireComponent(typeof(Camera))]
public class VelocityCam : AircraftCamera
{
    public Vector3 StartOffset = new Vector3(10.0f, 10.0f, 20.0f);
    public float ResetTimeSeconds = 15.0f;

    private Camera m_Camera;
    private Aircraft m_Aircraft;
    private Rigidbody m_Aircraft_Rigidbody;
    private Vector3 m_CurrentPosition;
    private Vector3 m_Velocity;
    private float m_CurrentTime;

    // Use this for initialization
    public void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_Camera.enabled = false;
        m_Aircraft = transform.root.gameObject.GetComponent<Aircraft>();
        m_Aircraft_Rigidbody = m_Aircraft.GetComponent<Rigidbody>();
    }

    protected override void OnCameraEnabled()
    {
        Reposition();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (m_CameraActive == true && m_Aircraft != null)
        {
            //Reset view if necessary.
            if (m_CurrentTime > ResetTimeSeconds)
            {
                Reposition();
            }

            m_CurrentTime += Time.deltaTime;

            //Update camera position.
            m_CurrentPosition += m_Velocity * Time.deltaTime;

            Vector3 cameraTarget = m_Aircraft.transform.position;

            //Apply to main camera.
            Camera.main.transform.position = m_CurrentPosition;
            Camera.main.transform.LookAt(cameraTarget);

            Camera.main.fieldOfView = m_Camera.fieldOfView;
            Camera.main.nearClipPlane = m_Camera.nearClipPlane;
            Camera.main.farClipPlane = m_Camera.farClipPlane;
        }
    }

    private void Reposition()
    {
        if (m_Aircraft != null)
        {
            m_CurrentPosition = m_Aircraft.transform.position + (StartOffset);

            m_Velocity = m_Aircraft_Rigidbody.velocity;
            m_Velocity.y = 0.0f;

            m_CurrentTime = 0.0f;
        }
    }
}
