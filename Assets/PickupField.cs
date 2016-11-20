using UnityEngine;
using System.Collections;

public class PickupField : MonoBehaviour {

    public Transform[] pickupPositions;
    public Transform pickup;

	// Use this for initialization
	void Start () {

        // Choose position at random
        int randInt = Random.Range(0, pickupPositions.Length);
        Debug.Log(randInt);
        // Instantiate pickup at position
        GameObject.Instantiate(pickup, pickupPositions[randInt].transform.position, Quaternion.identity);

        // Destroy position objects
        foreach (Transform pu in pickupPositions)
        {
            Destroy(pu.gameObject);
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
