using UnityEngine;
using System.Collections;

public class TrollMovement : MonoBehaviour {

    enum MovementState
    {
        IDLE,
        MOVING,
        TURNING
    }

    MovementState state = MovementState.IDLE;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        switch(state)
        {
            case MovementState.IDLE:

                // Do not move troll.

                break;
            case MovementState.MOVING:

                // Move troll forward by move speed and rotate by turn speed

                break;
            case MovementState.TURNING:

                // Rotate troll by turn speed

                break;
        }

	}
}
