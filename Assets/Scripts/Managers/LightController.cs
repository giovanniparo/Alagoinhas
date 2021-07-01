using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private Light2D urpLight;
    [SerializeField] private float minRange;
    [SerializeField] private float maxRange;
    [SerializeField] private float flickerSpeed;

    private void Start()
    {
        urpLight = GetComponent<Light2D>();
    }

    private void Update()
    {
        urpLight.intensity = Mathf.Lerp(minRange, maxRange, Mathf.PingPong(Time.time, flickerSpeed));
    }
}
