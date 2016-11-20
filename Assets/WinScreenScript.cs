using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinScreenScript : MonoBehaviour {

    public Image background;
    public Text text1;
    public Text text2;
    public Text text3;

    public Material daySkybox;
    public Light sun;
    public Color dayLightColor;

    bool running = false;

    Material savedSkybox;
    Color savedLightColor;
    float savedLightIntensity;

    float[] times = new float[]
    {
        2, // Fade in background
        3, // Fade in text 1
        2, // Pause
        3, // Fade in text 2
        4, // Pause
        3, // Fade in text 3
        4, // Pause
        5  // Fade out 
    };

    int time_index = 0;

	// Use this for initialization
	void Start ()
    {
        SetAlpha(background, 0);
        SetAlpha(text1, 0);
        SetAlpha(text2, 0);
        SetAlpha(text3, 0);

    }

    // Update is called once per frame
    float t = 0;
	void Update () {
	    if(running)
        {
            t += Time.deltaTime;

            float tt = t / times[time_index];
            

            switch(time_index) {
                case 0: // Fade in background
                    SetAlpha(background, Mathf.Lerp(0, 1, tt));
                    break;
                case 1: // Fade in text
                    SetAlpha(text1, Mathf.Lerp(0, 1,tt));
                    // Switch to daylight
                    SwitchToDaylight();
                    break;
                case 2: // Pause
                    SwitchToDaylight();
                    break;
                case 3: // Fade in text 2
                    SetAlpha(text2, Mathf.Lerp(0, 1, tt));
                    break;
                case 4: // Pause

                    break;
                case 5: // Fade in text 3
                    SetAlpha(text3, Mathf.Lerp(0, 1, tt));
                    break;
                case 6: // Pause

                    break;
                case 7: // Fade out
                    SetAlpha(background, Mathf.Lerp(1, 0, tt));
                    SetAlpha(text1, Mathf.Lerp(1, 0, tt));
                    SetAlpha(text2, Mathf.Lerp(1, 0, tt));
                    SetAlpha(text3, Mathf.Lerp(1, 0, tt));
                    break;
                default:
                    time_index = 7;
                    break;
            }

            if (t > times[time_index])
            {
                t = 0;
                time_index++;
                if (time_index >= times.Length)
                {
                    // Sequence done
                    time_index = 0;
                    running = false;
                }
            }

        }
	}

    public void StartSequence()
    {
        running = true;
    }

    void SetAlpha(Image image, float a)
    {
        Color tmpColor = image.color;
        tmpColor.a = a;
        image.color = tmpColor;
    }
    void SetAlpha(Text image, float a)
    {
        Color tmpColor = image.color;
        tmpColor.a = a;
        image.color = tmpColor;
    }

    void SwitchToDaylight()
    {
        // Switch to daylight
        savedSkybox = RenderSettings.skybox;
        savedLightColor = sun.color;
        savedLightIntensity = sun.intensity;

        RenderSettings.skybox = daySkybox;
        sun.color = dayLightColor;
        sun.intensity = 1.0f;
        Component fog = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent("GlobalFog");
        fog.GetType().GetProperty("enabled").SetValue(fog, false, null);
        RenderSettings.ambientIntensity = 2.5f;
        Flashlight fl = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<Flashlight>();
        fl.turnOff();
        fl.enabled = false;

        // Turn trolls to stone
        GameObject[] trolls = GameObject.FindGameObjectsWithTag("Troll");
        foreach(GameObject troll in trolls)
        {
            troll.GetComponent<TrollAI>().TurnToStone();
        }
    }
}
