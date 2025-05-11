using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/External/Wind Audio")]
[RequireComponent(typeof(Rigidbody))]
public class WindAudio : MonoBehaviour
{
    public AudioClip WindClip = null;
    public float VelocityForMaxVolume = 100.0f;
    public float MaxVolume = 0.5f;

    public float MinPitch = 0.5f;
    public float MaxPitch = 1.5f;

    private AudioSource m_Wind_AudioSource;
    private Rigidbody m_AicraftRigdbody = null;

    void Start()
    {
        if (WindClip != null)
        {
            m_Wind_AudioSource = gameObject.AddComponent<AudioSource>();
            m_Wind_AudioSource.clip = WindClip;
            m_Wind_AudioSource.loop = true;
            m_Wind_AudioSource.volume = 0.0f;
            m_Wind_AudioSource.Play();
            m_Wind_AudioSource.dopplerLevel = 0.0f;
        }

        m_AicraftRigdbody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (m_AicraftRigdbody != null)
        {
            float volume = (m_AicraftRigdbody.velocity.magnitude / VelocityForMaxVolume) * MaxVolume;
            volume = Mathf.Clamp(volume, 0.0f, 1.0f);
            m_Wind_AudioSource.volume = volume;

            float pitch = MinPitch + ((m_AicraftRigdbody.velocity.magnitude / VelocityForMaxVolume) * (MaxPitch - MinPitch));
            pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);
            m_Wind_AudioSource.pitch = pitch;
        }
    }
}
