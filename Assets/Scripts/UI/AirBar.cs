using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AirBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color safe;
    [SerializeField] private Color danger;

    [SerializeField] private float threshold = 0.2f;
    [SerializeField] private float safeLimit = 0.2f;

    public void SetAir(float air) {
        // Fix to max value if air is close enough
        if(slider.maxValue-air < threshold) air = slider.maxValue;
        slider.value = air;

        // Calculate approximate minutes and seconds remaining
        double minutes = Math.Truncate(air / 60);
        double seconds = Math.Truncate(air-(minutes*60));

        // Add extra zero to seconds to keep double digits
        string zero = "";
        if(seconds < 10) {
            zero = "0";
        }

        text.text = minutes.ToString() + ":"+ zero + seconds.ToString();

        if(air > safeLimit*slider.maxValue) {
            text.color = safe;
        } else {
            text.color = danger;
        }
    }

    public void SetMaxAir(float maxAir) {
        slider.maxValue = maxAir;
    }
}
