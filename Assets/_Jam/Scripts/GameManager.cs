using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// TODO: make sure this can persist across level scenes?
public class GameManager : MonoBehaviour
{

    public GameObject currentPlayer;
    public List<GameObject> players;
    public bool inCutscene;
    private float cutSceneFailsafeTimer = 1f;
    private Text cutSceneMessageHUD;
    private StoryTrigger currentStory;

    // Use this for initialization
    void Start()
    {        
        cutSceneMessageHUD = GameObject.Find("cutSceneMessageHUD").GetComponent<Text>();

        players = new List<GameObject>();
        for (int i = 0; i < 6; ++i)
        {
            var player = GameObject.Find("Player" + (i + 1));
            if (player)
            {
                players.Add(player);
            }
        }
        setActivePlayer(1);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (this.inCutscene)
        {
            Debug.Log("cutscene time left: " + cutSceneFailsafeTimer);
            // continue cutscene
            //TODO: StartCoroutine();
            if (cutSceneFailsafeTimer <= 0)
            {
                Debug.Log("cutscene time OVER!");
                cutSceneFailsafeTimer = 0f;
                this.exitCutscene();
            }
            cutSceneFailsafeTimer -= Time.deltaTime;
        } 
    }

    public void enterCutscene(StoryTrigger story)
    {
        currentStory = story;
        StartCoroutine("enterCutsceneCoroutine");
    }

    public IEnumerator enterCutsceneCoroutine()
    {
        Debug.Log("enterting cutscene");
        inCutscene = true;

        cutSceneFailsafeTimer = currentStory.storyDuration;

        // story props
        cutSceneMessageHUD.text = currentStory.Message;

        if(currentStory.StoryID == 2) {
            // lever cutscene
            //setActivePlayer(1);
            // follow camera instead
            var follow = Camera.main.GetComponent<CameraFollow>();
            follow.playerTransform = currentPlayer.transform;
            var player2 = players[1];
            if(player2) {
                follow.playerTransform = player2.transform;
            }

            yield return new WaitForSeconds(0.2f);
        }

        removeCurrentPlayerControl();
    }

    public void exitCutscene()
    {
        if(! inCutscene) { return; }
        Debug.Log("exiting cutscene");
        inCutscene = false;
        setActivePlayer(1);
    }

    public void setActivePlayer(int playerNum)
    {
        if (inCutscene)
        {
            return;
        } 

        var arrayIndex = playerNum - 1;
        var player = arrayIndex < players.Count ? players [arrayIndex] : null;

        Debug.Log("setting player " + arrayIndex + " to active!");

        if (player)
        {
            // remove controller from previous player
            removeCurrentPlayerControl();

            currentPlayer = player;

            var control = currentPlayer.GetComponent<PlayerControl>();
            if (control)
            {
                control.enabled = true;
            }
            var follow = Camera.main.GetComponent<CameraFollow>();
            follow.playerTransform = currentPlayer.transform;
        }
    }

    public void removeCurrentPlayerControl()
    {
        if (currentPlayer)
        {
            var control = currentPlayer.GetComponent<PlayerControl>();
            if (control)
            {
                Debug.Log("disabling player control");
                control.enabled = false;
            }
        }
    }
}
