using UnityEngine;
using System.Collections;

public class AdaptiveDOF : MonoBehaviour {

    new Camera camera;
	// Use this for initialization
	void Start () {
        camera = transform.parent.GetComponentInChildren<Camera>();
	}

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
        }
    }
}
