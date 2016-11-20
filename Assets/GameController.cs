using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class GameController : MonoBehaviour {

    public Canvas mainMenu;
    public Canvas pauseMenu;
    public Canvas winScreen;
    public Canvas loseScreen;
    public Canvas introScreen;
    public Canvas tutorialScreen;
    public int pickups_to_win = 4;


    Transform player;
    FirstPersonController playerController;
    List<NavMeshAgent> trollControllers = new List<NavMeshAgent>();
    WinScreenScript winScreenScript;

    Transform killerTroll;
    Vector3 startKillPosition, endKillPosition;

    public enum GameState
    {
        INTRO,
        MENU,
        PAUSED,
        RUNNING,
        RESTARTING,
        WINNING,
        LOSING
    }

    GameState state = GameState.INTRO;

    int pickup_count = 0;

	// Use this for initialization
	void Start () {

        Cursor.visible = false;

        introScreen.enabled = true;
        mainMenu.enabled = false;
        tutorialScreen.enabled = false;
        pauseMenu.enabled = false;
        loseScreen.enabled = false;
        winScreen.enabled = false;
        winScreenScript = winScreen.GetComponent<WinScreenScript>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<FirstPersonController>();

        resetPlayer();

        GameObject[] trolls = GameObject.FindGameObjectsWithTag("Troll");
        
        foreach(GameObject troll in trolls)
        {
            trollControllers.Add(troll.GetComponent<NavMeshAgent>());
        }

        disableTrolls();

        introScreen.GetComponent<FadeTextScript>().StartSequence();

    }
	
	// Update is called once per frame
	void Update () {
	
        switch(state)
        {
            case GameState.INTRO:
                if(introScreen.GetComponent<FadeTextScript>().SequenceComplete())
                {
                    state = GameState.MENU;
                    mainMenu.enabled = true;
                    introScreen.enabled = false;
                }
                break;
            case GameState.MENU:
                Cursor.visible = true;
                break;
            case GameState.PAUSED:
                if (Input.GetButtonDown("Cancel"))
                {
                    resumeGame();
                }
                break;
            case GameState.RESTARTING:

                break;
            case GameState.RUNNING:
                if(Input.GetButtonDown("Cancel"))
                {
                    pauseGame();
                }
                if(tutorialScreen.GetComponent<FadeTextScript>().SequenceComplete())
                {
                    tutorialScreen.enabled = false;
                }

                break;
            case GameState.WINNING:

                if (Input.GetButtonDown("Cancel"))
                {
                    pauseGame();
                }

                break;
            case GameState.LOSING:

                player.position = Vector3.Lerp(player.position, endKillPosition, 3.0f * Time.deltaTime);
                Transform playerChild = player.GetChild(0);
                Quaternion startRotation = playerChild.rotation;
                playerChild.LookAt(killerTroll.GetComponent<TrollAI>().headBone.position);
                Quaternion endRotation = playerChild.rotation;
                playerChild.rotation = Quaternion.Lerp(startRotation, endRotation, 3.0f * Time.deltaTime);

                if(Input.GetButtonDown("Cancel") || Input.GetButtonDown("Submit"))
                {
                    returnToMainMenu();
                }

                break;
        }

	}

    public void startGame()
    {
        state = GameState.RUNNING;
        mainMenu.enabled = false;
        tutorialScreen.enabled = true;
        enablePlayer();
        enableTrolls();
        Cursor.visible = false;
        if (tutorialScreen.enabled)
        {
            tutorialScreen.GetComponent<FadeTextScript>().StartSequence();
        }
    }

    public void pauseGame()
    {
        state = GameState.PAUSED;
        pauseMenu.enabled = true;
        disablePlayer();
        disableTrolls();
        Cursor.visible = true;

    }

    public void resumeGame()
    {
        state = GameState.RUNNING;
        pauseMenu.enabled = false;
        enablePlayer();
        enableTrolls();
        Cursor.visible = false;
    }

    public void returnToMainMenu()
    {
        /*
        state = GameState.MENU;
        mainMenu.enabled = true;
        pauseMenu.enabled = false;
        resetPlayer();
        resetTrolls();
        */
        Application.LoadLevel("TrollIsland");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void GetPickup(GameObject pickup)
    {
        Destroy(pickup);
        pickup_count++;
        if(pickup_count == pickups_to_win)
        {
            winScreen.enabled = true;
            winScreenScript.StartSequence();
            state = GameState.WINNING;
        }
    }

    public void KillPlayer(Transform troll)
    {
        state = GameState.LOSING;
        endKillPosition = troll.position + troll.forward * 5 + Vector3.up * 3.5f;
        killerTroll = troll;
        troll.GetComponent<CapsuleCollider>().enabled = false;
        
        disablePlayer();
        disableTrolls();

        loseScreen.enabled = true;
    }

    void enableTrolls()
    {
        Debug.Log("Controllers: " + trollControllers.Count);
        foreach (NavMeshAgent controller in trollControllers)
        {
            controller.enabled = true;
            controller.transform.GetComponentInChildren<Animator>().enabled = true;
        }
    }

    void disableTrolls()
    {
        foreach(NavMeshAgent controller in trollControllers)
        {
            controller.enabled = false;
            controller.transform.GetComponentInChildren<Animator>().enabled = false;
        }
    }

    void resetTrolls()
    {

    }

    void enablePlayer()
    {
        playerController.enabled = true;
    }

    void disablePlayer()
    {
        playerController.enabled = false;
    }

    void resetPlayer()
    {
        //player.position = playerStart.position;
        disablePlayer();
    }

    public GameState getState()
    {
        return state;
    }
}
