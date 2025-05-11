using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Cockpit/Attitude Indicator")]
public class AttitudeIndicator : MonoBehaviour
{
    public void Update()
    {
        Vector3 forward = transform.root.forward;
        forward.y = 0.0f;
        forward.Normalize();

        this.transform.LookAt(transform.position + forward);
        this.transform.localEulerAngles = new Vector3(-this.transform.localEulerAngles.x,
                                                    this.transform.localEulerAngles.y,
                                                    this.transform.localEulerAngles.z);
    }
}
