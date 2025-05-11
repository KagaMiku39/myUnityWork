using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/External/Stall Warner")]
[RequireComponent(typeof(Wing))]
public class StallWarner : MonoBehaviour
{
    public AudioClip StallWarnerClip = null;
    public float AngleOfAttackToTrigger = 12.0f;

    private Wing m_AttachedWing = null;
    private AudioSource m_StallWarnerSource = null;
    private Rigidbody m_Root_Rigidbody = null;

    void Start()
    {
        m_Root_Rigidbody = transform.root.gameObject.GetComponent<Rigidbody>();

        m_AttachedWing = GetComponent<Wing>();

        if (StallWarnerClip != null)
        {
            m_StallWarnerSource = gameObject.AddComponent<AudioSource>();
            m_StallWarnerSource.clip = StallWarnerClip;
            m_StallWarnerSource.volume = 1.0f;
            m_StallWarnerSource.loop = true;
            m_StallWarnerSource.dopplerLevel = 0.0f;
        }
    }

    void Update()
    {
        if (m_AttachedWing != null)
        {
            if (m_AttachedWing.AngleOfAttack >= AngleOfAttackToTrigger)
            {
                if (!m_StallWarnerSource.isPlaying)
                {
                    if (m_Root_Rigidbody.velocity.magnitude > 0.1f) //only play if we are moving
                    {
                        m_StallWarnerSource.Play();
                    }
                }
            }
            else
            {
                if (m_StallWarnerSource.isPlaying)
                {
                    m_StallWarnerSource.Stop();
                }

            }
        }
    }
}
