using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Base/AircraftCamera")]
public class AircraftCamera : MonoBehaviour
{
    protected bool m_CameraActive = false;

    //Base class for all cameras.
    public void SetCameraActive(bool active)
    {
        if (m_CameraActive != active)
        {
            m_CameraActive = active;

            if (m_CameraActive == true)
            {
                OnCameraEnabled();
            }
            else
            {
                OnCameraDisabled();
            }
        }
    }

    protected virtual void OnCameraEnabled() { }
    protected virtual void OnCameraDisabled() { }

}
