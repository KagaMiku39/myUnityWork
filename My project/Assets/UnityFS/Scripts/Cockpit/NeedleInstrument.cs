using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityFS/Cockpit/Needle Instrument")]
public class NeedleInstrument : MonoBehaviour
{
    public enum InstrumentSource
    {
        AirspeedKnots = 0,
        AltitudeThousandsFeet,
        AltitudeHundredsFeet,
        RateOfClimbFPM,
        EngineRPM,
        Heading,
        Bank,
    }

    public InstrumentSource Source = InstrumentSource.AirspeedKnots;
    public float MinValue = 0.0f;
    public float MaxValue = 1.0f;
    public float MinAngleDegrees = 0.0f;
    public float MaxAngleDegrees = 360.0f;
    public Vector3 RotationAxis = new Vector3(0.0f, 0.0f, 1.0f);

    private Quaternion m_InitialRotation;
    private Aircraft m_Aircraft;

    void Start()
    {
        RotationAxis.Normalize();

        //Store root rotation.
        m_InitialRotation = this.transform.localRotation;

        //Get aeroplane.
        GameObject root = this.transform.root.gameObject;
        m_Aircraft = root.GetComponent<Aircraft>();
    }

    void Update()
    {

        //Get instrument value.
        float instrumentValue = 0.0f;
        switch (Source)
        {
            case InstrumentSource.AirspeedKnots:
                {
                    instrumentValue = m_Aircraft.GetAirspeedKnots();
                }
                break;

            case InstrumentSource.AltitudeThousandsFeet:
                {
                    instrumentValue = m_Aircraft.GetAltitudeThousandsFeet();
                    float subtractAmount = Mathf.Floor(instrumentValue / 10.0f);
                    instrumentValue -= subtractAmount * 10.0f;
                }
                break;

            case InstrumentSource.AltitudeHundredsFeet:
                {
                    instrumentValue = m_Aircraft.GetAltitudeHundredsFeet();
                    float subtractAmount = Mathf.Floor(instrumentValue / 10.0f);
                    instrumentValue -= subtractAmount * 10.0f;
                }
                break;

            case InstrumentSource.RateOfClimbFPM:
                {
                    instrumentValue = m_Aircraft.GetRateOfClimbFPM();
                }
                break;

            case InstrumentSource.EngineRPM:
                {
                    instrumentValue = m_Aircraft.GetEngineRPM();
                }
                break;

            case InstrumentSource.Heading:
                {
                    instrumentValue = m_Aircraft.GetHeadingDegrees();
                }
                break;

            case InstrumentSource.Bank:
                {
                    instrumentValue = m_Aircraft.GetBankDegrees();

                    if (instrumentValue > 180.0f)
                    {
                        instrumentValue = -(360.0f - instrumentValue);
                    }

                }
                break;
        }

        //Allow force max value to quickly set up instruments when editing them.
        instrumentValue = Mathf.Clamp(instrumentValue, MinValue, MaxValue);

        float valueDelta = (instrumentValue - MinValue) / (MaxValue - MinValue);
        valueDelta = Mathf.Clamp(valueDelta, 0.0f, 1.0f);

        float angleDeltaDegrees = MinAngleDegrees + ((MaxAngleDegrees - MinAngleDegrees) * valueDelta);

        this.transform.localRotation = m_InitialRotation;
        this.transform.Rotate(RotationAxis, angleDeltaDegrees);
    }
}
