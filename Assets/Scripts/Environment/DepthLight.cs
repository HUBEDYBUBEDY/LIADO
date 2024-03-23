using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DepthLight : MonoBehaviour
{
    [SerializeField] SubmarineManager manager;
    [SerializeField] private Light2D lightObj;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float activationDepth;

    void Awake()
    {
        maxIntensity = lightObj.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        activationDepth = manager.activationDepth;
        float y = lightObj.transform.position.y;

        if (y >= 0) {
            lightObj.intensity = 0;
        } else if (y <= -activationDepth) {
            lightObj.intensity = maxIntensity;
        } else {
            lightObj.intensity = maxIntensity * (Math.Abs(y)/activationDepth);
        }
    }
}
