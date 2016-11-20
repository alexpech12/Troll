using UnityEngine;
using System.Collections;

public class AwarenessState : MonoBehaviour {

    public TrollState.State type = TrollState.State.UNAWARE;

    public float movementSpeed = 1.0f;

    public float min = 0.0f;
    public float max = 100.0f;
    public float start = 0.0f;
    public float visionIncrease = 1.0f;
    public float awarenessDecrease = 1.0f;

    public float viewDistance = 7.0f;
    public float viewAngle = 45.0f;
    //public float visionMultipler = 1.0f;
    public float hearingDistance = 20.0f;
    public float hearingIncrease = 1.0f;
    public float smellDistance = 8.0f;
    //public float hearingMultiplier = 1.0f;
    //public float smellMultiplier = 1.0f;

    public bool checkMin(float test_var)
    {
        return test_var <= min;
    }
    public bool checkMax(float test_var)
    {
        return test_var >= max;
    }

}
