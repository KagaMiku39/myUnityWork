using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Base/AircraftAttachment")]
public class AircraftAttachment : MonoBehaviour
{
    // Base class for all UnityFS attachable objects.
    protected bool m_Controllable = false;

    public void SetControllable(bool enable)
    {
        m_Controllable = enable;
    }
}
