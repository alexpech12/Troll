using UnityEngine;
using System.Collections;

public class TrollAudio : MonoBehaviour {

    public AudioClip[] randomClips;
    public float clipSpacing = 15.0f;
    public float clipSpacingSpread = 10.0f;
    public AudioClip roarClip;

    AudioSource audioSource;
    GameController gameController;

	// Use this for initialization
	void Start () {
        audioSource = transform.GetComponent<AudioSource>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    }

    // Update is called once per frame
    float t = 0;
    float timeToNextClip = 0;
	void Update () {

        if(gameController.getState() == GameController.GameState.RUNNING ||
            gameController.getState() == GameController.GameState.WINNING ||
            gameController.getState() == GameController.GameState.LOSING
            )
        {
            audioSource.mute = false;
        } else
        {
            audioSource.mute = true;
        }

        t += Time.deltaTime;

        if(t > timeToNextClip)
        {
            timeToNextClip = Random.Range(clipSpacing - clipSpacingSpread, clipSpacing + clipSpacingSpread) + audioSource.clip.length;
            t = 0;
            if (audioSource.enabled)
            {
                audioSource.clip = getRandomClip();
                audioSource.Play();
            }
        }

	}

    AudioClip getRandomClip()
    {
        return randomClips[Random.Range(0, randomClips.Length)];
    }

    public void Roar()
    {
        audioSource.clip = roarClip;
        audioSource.Play();
        t = 0;
    }
}
