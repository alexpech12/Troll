using UnityEngine;
using System.Collections;

public class PlayerGhostScript : MonoBehaviour {

    public enum BehaviourType
    {
        STATIONARY,
        FOLLOW_PLAYER,
        FOLLOW_VELOCITY
    }

    public Transform troll;
    Transform player;

    public float velocityFalloff = 0.5f;
    public float captureDistance = 2f;

    BehaviourType behaviour = BehaviourType.STATIONARY;

    TrollSense trollSense;
    TrollState trollState;

    Vector3 velocity;

    Vector3 returnPosition;
    bool returning = false;

    GameObject[] trollDestinations;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        trollDestinations = GameObject.FindGameObjectsWithTag("TrollDestination");
    }

	// Use this for initialization
	void Start () {

        trollSense = troll.GetComponent<TrollSense>();
        trollState = troll.GetComponent<TrollState>();
        
	}

    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().getState() == GameController.GameState.RUNNING)
        {
            if (behaviour != BehaviourType.FOLLOW_VELOCITY)
            {
                UpdatePlayerVelocity();
            }
        }
    }

    public void UpdatePosition()
    {
        // Go to player position by default
        UpdatePosition(player.position);
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        behaviour = BehaviourType.FOLLOW_PLAYER;
        transform.position = newPosition;
    }

    public void UpdateRandomPosition()
    {
        behaviour = BehaviourType.STATIONARY;
        transform.position = GetRandomPosition();
    }

    public void UpdateRandomPosition(Vector3 centre, float radius)
    {
        Vector3 randPos = centre + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
        randPos.y = getTerrainHeight(randPos);
        transform.position = randPos;
    }

    public void UpdateVelocityPosition()
    {
        behaviour = BehaviourType.FOLLOW_VELOCITY;
        transform.Translate(velocity);
        velocity = velocity * (1 - velocityFalloff * Time.deltaTime);
    }

    Vector3 GetRandomPosition()
    {
        //Vector3 randPos = new Vector3(Random.Range(-250, 250), 0, Random.Range(-250, 250));
        Vector3 randPos = trollDestinations[Random.Range(0, trollDestinations.Length)].transform.position;
        randPos.y = getTerrainHeight(randPos);
        return randPos;
    }

    Vector3[] positionList = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero };
    
    void UpdatePlayerVelocity()
    {

        positionList[2] = positionList[1];
        positionList[1] = positionList[0];
        positionList[0] = player.position;
        velocity = (0.67f * (positionList[0] - positionList[1]) + 0.33f * (positionList[1] - positionList[2])) / 2f;
        

    }

    float getTerrainHeight(Vector3 position)
    {
        return Terrain.activeTerrain.SampleHeight(position) + player.GetComponent<CharacterController>().height/2;
    }
}
