using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeGenerator : MonoBehaviour {

    public Transform prefab;
    public Transform terrain;
    public int tree_num = 100;
    public float min_x = -10f;
    public float max_x = 10f;
    public float min_z = -10f;
    public float max_z = 10f;

    List<GameObject> trees = new List<GameObject>();

    // Use this for initialization
    void Start () {
	
        for(int i = 0; i < tree_num; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(min_x, max_x), 
                prefab.localScale.y / 2, 
                Random.Range(min_z, max_z)
                );
            Quaternion rotation = Quaternion.Euler(new Vector3(0, Random.Range(0.0f, 360.0f), 0));
            trees.Add( (GameObject)Instantiate(prefab.gameObject,position,rotation) );
            
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
