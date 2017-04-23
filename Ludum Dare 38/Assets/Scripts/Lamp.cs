using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Lamp : MonoBehaviour {

    Light _light;
    float intensity;
    bool isOn;

    void Start () {
        _light = GetComponent<Light>();
        intensity = _light.intensity;

        isOn = false;
        _light.intensity = 0;
    }
	
    public void ToggleSwitch()
    {
        isOn = !isOn;
        if (isOn)
            _light.intensity = intensity;
        else
            _light.intensity = 0;
    }
}
