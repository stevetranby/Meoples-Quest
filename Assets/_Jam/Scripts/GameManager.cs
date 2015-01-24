using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: make sure this can persist across level scenes?
public class GameManager : MonoBehaviour {

    private GameObject currentPlayer;
    private List<GameObject> players;

	// Use this for initialization
	void Start () {
        players = new List<GameObject>();
        for(int i = 0; i < 6; ++i)
        {
            var player = GameObject.Find("Player" + (i+1));
            if(player) {
                players.Add(player);
            }
        }
        setActivePlayer(1);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void setActivePlayer(int playerNum) 
    {
        var arrayIndex = playerNum - 1;
        var player = arrayIndex < players.Count ? players[arrayIndex] : null;

        Debug.Log("setting player " + arrayIndex + " to active!");

        if(player) {
            // remove controller from previous player
            if(currentPlayer) 
            {
                var control = currentPlayer.GetComponent<PlayerControl>();
                if(control) {  control.enabled = false; }
            }

            currentPlayer = player;

            {
                var control = currentPlayer.GetComponent<PlayerControl>();
                if(control) { control.enabled = true; }
                var follow = Camera.main.GetComponent<CameraFollow>();
                follow.playerTransform = currentPlayer.transform;
            }
        }
    }
}
