using UnityEngine;
using System.Collections;

public class TrollSenseDebug : MonoBehaviour {

    TrollState state;
    Transform head;
    float viewDistance, viewAngle, hearingDistance, smellDistance;

    public bool vision, hearing, smell, forward;

	// Use this for initialization
	void Start () {

        state = transform.GetComponent<TrollState>();
        head = GetComponent<TrollSense>().head;

	}
	
	// Update is called once per frame
	void Update () {

        viewDistance = state.GetStateComponent().viewDistance;
        viewAngle = state.GetStateComponent().viewAngle;
        hearingDistance = state.GetStateComponent().hearingDistance;
        smellDistance = state.GetStateComponent().smellDistance;

        DebugDrawing();
	}

    void DebugDrawing()
    {
        if (forward)
        {
            Debug.DrawRay(transform.position, transform.forward * 5, Color.magenta);
        }

        if (vision)
        {
            Debug.DrawRay(head.position, viewDistance * head.forward, Color.green);
            Debug.DrawRay(head.position, viewDistance * (Quaternion.Euler(new Vector3(0, viewAngle, 0)) * head.forward), Color.green);
            Debug.DrawRay(head.position, viewDistance * (Quaternion.Euler(new Vector3(0, -viewAngle, 0)) * head.forward), Color.green);
            Debug.DrawRay(head.position, viewDistance * (Quaternion.Euler(new Vector3(0, viewAngle / 2, 0)) * head.forward), Color.green);
            Debug.DrawRay(head.position, viewDistance * (Quaternion.Euler(new Vector3(0, -viewAngle / 2, 0)) * head.forward), Color.green);
        }
        if (hearing)
        {
            DebugDrawCircle(transform.position, hearingDistance, Color.blue);
        }
        if (smell)
        {
            DebugDrawCircle(transform.position, smellDistance, Color.red);
        }
    }

    void DebugDrawCircle(Vector3 centre, float radius, Color color)
    {
        for (int i = 0; i < 30; i++)
        {
            float a = i * ((2 * Mathf.PI) / 30.0f);
            float a2 = (i + 1) * ((2 * Mathf.PI) / 30.0f);
            Vector3 startPoint = centre + new Vector3(radius * Mathf.Cos(a), 0, radius * Mathf.Sin(a));
            Vector3 endPoint = centre + new Vector3(radius * Mathf.Cos(a2), 0, radius * Mathf.Sin(a2));
            Debug.DrawLine(startPoint, endPoint, color);
        }
    }
}
