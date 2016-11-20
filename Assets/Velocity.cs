using UnityEngine;
using System.Collections;

public class Velocity : MonoBehaviour {

    Vector3 lastPosition;

    Vector3 velocity;
    // Use this for initialization
    void Start () {
        velocity = Vector3.zero;
        lastPosition = transform.position;
	}

    // Update is called once per frame
	void Update () {
        Vector3 newPosition = transform.position;
        velocity = (newPosition - lastPosition) / Time.deltaTime;
        lastPosition = newPosition;

	}

    public Vector3 Value()
    {
        return velocity;
    }
}
