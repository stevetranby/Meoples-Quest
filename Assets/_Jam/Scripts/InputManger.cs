using UnityEngine;
using System.Collections;

public class InputManger : MonoBehaviour
{
    private GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = this.GetComponent<GameManager>();
        gameManager.setActivePlayer(1);
    }
    
    // Update is called once per frame
    void Update()
    {
        // GameObjects handle action buttons themself for now
//        if (Input.GetButtonUp("Fire1"))
//        {
//            var player = currentPlayer.GetComponent<PlayerControl>();
//            if (player.item1)
//            {
//                // Use Action
//                player.UseActionItem();          
//            } else {
//                // pickup item
//
//            }
//        } else if (Input.GetButtonUp("Fire2"))
//        {
//            // Use Action
//        } else if (Input.GetButtonUp("Jump"))
//        {
//            // Use Action
//        }

        int playerIndex = 0;
        // TODO: refactor into dictionary map
        // check keyboard input
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            playerIndex = 1;
        } else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            playerIndex = 2;
        } else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            playerIndex = 3;
        } else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            playerIndex = 4;
        } else if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            playerIndex = 5;
        } else if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            playerIndex = 6;
        }

        if (playerIndex > 0)
        {
            Debug.Log("KEY UP: playerIndex = " + playerIndex);
            // switch player controlled
            gameManager.setActivePlayer(playerIndex);
        }
    }
}
