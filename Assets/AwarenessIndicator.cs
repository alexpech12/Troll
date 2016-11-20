using UnityEngine;
using System.Collections;

public class AwarenessIndicator : MonoBehaviour {

    public Transform troll;
    TrollSense trollSense;
    public Transform awarenessBar;
    public Transform barFrame;

	// Use this for initialization
	void Start () {
        trollSense = troll.GetComponent<TrollSense>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform.position);
        SetAwareness(trollSense.GetAwareness());
	}

    public void SetAwareness(float awareness)
    {
        awareness = Mathf.Clamp(awareness, 0, 100) / 100;
        float newSize = awareness * barFrame.localScale.y * 0.9f;
        Vector3 tempScale = awarenessBar.localScale;
        tempScale.y = newSize;
        awarenessBar.localScale = tempScale;
    }
}
