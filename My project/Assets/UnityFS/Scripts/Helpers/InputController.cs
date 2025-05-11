using UnityEngine;

#if !NETFX_CORE
using System.Reflection;
#endif

[System.Serializable]
public class InputController
{
    public enum InputSource
    {
        InputManager = 0,
        Manual
    }

    [SerializeField]
    public InputSource inputSource = InputSource.InputManager;

    [SerializeField]
    public bool Invert = false;

    //Input manager vars
    [SerializeField]
    public string AxisName = "";
    public string ButtonName = "";

    //Manual vars
    private float m_ManualInput = 0.0f;
    private bool m_ManualButtonPressed = false;
    private bool m_ButtonPressedLastFrame = false;
    private bool m_ButtonWasPressedThisFrame = false;

    public void SetInputSource(InputSource inputSource)
    {
        this.inputSource = inputSource;
    }

    public void SetManualInputMinusOneToOne(float input)
    {
        m_ManualInput = Mathf.Clamp(input, -1.0f, 1.0f);

        if (inputSource != InputSource.Manual)
        {
            Debug.LogWarning("InputController - Setting manual input when not in manual input mode.");
        }
    }

    public void SetManualInputButtonPressed(bool isPressed)
    {
        m_ManualButtonPressed = isPressed;
    }

    public float GetAxisInput()
    {
        float inputValue = 0.0f;

        switch (inputSource)
        {
            case InputSource.InputManager:
                {
                    if (AxisName != "")
                    {
                        inputValue = Input.GetAxis(AxisName);
                    }
                    else
                    {
                        Debug.LogWarning("InputController - Trying to get input from InputManager with no axis defined.");
                    }
                }
                break;
           

            case InputSource.Manual:
                {
                    inputValue = m_ManualInput;
                }
                break;
        }

        inputValue = Mathf.Clamp(inputValue, -1.0f, 1.0f);

        if (Invert == true)
        {
            inputValue = -inputValue;
        }

        return inputValue;
    }

    public bool GetButton()
    {
        bool inputValue = false;

        switch (inputSource)
        {
            case InputSource.InputManager:
                {
                    if (ButtonName != "")
                    {
                        inputValue = Input.GetButton(ButtonName);
                    }
                    else
                    {
                        Debug.LogWarning("InputController - Trying to get input from InputManager with no button defined.");
                    }
                }
                break;

            case InputSource.Manual:
                {
                    inputValue = m_ManualButtonPressed;
                }
                break;
        }

        if (Invert == true)
        {
            inputValue = !inputValue;
        }

        //Also support button pressed events not just holding..
        if (m_ButtonPressedLastFrame == false && inputValue == true)
        {
            m_ButtonWasPressedThisFrame = true;
        }
        else
        {
            m_ButtonWasPressedThisFrame = false;
        }

        m_ButtonPressedLastFrame = inputValue;

        return inputValue;
    }

    public bool GetButtonPressed()
    {
        //Needs to poll get button in order to check this is a bit dodgy really..
        //(Because there is no update for input controller)
        GetButton();

        return m_ButtonWasPressedThisFrame;
    }



}