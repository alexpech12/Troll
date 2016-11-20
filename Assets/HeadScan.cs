using UnityEngine;
using System.Collections;

public class HeadScan : MonoBehaviour {

    public float scanSpeed = 1.0f;
    public float scanAngle = 90.0f;

    bool scanning = false;
    bool stopScanning = false;

    float headAngle = 0.0f;
    float headAnglePrev = 0.0f;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    float t = 0f;
	void Update () {
	
        if(scanning)
        {
            t += Time.deltaTime * scanSpeed;
            headAngle = scanAngle * Mathf.Sin(2 * Mathf.PI * t);

            if (stopScanning)
            {
                if ((headAngle >= 0 && headAnglePrev <= 0) || (headAngle <= 0 && headAnglePrev >= 0))
                {
                    headAngle = 0;
                    scanning = false;
                }
            }

            Vector3 eulerRotation = transform.localEulerAngles;
            eulerRotation.y = headAngle;
            transform.localEulerAngles = eulerRotation;

        }
        headAnglePrev = headAngle;

    }

    public void StartScanning()
    {
        scanning = true;
        stopScanning = false;
        t = 0f;
    }

    public void StopScanning()
    {
        stopScanning = true;
    }
}
