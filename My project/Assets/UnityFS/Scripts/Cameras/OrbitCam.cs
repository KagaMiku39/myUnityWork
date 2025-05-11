using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Cameras/Orbit Cam")]
[RequireComponent(typeof(Camera))]
public class OrbitCam : AircraftCamera
{
    public float CameraZOffset = 10.0f;
    public float CameraYOffset = 2.0f;

    private bool m_FirstClick = false;
    private Vector3 m_MouseStart;
    private float m_CameraAngle = 180.0f;

    private Camera m_Camera;
    private Aircraft m_Aircraft;

    public void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_Camera.enabled = false;

        m_Aircraft = transform.root.gameObject.GetComponent<Aircraft>();
    }

    public void Update()
    {
        if (m_CameraActive == true && m_Aircraft != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (m_FirstClick == true)
                {
                    m_MouseStart = Input.mousePosition;
                    m_FirstClick = false;
                }
                m_CameraAngle += (Input.mousePosition - m_MouseStart).x * Time.deltaTime;
            }
            else
            {
                m_FirstClick = true;
            }

            Vector3 zAxis = m_Aircraft.transform.forward;
            zAxis.y = 0.0f;
            zAxis.Normalize();
            zAxis = Quaternion.Euler(0, m_CameraAngle, 0) * zAxis;

            Vector3 cameraPosition = m_Aircraft.transform.position;
            cameraPosition += zAxis * CameraZOffset;
            cameraPosition += new Vector3(0.0f, 1.0f, 0.0f) * CameraYOffset;

            Vector3 cameraTarget = m_Aircraft.transform.position;

            //Apply to main camera.
            Camera.main.transform.position = cameraPosition;
            Camera.main.transform.LookAt(cameraTarget);

            Camera.main.fieldOfView = m_Camera.fieldOfView;
            Camera.main.nearClipPlane = m_Camera.nearClipPlane;
            Camera.main.farClipPlane = m_Camera.farClipPlane;
        }
    }
}
