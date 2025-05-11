using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Cameras/Tower Cam")]
[RequireComponent(typeof(Camera))]
public class TowerCam : AircraftCamera
{
    public string ZoomInputAxis = "";
    public float ZoomSpeed = 100.0f;
    public float MinFOV = 1.0f;
    public float MaxFOV = 179.0f;

    private Camera m_Camera;
    private Aircraft m_Aircraft;
    private Vector3 m_IntitialWorldPosition;

    public void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_Aircraft = transform.root.gameObject.GetComponent<Aircraft>();
        m_IntitialWorldPosition = transform.position;
    }

    public void Update()
    {
        if (m_CameraActive == true &&  m_Aircraft != null)
        {
            //Reset back to correct world position.
            transform.position = m_IntitialWorldPosition;

            //Look at target.
            transform.LookAt(m_Aircraft.transform.position, Vector3.up);

            //Do zoom.
            if (ZoomInputAxis != "")
            {
                m_Camera.fieldOfView += -Input.GetAxis("CameraZoom") * ZoomSpeed * Time.deltaTime;
            }
            m_Camera.fieldOfView = Mathf.Clamp(m_Camera.fieldOfView, MinFOV, MaxFOV);

            //Apply to main camera.
            Camera.main.transform.position = transform.position;
            Camera.main.transform.rotation = transform.rotation;

            Camera.main.fieldOfView = m_Camera.fieldOfView;
            Camera.main.nearClipPlane = m_Camera.nearClipPlane;
            Camera.main.farClipPlane = m_Camera.farClipPlane;

        }
    }
}
