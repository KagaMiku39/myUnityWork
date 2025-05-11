using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Cameras/Cockpit Cam")]
[RequireComponent(typeof(Camera))]
public class CockpitCam : AircraftCamera
{
    public string ZoomInputAxis = "";
    public float LookXSpeed = 10.0f;
    public float LookYSpeed = -10.0f;
    public float ZoomSpeed = 100.0f;

    public float MinFOV = 1.0f;
    public float MaxFOV = 179.0f;

    private Camera m_Camera;
    private Quaternion m_StartOrientation;
    private Vector3 m_LastMousePosition;

    private float m_YRotation;
    private float m_XRotation;

    // Use this for initialization
    public void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_Camera.enabled = false;

        m_StartOrientation = gameObject.transform.localRotation;
        m_LastMousePosition = Input.mousePosition;
        m_YRotation = 0.0f;
        m_XRotation = 0.0f;
    }

    // Update is called once per frame
    public void Update()
    {
        if (m_CameraActive == true)
        {
            //Look around.
            if (Input.GetMouseButton(0))
            {
                Vector3 mouseDelta = Input.mousePosition - m_LastMousePosition;
                m_YRotation += mouseDelta.x * LookXSpeed * Time.deltaTime;
                m_XRotation += mouseDelta.y * LookYSpeed * Time.deltaTime;

                gameObject.transform.localRotation = m_StartOrientation;
                gameObject.transform.Rotate(transform.parent.right, m_XRotation, Space.World);
                gameObject.transform.Rotate(transform.parent.up, m_YRotation, Space.World);
            }

            //Zoom.
            if (ZoomInputAxis != "")
            {
                m_Camera.fieldOfView += -Input.GetAxis(ZoomInputAxis) * ZoomSpeed * Time.deltaTime;
            }
            m_Camera.fieldOfView = Mathf.Clamp(m_Camera.fieldOfView, MinFOV, MaxFOV);
            m_LastMousePosition = Input.mousePosition;

            //Apply to main camera.
            Camera.main.transform.position = transform.position;
            Camera.main.transform.rotation = transform.rotation;

            Camera.main.fieldOfView = m_Camera.fieldOfView;
            Camera.main.nearClipPlane = m_Camera.nearClipPlane;
            Camera.main.farClipPlane = m_Camera.farClipPlane;
        }
    }

}
