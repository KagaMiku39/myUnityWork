using UnityEngine;
using System.Collections;


[RequireComponent(typeof(WheelCollider))]
public abstract class WheelBase : MonoBehaviour
{
    [HideInInspector]
    public InputController Controller;

    public Transform wheelModel;
    public RotationAxis wheelRotationAxis;

    public AudioClip wheelRollClip = null;
    public float rpmForMaxVolume = 1000.0f;
    public float maxVolume = 0.25f;

    protected Aircraft m_Owner;
    protected WheelCollider m_WheelCollider;

    protected AudioSource m_AudioSource;

    protected Vector3 m_LastFramePos;
    protected Vector3 m_Velocity;
    public float WheelColliderOriginalRadius { get; set; }

    protected virtual void Awake()
    {
        m_Owner = this.transform.root.GetComponent<Aircraft>();
        m_WheelCollider = this.GetComponent<WheelCollider>();
    }

    protected virtual void Start()
    {
        if (m_WheelCollider == null)
        {
            return;
        }

        WheelColliderOriginalRadius = m_WheelCollider.radius;

        CalculateVelocityInitialize();

        WheelAudioInitalize();
    }


    protected virtual void Update()
    {
        if (m_WheelCollider == null)
        {
            return;
        }

        CalculateVelocityUpdate();

        WheelLogicUpdate();
    }

    public abstract void WheelLogicUpdate();

    private void WheelAudioInitalize()
    {
        if (wheelRollClip != null)
        {
            m_AudioSource = gameObject.AddComponent<AudioSource>();
            m_AudioSource.clip = wheelRollClip;
            m_AudioSource.loop = true;
            m_AudioSource.volume = 0.0f;
            m_AudioSource.dopplerLevel = 0.0f;

            m_AudioSource.Play();
        }
    }

    protected void CalculateVelocityInitialize()
    {
        m_LastFramePos = this.transform.position;
        m_Velocity = Vector3.zero;
    }

    protected void CalculateVelocityUpdate()
    {
        Vector3 distance = this.transform.position - m_LastFramePos;
        m_Velocity = distance / Time.deltaTime;
        m_LastFramePos = this.transform.position;
    }

    public void SetWheelColliderRadius(float radius)
    {
        m_WheelCollider.radius = radius;
    }
}
