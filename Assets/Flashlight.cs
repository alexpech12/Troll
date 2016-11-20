using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {

    public Light flashLight;
    public float ambientIntensityOn = 0.0f;
    public float ambientIntensityOff = 1.0f;
    public float adjustmentSpeedOn = 0.5f;
    public float adjustmentSpeedOff = 0.05f;

    bool on = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.F))
        {
            toggleLight();
        }

        float currentAmbient = RenderSettings.ambientIntensity;
        float targetAmbient = on ? ambientIntensityOn : ambientIntensityOff;
        float adjustmentSpeed = on ? adjustmentSpeedOn : adjustmentSpeedOff;
        RenderSettings.ambientIntensity = Mathf.Lerp(currentAmbient, targetAmbient, adjustmentSpeed);
        /*
        if(on)
        {
            RenderSettings.ambientIntensity = (currentAmbient - targetAmbient) * adjustmentSpeed;
        } else
        {
            RenderSettings.ambientIntensity = (targetAmbient - currentAmbient) * adjustmentSpeed;
        }
        */

	}

    void toggleLight()
    {
        on = !on;
        flashLight.enabled = on;
    }

    public void turnOff()
    {
        on = false;
        flashLight.enabled = false;
    }
}
