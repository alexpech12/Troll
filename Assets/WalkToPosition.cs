using UnityEngine;
using System.Collections;

public class WalkToPosition : MonoBehaviour {

    Transform target;

    NavMeshAgent agent;
    TrollState state;
    TrollAI trollAI;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        state = GetComponent<TrollState>();
        trollAI = GetComponent<TrollAI>();
        //target = trollAI.playerGhost;
	}

    // Update is called once per frame
	void Update () {

        if (agent.enabled)
        {
            agent.destination = target.position;
        }
        if (state.Changed())
        {
            //agent.speed = state.GetStateComponent().movementSpeed;
        }
        

    }
}
