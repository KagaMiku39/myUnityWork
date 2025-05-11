using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Dynamics/Center Of Gravity")]
public class CenterOfGravity : AircraftAttachment
{
    private GameObject m_Root;
    private Rigidbody m_Root_Rigidbody;

    public void Start()
    {
        //Get the topmost parent
        m_Root = this.transform.root.gameObject;
        m_Root_Rigidbody = m_Root.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        //Update center of mass.
        if (m_Root_Rigidbody != null)
        {
            m_Root_Rigidbody.centerOfMass = this.transform.localPosition;
        }

        //Debug draw.
        Debug.DrawLine(this.transform.position - (this.transform.up * 1.0f), this.transform.position + (this.transform.up * 1.0f), Color.blue);
        Debug.DrawLine(this.transform.position - (this.transform.right * 1.0f), this.transform.position + (this.transform.right * 1.0f), Color.blue);
    }

    public void OnDrawGizmos()
    {
        //Draw sphere at cg position.
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}

