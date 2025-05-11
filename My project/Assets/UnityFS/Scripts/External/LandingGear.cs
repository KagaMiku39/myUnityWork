using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/External/Landing Gear")]
[RequireComponent(typeof(Aircraft))]
public class LandingGear : MonoBehaviour
{
    [HideInInspector]
    public InputController LandingGearController;

    public AudioClip ToggleLandingGearClip = null;
    public GameObject ToggleLandingGearAnimationGameObject = null;
    public string ToggleLandingGearAnimationName = "";
    public float GearDownDrag = 0.05f;
    public float GearUpDrag = 0.04f;

    private Rigidbody m_Rigidbody;
    private WheelBase[] m_WheelBaseArray;
    private AudioSource m_ToggleLandingGear;
    private bool m_GearDown = true;
    private bool m_CycleFinished = false;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        m_WheelBaseArray = GetComponentsInChildren<WheelBase>();

        //Add audio source for gear raise and lower if added.
        if (ToggleLandingGearClip != null)
        {
            m_ToggleLandingGear = gameObject.AddComponent<AudioSource>();
            m_ToggleLandingGear.clip = ToggleLandingGearClip;
            m_ToggleLandingGear.volume = 1.0f;
            m_ToggleLandingGear.playOnAwake = false;
            m_ToggleLandingGear.loop = false;
            m_ToggleLandingGear.dopplerLevel = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Listen for input for landing gear.
        if (LandingGearController.GetButtonPressed() == true)
        {
            //Play landing gear audio.
            if (m_ToggleLandingGear != null)
            {
                m_ToggleLandingGear.Play();
            }

            //Toggle gear animation.
            if (ToggleLandingGearAnimationName != "" && ToggleLandingGearAnimationGameObject != null)
            {
                //Play landing gear animation.
                Animation animation = ToggleLandingGearAnimationGameObject.GetComponent<Animation>();
                if (animation != null)
                {
                    animation[ToggleLandingGearAnimationName].wrapMode = WrapMode.ClampForever;
                    animation[ToggleLandingGearAnimationName].speed = m_GearDown ? 1.0f : -1.0f;

                    if (m_CycleFinished == true)
                    {
                        animation[ToggleLandingGearAnimationName].normalizedTime = m_GearDown ? 0.0f : 1.0f;
                    }
                    animation.Play(ToggleLandingGearAnimationName);

                }
            }


            //Change gear down state.
            m_GearDown = !m_GearDown;

            //Set drag.
            m_Rigidbody.drag = m_GearDown ? GearDownDrag : GearUpDrag;

            //Enable / disable wheel colliders based on new gear state.
            if (m_WheelBaseArray != null)
            {
                for (int i = 0; i < m_WheelBaseArray.Length; i++)
                {
                    if (m_WheelBaseArray[i] != null)
                    {
                        float radius = (m_GearDown == true) ? m_WheelBaseArray[i].WheelColliderOriginalRadius : 0.0f;
                        m_WheelBaseArray[i].SetWheelColliderRadius(radius);
                    }
                }
            }

            m_CycleFinished = false;
        }

        //Toggle gear animation.
        if (ToggleLandingGearAnimationName != "" && ToggleLandingGearAnimationGameObject != null)
        {
            //Play landing gear animation.
            Animation animation = ToggleLandingGearAnimationGameObject.GetComponent<Animation>();
            if (animation != null)
            {
                if (animation[ToggleLandingGearAnimationName].normalizedTime < 0.0f || animation[ToggleLandingGearAnimationName].normalizedTime > 1.0f)
                {
                    animation.Stop(ToggleLandingGearAnimationName);
                    if (m_ToggleLandingGear != null)
                    {
                        m_ToggleLandingGear.Stop();
                        m_ToggleLandingGear.time = 0.0f;
                    }

                    m_CycleFinished = true;
                }
            }
        }

    }

}
