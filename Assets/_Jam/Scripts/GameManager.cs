using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// TODO: make sure this can persist across level scenes?
public class GameManager : MonoBehaviour
{
    public GameObject ropePostPrefab;
    public GameObject flamesPrefab;
	public GameObject rocketExplosionPrefab;

    public GameObject currentPlayer;
    public List<GameObject> players;
    public bool inCutscene = false;
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
//        if (this.inCutscene)
//        {
//            //Debug.Log("cutscene time left: " + cutSceneFailsafeTimer);
//            // continue cutscene
//            //TODO: StartCoroutine();
//            if (cutSceneFailsafeTimer <= 0)
//            {
//                Debug.Log("cutscene time OVER!");
//                cutSceneFailsafeTimer = 0f;
//                
//            }
//            cutSceneFailsafeTimer -= Time.deltaTime;
//        } 
    }

    public void enterCutscene(StoryTrigger story)
    {
		/*
		 * storyIntro
		 * storyNeedLever
		 * storyDropRope
		 * storyBalloonTakeOff
		 * storyAirplaneTakeOff
		 * storyRocketTakeOff
		 */
        
		currentStory = story;

		Debug.Log("enterting cutscene");
		inCutscene = true;
		removeCurrentPlayerControl();
		cutSceneFailsafeTimer = currentStory.storyDuration;		
		cutSceneMessageHUD.text = currentStory.Message;

        StartCoroutine(currentStory.StoryName);
    }

	public void AttachPlayersToCutscene(Transform transform, int num, bool horizontal) 
	{
		for(int i=0; i<num; i++) {
			var player = players[i];

			players[i].GetComponent<FollowTarget>().target = transform;

			if(horizontal) {
			players[i].GetComponent<FollowTarget>().offset = new Vector3(-.4f + .5f * i, 0f, 0f);
			} else {
				players[i].GetComponent<FollowTarget>().offset = new Vector3(0, -.4f + .5f * i, 0f);
			}

			// disable physics
			player.rigidbody2D.isKinematic = true;
			players[i].GetComponent<BoxCollider2D>().enabled = false;
			players[i].GetComponent<CircleCollider2D>().enabled = false;
			players[i].GetComponent<PlayerControl>().enabled = false;
		}
	}

	public void ResetPlayerControl(int num)
	{
		Debug.Log("resetting Players");
		// Reset Players
		for(int i=0; i<num; i++) {
			var player = players[i];

			// fix position
//			var pos = player.transform.position;
//			player.transform.parent = null;
//			player.transform.localPosition = pos;
			players[i].GetComponent<FollowTarget>().target = null;


			player.rigidbody2D.isKinematic = false; // false - use physics			
			players[i].GetComponent<BoxCollider2D>().enabled = true;
			players[i].GetComponent<CircleCollider2D>().enabled = true;
		}
	}

	public IEnumerator storyIntro()
	{
		// TODO: dialog	
		float duration = currentStory.storyDuration;
		yield return new WaitForSeconds(duration);

		this.exitCutscene();
	}


	public IEnumerator storyNeedLever()
	{
		// follow camera instead
		var follow = Camera.main.GetComponent<FollowTarget>();
		follow.target = currentPlayer.transform;
		var player = players[1];
		if(player) {
			follow.target = player.transform;
		}

		float duration = currentStory.storyDuration;
		yield return new WaitForSeconds(duration);  

		this.exitCutscene();
	}

	public IEnumerator storyDropRope()
	{
		var ropeItem = GameObject.Find("ropeItem");
		Destroy(ropeItem);
		yield return new WaitForSeconds(0.2f);
		ropePostPrefab.SetActive(true);

		this.exitCutscene();
	}


	public IEnumerator storyBalloonTakeOff()
	{
		var balloon = GameObject.Find("balloon").transform;

		// follow balloon instead
		var follow = Camera.main.GetComponent<FollowTarget>();	
		follow.target = balloon.transform;
		
		// force players into balloon basket
		var basketTransform = GameObject.Find("basket").transform;
		AttachPlayersToCutscene(basketTransform, 2, true);

		yield return new WaitForSeconds(0.5f);
		
		// TODO: create a script that updates using Lerp or other tween
		// dialog and move balloon
		//            while( not at destination or next scene cut ) {
		//                  do lerp toward destination
		//            }
		var destination = GameObject.Find("basket_destination").transform.localPosition;
		var basketOrig = basketTransform.localPosition;
		var basketDest = destination + new Vector3(0,.5f,0);
		var balloonOrig = balloon.transform.localPosition;
		var balloonDest = basketDest + (balloonOrig - basketOrig);
		for(int i=0; i<400; i++) {
			balloon.transform.localPosition = Vector3.Lerp(balloonOrig, balloonDest, (float)i/400f);
			basketTransform.localPosition = Vector3.Lerp(basketOrig, basketDest, (float)i/400f);
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + .1f, 5f, 30f);
			yield return new WaitForEndOfFrame();// new WaitForSeconds(0.1f);
		}
		
		{
			// spawn flames
			GameObject flames = (GameObject)Instantiate(flamesPrefab, balloon.transform.localPosition , balloon.transform.localRotation);
			flames.transform.parent = balloon.transform;
			flames.transform.Translate(Vector3.left * 4f);
			
			// fade out balloon
			var color = balloon.GetComponent<SpriteRenderer>().material.color; 
			for(int i=0; i<200; i++) {
				float alpha = (float)i / 200f;
				color.a = (1f - alpha);
				balloon.GetComponent<SpriteRenderer>().material.color = color;
				yield return new WaitForEndOfFrame();
			}

			//yield return new WaitForSeconds(1f);               
		}
		
//		// drop basket
//		// follow balloon instead
//		Camera.main.GetComponent<FollowTarget>().target = basketTransform;
//		for(int i=0; i<150; i++) {
//
//			basketTransform.Translate(new Vector3(0,-.01f,0));
//			yield return new WaitForEndOfFrame();// new WaitForSeconds(0.1f);
//		}           
//		

		// target basket
		Camera.main.GetComponent<FollowTarget>().target = basketTransform;

		// TOOD: get from gameobject placeholder
		for(int i=0; i<200; i++) {
			balloon.transform.Translate(new Vector3(0,.1f,0));
			basketTransform.localPosition = Vector3.Lerp(basketTransform.localPosition, destination, (float)i/200f);
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - .1f, 5f, 30f);
			yield return new WaitForEndOfFrame();
		}
		// destroy balloon
		Destroy(balloon.gameObject);
		yield return new WaitForEndOfFrame();
		
		ResetPlayerControl(2);

		basketTransform.RotateAround(new Vector3(0,0,0), Vector3.forward, 90);
		//Destroy(basketTransform.gameObject);

		this.exitCutscene();
	}


	public IEnumerator storyPlaneTakeOff() 
	{
		var airplane = GameObject.Find("Airplane").transform;
		
		// follow balloon instead
		var follow = Camera.main.GetComponent<FollowTarget>();
		follow.target = airplane.transform;
		
		// force players into balloon basket
		AttachPlayersToCutscene(airplane.transform, 4, true);
		
		yield return new WaitForSeconds(0.5f);

		for(int i=0; i<100; i++) {
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + .2f, 5f, 100f);
			yield return new WaitForEndOfFrame();
		}

		var airplaneOrig = airplane.localPosition;
		var destination = GameObject.Find("airplane_destination").transform.localPosition;
		for(int i=0; i<400; i++) {
			airplane.localPosition = Vector3.Lerp(airplaneOrig, destination, (float)i/400f);
			yield return new WaitForEndOfFrame();
		}

		for(int i=0; i<100; i++) {
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - .2f, 5f, 100f);
			yield return new WaitForEndOfFrame();
		}

		airplane.gameObject.rigidbody2D.isKinematic = false;
		airplane.gameObject.GetComponent<BoxCollider2D>().enabled = true;

		ResetPlayerControl(4);

		this.exitCutscene();
	}


	public IEnumerator storyRocketTakeOff() 
	{
		var rocket = GameObject.Find("rocket").transform;

		// follow balloon instead
		var follow = Camera.main.GetComponent<FollowTarget>();
		follow.target = rocket.transform;
		
		AttachPlayersToCutscene(rocket.transform, 6, false);
		
		yield return new WaitForSeconds(0.5f);
		
		// TOOD: get from gameobject placeholder
		for(int i=0; i<200; i++) {
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + .1f, 5f, 100f);
			yield return new WaitForEndOfFrame();
		}
		
		// TOOD: get from gameobject placeholder
		var rocketOrig = rocket.localPosition;
		var destination = GameObject.Find("rocket_destination").transform.localPosition;
		for(int i=0; i<100; i++) {
			rocket.localPosition = Vector3.Lerp(rocketOrig, destination, (float)i/100f);
			yield return new WaitForEndOfFrame();
		}
		
		Debug.Log("end game!");
		
		yield return new WaitForSeconds(1.5f);
		
		var explode = (GameObject)Instantiate(rocketExplosionPrefab, rocket.localPosition, rocket.localRotation);
		explode.GetComponent<Animator>().SetTrigger("RocketExplode");

		Destroy(rocket.gameObject);
		
		yield return new WaitForSeconds(1f);
		
		Application.LoadLevel ("EndGame");

		this.exitCutscene();
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
        var player = (arrayIndex < players.Count) ? players [arrayIndex] : null;

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
			var follow = Camera.main.GetComponent<FollowTarget>();
			follow.target = currentPlayer.transform;
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
