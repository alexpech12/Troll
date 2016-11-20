using UnityEngine;
//using System;
using System.Collections.Generic;

public class TrollAI : MonoBehaviour {

    public Transform mesh;
    public Transform body;
    public Transform headPivot;
    public Transform head;
    public Transform headBone;
    public Transform awarenessIndicator;

    public Transform playerGhostPrefab;

    public float walkSpeed = 2.0f;

    public float captureDistance = 3.0f;

    public GameObject eye_left;
    public GameObject eye_right;

    float awareness = 0;

    TrollState state;
    TrollSense sense;
    PlayerGhostScript target;
    NavMeshAgent agent;
    Animator anim;

    Vector3 return_position;
    bool investigate_return = false;
    Vector3 last_known_position;
    float start_search_radius = 1.0f;
    float search_radius = 2.0f;
    float search_radius_increase = 2.0f;
    float max_search_radius = 50.0f;

    bool waiting = false;

    Dictionary<TrollState.State, Color> DebugStateColor = new Dictionary<TrollState.State, Color>()
    {
        { TrollState.State.DORMANT, Color.green },
        { TrollState.State.UNAWARE, Color.white },
        { TrollState.State.INVESTIGATE, Color.blue },
        { TrollState.State.AWARE, Color.red },
        { TrollState.State.LOST_SIGHT, Color.yellow },
        { TrollState.State.SEARCHING, Color.cyan }
    };

	// Use this for initialization
	void Start () {
        sense = GetComponent<TrollSense>();
        state = GetComponent<TrollState>();
        agent = GetComponent<NavMeshAgent>();
        anim = mesh.GetComponent<Animator>();
        Transform playerGhost = Instantiate(playerGhostPrefab);
        target = playerGhost.GetComponent<PlayerGhostScript>();
        target.troll = transform;
        target.UpdateRandomPosition();
        //agent.updatePosition = false;
    }

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    Vector2 lastVelocity = Vector2.zero;
    float t = 0;
    float lastRotationY = 0;
    void Update()
    {
        body.GetComponent<MeshRenderer>().material.color = DebugStateColor[state.GetState()];
        head.GetComponent<MeshRenderer>().material.color = DebugStateColor[state.GetState()];

        // Update nav agent
        if (agent != null)
        {
            if (agent.enabled)
            {
                agent.destination = target.transform.position;
            }
        }

        float velocityMagnitude = agent.enabled ? agent.velocity.magnitude : 0;
        anim.SetFloat("Forward", velocityMagnitude * 0.12f);
        Vector2 forward2D = new Vector2(transform.forward.x, transform.forward.z).normalized;
        Vector2 steer2D = new Vector2(agent.steeringTarget.x,agent.steeringTarget.z).normalized;
        
        float turning = Mathf.DeltaAngle(lastRotationY,transform.rotation.eulerAngles.y) / (180*Time.deltaTime);
        lastRotationY = transform.rotation.eulerAngles.y;
        anim.SetFloat("Turn", turning);
        
    }

    // Update is called once per frame
	void LateUpdate () {


        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().getState() == GameController.GameState.RUNNING)
        {
            UpdateState();
        }

    }

    float t_decision = 0f;
    void UpdateState()
    {
        // Next state logic

        t_decision += Time.deltaTime;

        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        Vector3 horizontalVectorToTarget = target.transform.position - transform.position;
        horizontalVectorToTarget.y = 0;
        float horizontalDistanceToTarget = horizontalVectorToTarget.magnitude;
        bool atTarget = horizontalDistanceToTarget < captureDistance;

        bool makeDecision = false;
        if(t_decision > 2.0f)
        {
            makeDecision = true;
            t_decision = 0;
        }

        switch (state.GetState())
        {
            case TrollState.State.DORMANT:
                if(sense.Seen())
                {
                    SetState(TrollState.State.INVESTIGATE);
                    target.UpdatePosition();
                    return_position = transform.position;
                    investigate_return = false;
                    turnToFace(2, "endTurn");
                    agent.enabled = true;
                } else if (makeDecision && Random.Range(0f,1f) > 0.9f)
                {
                    // Wake up troll
                    SetState(TrollState.State.UNAWARE);
                    //target.UpdateRandomPosition();
                    agent.enabled = true;
                }
                break;
            case TrollState.State.UNAWARE:
                
                if (sense.Seen())
                {
                    SetState(TrollState.State.INVESTIGATE);
                    target.UpdatePosition();
                    return_position = transform.position;
                    investigate_return = false;
                    turnToFace(2, "endTurn");
                }
                else if (atTarget)
                {
                    target.UpdateRandomPosition();
                } else if(makeDecision && Random.Range(0f,1f) > 0.9f)
                {
                    SetState(TrollState.State.DORMANT);
                    agent.enabled = false;
                }

                break;
            case TrollState.State.INVESTIGATE:

                if(sense.Seen())
                {
                    SetState(TrollState.State.AWARE);
                    target.UpdatePosition();
                    turnToFace(1.5f, "endTurn");
                    stopHeadScanning();
                    GetComponent<TrollAudio>().Roar();
                }

                if(atTarget)
                {
                    if(investigate_return)
                    {
                        SetState(TrollState.State.UNAWARE);
                        target.UpdateRandomPosition();
                        stopHeadScanning();
                    } else
                    {
                        investigate_return = true;
                        target.UpdatePosition(return_position);
                        waitForTime(3, "endWait");
                        startHeadScanning();
                    }
                }
                break;
            case TrollState.State.AWARE:

                target.UpdatePosition();
                if(atTarget && distanceToTarget < captureDistance)
                {
                    // Caught!!!
                    Debug.Log("Player caught!!!");
                    killPlayer();
                }
                
                if (!sense.Seen())
                {
                    SetState(TrollState.State.LOST_SIGHT);
                    last_known_position = target.transform.position;
                }

                break;
            case TrollState.State.LOST_SIGHT:

                target.UpdateVelocityPosition();
                last_known_position = target.transform.position;
                if(sense.Seen())
                {
                    SetState(TrollState.State.AWARE);
                    target.UpdatePosition();
                    GetComponent<TrollAudio>().Roar();
                }
                else if(atTarget)
                {
                    SetState(TrollState.State.SEARCHING);
                    search_radius = start_search_radius;
                    target.UpdateRandomPosition(target.transform.position,search_radius);
                    startHeadScanning();
                }

                break;
            case TrollState.State.SEARCHING:

                if(sense.Seen())
                {
                    SetState(TrollState.State.INVESTIGATE);
                    target.UpdatePosition();
                    return_position = transform.position;
                    investigate_return = false;
                } else if(atTarget)
                {
                    search_radius += search_radius_increase;
                    if (search_radius > max_search_radius)
                    {
                        SetState(TrollState.State.UNAWARE);
                        stopHeadScanning();
                    }
                    else
                    {
                        target.UpdateRandomPosition(target.transform.position, search_radius);
                    }
                }

                break;
        }
    }

    void SetState(TrollState.State nextState)
    {
        state.SetState(nextState);
        sense.ResetAwareness();
        GetComponent<NavMeshAgent>().speed = state.GetStateComponent().movementSpeed;
    }

    void waitForTime(float seconds, string callback)
    {
        // Disable nav mesh agent component for given time
        GetComponent<NavMeshAgent>().enabled = false;
        Invoke(callback, seconds);
    }

    void endWait()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().getState() == GameController.GameState.RUNNING)
        {
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    void turnToFace(float seconds, string callback)
    {
        GetComponent<NavMeshAgent>().speed = 0.2f;
        Invoke(callback, seconds);
    }

    void endTurn()
    {
        GetComponent<NavMeshAgent>().speed = state.GetStateComponent().movementSpeed;
    }

    void startHeadScanning()
    {
        headPivot.GetComponent<HeadScan>().StartScanning();
    }

    void startHeadScanning(float seconds)
    {
        startHeadScanning();
        Invoke("stopHeadScanning", seconds);
    }

    void stopHeadScanning()
    {
        headPivot.GetComponent<HeadScan>().StopScanning();
    }
    
    float blendTime = 0.0f;
    float blendTimeTotal = 1.0f;
    void blendDormantAnimation()
    {
        if(blendTime < blendTimeTotal)
        {
            blendTime += Time.deltaTime;
            // Set blend value
            float blendValue = blendTime / blendTimeTotal;
            anim.SetFloat("DormantBlend", blendValue);

            Invoke("blendDormantAnimation", Time.deltaTime);
        } else
        {
            blendTime = 0.0f;
            anim.SetFloat("DormantBlend", 1.0f);
        }
    }

    public void TurnToStone()
    {
        Destroy(eye_left);
        Destroy(eye_right);
        anim.enabled = false;
        agent.enabled = false;
        sense.enabled = false;
        enabled = false;
        GetComponent<AudioSource>().enabled = false;
    }

    void killPlayer()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().KillPlayer(transform);
    }
    
}
