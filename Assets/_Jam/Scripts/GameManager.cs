using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// TODO: make sure this can persist across level scenes?
public class GameManager : MonoBehaviour
{
    public GameObject ropePostPrefab;
    public GameObject flamesPrefab;

    public GameObject currentPlayer;
    public List<GameObject> players;
    public bool inCutscene;
    private float cutSceneFailsafeTimer = 1f;
    private Text cutSceneMessageHUD;
    private StoryTrigger currentStory;

    // Use this for initialization
    void Start()
    {        
        ropePostPrefab.SetActive(false);

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
            //Debug.Log("cutscene time left: " + cutSceneFailsafeTimer);
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

        removeCurrentPlayerControl();

        cutSceneFailsafeTimer = currentStory.storyDuration;

        // story props
        cutSceneMessageHUD.text = currentStory.Message;



        // lever cutscene
        if(currentStory.StoryName == "intro") 
        {
            // dialog
            
            yield return new WaitForSeconds(2.2f);
        }

        // lever cutscene
        if(currentStory.StoryName == "need_lever") 
        {
            //setActivePlayer(1);
            // follow camera instead
            var follow = Camera.main.GetComponent<CameraFollow>();
            follow.playerTransform = currentPlayer.transform;
            var player = players[1];
            if(player) {
                follow.playerTransform = player.transform;
            }

            yield return new WaitForSeconds(0.2f);                      
        }

        // lever cutscene
        if(currentStory.StoryName == "drop_rope") 
        {
            var ropeItem = GameObject.Find("ropeItem");
            Destroy(ropeItem);
            yield return new WaitForSeconds(0.2f);
            ropePostPrefab.SetActive(true);
        }

        // balloon scene
        if(currentStory.StoryName == "balloon_takeoff") 
        {   
            var balloon = GameObject.Find("balloon").transform;

            // follow balloon instead
            var follow = Camera.main.GetComponent<CameraFollow>();
            follow.playerTransform = currentPlayer.transform;
            var player1 = players[0];
            if(player1) {
                follow.playerTransform = balloon.transform;
            }

            // force players into balloon basket
            var basketTransform = GameObject.Find("basket").transform;
            for(int i=0; i<2; i++) {
                var player = players[i];
                player.transform.parent = basketTransform;
                player.transform.localPosition = new Vector3(-.4f + .5f * i, 0f, 0f);
                player.rigidbody2D.isKinematic = true;

                var comp = players[i].GetComponent<PlayerControl>();
//                comp.enabled = false;
                Destroy(comp);
            }
            
            yield return new WaitForSeconds(0.5f);

            // TODO: create a script that updates using Lerp or other tween
            // dialog and move balloon
//            while( not at destination or next scene cut ) {
//                  do lerp toward destination
//            }
            for(int i=0; i<200; i++) {
                balloon.transform.Translate(new Vector3(-.1f,.1f,0));
                basketTransform.Translate(new Vector3(-.1f,.1f,0));
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + .3f, 1f, 10f);
                yield return new WaitForEndOfFrame();// new WaitForSeconds(0.1f);
            }

            {
                // spawn flames
                GameObject flames = (GameObject)Instantiate(flamesPrefab, balloon.transform.localPosition , balloon.transform.localRotation);
                flames.transform.parent = balloon.transform;

                // TODO:  fade out balloon

                yield return new WaitForSeconds(1f);               
            }

            // drop basket
            for(int i=0; i<150; i++) {
                balloon.transform.Translate(new Vector3(0,.1f,0));
                basketTransform.Translate(new Vector3(0,-.01f,0));
                yield return new WaitForEndOfFrame();// new WaitForSeconds(0.1f);
            }           

            // destroy balloon
            Destroy(balloon.gameObject);
            yield return new WaitForEndOfFrame();

            // TOOD: get from gameobject placeholder
            var destination = GameObject.Find("basket_destination").transform.localPosition;
            for(int i=0; i<200; i++) {
                basketTransform.localPosition = Vector3.Lerp(basketTransform.localPosition, destination, i/20f);
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - .1f, 3f, 10f);
                yield return new WaitForEndOfFrame();
            }

            Destroy(basketTransform.gameObject);

            Debug.Log("resetting Players");
            // Reset Players
            for(int i=0; i<2; i++) {
                var player = players[i];
                var pos = player.transform.position;
                player.transform.parent = null;
                player.transform.localPosition = pos;
                player.rigidbody2D.isKinematic = false; // use physics

                // TODO: should disable/enable
//                var comp = players[i].GetComponent<PlayerControl>();
//                comp.enabled = true;

                players[i].AddComponent<PlayerControl>();

                setActivePlayer(1);
            }
        }
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
