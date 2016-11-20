using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrollSense : MonoBehaviour {
    
    public enum VisionState
    {
        UNSEEN,
        SEEN
    }

    public enum HearingState
    {
        UNHEARD,
        HEARD
    }

    VisionState visionState = VisionState.UNSEEN;
    HearingState hearingState = HearingState.UNHEARD;

    public Transform head;
    Transform player;

    //public float viewDistance = 7.0f;
    //public float viewAngle = 45.0f;
    //public float smellDistance = 10.0f;
   // public float hearingDistance = 8.0f;

    //public float visionMultiplier = 1.0f;
    //public float hearingMultiplier = 1.0f;
    //public float smellMultiplier = 1.0f;

    //public float awarenessReductionRate = 0.1f;

    float awareness = 0;
    //float hearingAwareness = 0;
    //float awareness = 0;

    float distToPlayer = 0;
    float angleToPlayer = 0;

    TrollState trollState;

    // Use this for initialization
    void Start () {

        trollState = GetComponent<TrollState>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {





        // Get awareness value for currently active state
        AwarenessState currentState = trollState.GetStateComponent();
        //if(currentState.atMax())
        //{
        //
        //}





        // Update awareness

        distToPlayer = Vector3.Distance(transform.position, player.position);
        angleToPlayer = Quaternion.FromToRotation(head.forward, player.position - transform.position).eulerAngles.y;
        angleToPlayer = angleToPlayer > 180 ? angleToPlayer = angleToPlayer - 360 : angleToPlayer;
        
        bool awarenessUpdated = UpdateVisionAwareness()
            || UpdateHearingAwareness()
            || UpdateSmellAwareness();

        
        if(!awarenessUpdated)
        {
            awareness = awareness - currentState.awarenessDecrease;
        }
        
        visionState = VisionState.UNSEEN;
        if(currentState.checkMin(awareness))
        {
            awareness = currentState.min;
        }

        if (currentState.checkMax(awareness)) {
            awareness = currentState.max;
            visionState = VisionState.SEEN;
        }
    }

    public bool Seen()
    {
        return visionState == VisionState.SEEN;
    }

    public float GetAwareness()
    {
        return awareness;
    }

    public VisionState GetVisionState()
    {
        return visionState;
    }

    public HearingState GetHearingState()
    {
        return hearingState;
    }

    public bool CheckVisionAwareness()
    {
        AwarenessState currentState = trollState.GetStateComponent();
        float viewDistance = currentState.viewDistance;
        float viewAngle = currentState.viewAngle;
        if (distToPlayer < viewDistance)
        {
            if (angleToPlayer > -viewAngle && angleToPlayer < viewAngle)
            {
                // Check line of sight
                Ray ray = new Ray(head.position, player.position - head.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, viewDistance))
                {
                    if (hit.transform == player.transform)
                    {
                        return true;
                    }
                }

            }
        }
        return false;
    }

    public bool CheckHearingAwareness()
    {
        AwarenessState currentState = trollState.GetStateComponent();
        float hearingDistance = currentState.hearingDistance;
        if (distToPlayer < hearingDistance)
        {
            return true;
        }
        return false;
    }

    public bool CheckSmellAwareness()
    {
        // TODO:
        return false;

        /*
        if (distToPlayer < smellDistance)
        {
            float dA = smellMultiplier * (1 - (distToPlayer / smellDistance));
            awareness += dA;
            return true;
        }
        return false;
        */
    }

    bool UpdateVisionAwareness()
    {
        if(CheckVisionAwareness())
        {
            // Get values from awareness state
            AwarenessState currentState = trollState.GetStateComponent();
            float multiplier = currentState.visionIncrease;
            float viewDistance = currentState.viewDistance;
            float viewAngle = currentState.viewAngle;

            float dA_dist = multiplier * (1 - (distToPlayer / viewDistance));
            float dA_angle = multiplier * (1 - (Mathf.Abs(angleToPlayer) / viewAngle));
            float dA = dA_dist + dA_angle;
            awareness += dA;
            return true;
        }
        return false;
    }

    bool UpdateHearingAwareness()
    {
        if (CheckHearingAwareness())
        {
            // TODO:
            AwarenessState currentState = trollState.GetStateComponent();
            float multiplier = currentState.hearingIncrease;
            float hearingDistance = currentState.hearingDistance;

            float playerSpeed = player.GetComponent<Velocity>().Value().magnitude;
            Debug.Log("Player velocity: " + playerSpeed);
            
            float dA_dist = multiplier * (1 - (distToPlayer / hearingDistance)) * playerSpeed - currentState.awarenessDecrease;
            awareness += dA_dist;

            return true;
        }
        return false;
    }

    bool UpdateSmellAwareness()
    {

        // TODO:

        /*
        if (CheckSmellAwareness())
        {
            float dA = smellMultiplier * (1 - (distToPlayer / smellDistance));
            awareness += dA;
            return true;
        }
        */
        return false;
        
    }

    public void ResetAwareness()
    {
        awareness = trollState.GetStateComponent().start;
    }
}
