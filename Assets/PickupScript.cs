using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PickupScript : MonoBehaviour {
    
    public Light pointLight;
    public Transform[] meshes;
    public float maxDistance = 20.0f;
    public float rotationSpeed = 500.0f;
    public string pickupAudioName = "PickupAudio";

    AudioSource audioSource;

    Transform player;

    Color startDiffuse;
    Color startEmission;
    float startLightIntensity;
    float startHaloColor;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = player.FindChild(pickupAudioName).GetComponent<AudioSource>();
        
        startDiffuse = meshes[0].GetComponent<MeshRenderer>().material.GetColor("_Color");
        startEmission = meshes[0].GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        startLightIntensity = pointLight.intensity;
	}
	
	// Update is called once per frame
	void Update () {

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        float dist_t = 1 - Mathf.Clamp01(distanceToPlayer / maxDistance);

        foreach(Transform mesh in meshes)
        {
            mesh.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", InterpolateColor(Color.black, startEmission, dist_t));
            mesh.GetComponent<MeshRenderer>().material.SetColor("_Color", InterpolateColor(Color.black, startDiffuse, dist_t));
        }

        pointLight.intensity = Mathf.Lerp(0, startLightIntensity, dist_t);

        float speed = dist_t * rotationSpeed;
        transform.Rotate(0, speed * Time.deltaTime, 0);

        if(distanceToPlayer < 1.5f)
        {
            // Get pickup
            Debug.Log("Get pickup"); audioSource.Play();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GetPickup(gameObject);
            
        }

	}

    Color InterpolateColor(Color color1,Color color2, float t)
    {
        Color interpColor = new Color();
        interpColor.r = Mathf.Lerp(color1.r, color2.r, t);
        interpColor.g = Mathf.Lerp(color1.g, color2.g, t);
        interpColor.b = Mathf.Lerp(color1.b, color2.b, t);
        interpColor.a = Mathf.Lerp(color1.a, color2.a, t);
        return interpColor;
    }
}
