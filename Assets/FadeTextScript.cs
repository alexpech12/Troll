using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeTextScript : MonoBehaviour {

    bool running;
    bool complete;

    public Text text;
    public float fadeTime = 5.0f;

	// Use this for initialization
	void Start () {
        running = false;
        complete = false;
	}

    // Update is called once per frame
    float t = 0;
	void Update () {

        if (running)
        {
            t += Time.deltaTime;
            Color tmpColor = text.color;
            tmpColor.a = (fadeTime - t)/fadeTime;
            text.color = tmpColor;
            if (t > fadeTime)
            {
                t = 0;
                running = false;
                complete = true;
            }
        }

	}

    public void StartSequence()
    {
        complete = false;
        running = true;
    }

    public bool SequenceComplete()
    {
        return complete;
    }
}
