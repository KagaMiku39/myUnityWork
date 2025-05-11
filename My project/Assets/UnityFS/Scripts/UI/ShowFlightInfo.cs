using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFlightInfo : MonoBehaviour
{
    public Aircraft Aircraft;
    public Text SpeedText;
    public Text AltitudeText;
    public Text ClimbRateText;


    void Update()
    {
        if (Aircraft == null)
        {
            return;
        }

        if (Aircraft.AircraftEnabled == false)
        {
            return;
        }

        if (SpeedText != null)
        { 
            SpeedText.text = string.Format("Speed: {0}kts", ((int)Aircraft.GetAirspeedKnots()).ToString());
        }

        if (AltitudeText != null)
        {
            AltitudeText.text = string.Format("Altitude: {0}ft", ((int)Aircraft.GetAltitude()).ToString());
        }

        if (ClimbRateText != null)
        {
            ClimbRateText.text = string.Format("ClimbRate: {0}fpm", ((int)Aircraft.GetRateOfClimbFPM()).ToString());
        }
    }
    public void GetMoreAsset()
    {
        Application.OpenURL("https://gamedev3d.com/");
    }
}
