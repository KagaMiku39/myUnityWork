using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Dynamics/Prop Wash")]
[RequireComponent(typeof(Wing))]
public class PropWash : AircraftAttachment
{
    public bool[] AffectedSections = null;
    public Engine PropWashSource = null;
    public float PropWashStrength = 0.01f;

    public Vector3 GetPropWash()
    {
        Vector3 propWash = Vector3.zero;

        if (PropWashSource != null)
        {
            propWash = PropWashSource.Thrust;
        }

        return propWash;
    }
    public void OnDrawGizmos()
    {
        ClampEditorValues();
    }

    private void ClampEditorValues()
    {
        Wing wing = this.GetComponent<Wing>();
        if (wing != null)
        {
            if (AffectedSections == null || wing.SectionCount != AffectedSections.Length)
            {
                AffectedSections = new bool[wing.SectionCount];
            }
        }
    }
}

