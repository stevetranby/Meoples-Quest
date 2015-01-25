using UnityEngine;
using System.Collections;

public class LadderTrigger : MonoBehaviour {

    GameObject playerInTrigger;
    bool climbActionButtonPressed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("player exiting LADDER");            
            // change state back 
            var playerInTrigger = other.gameObject;
            playerInTrigger.GetComponent<PlayerControl>().insideLadderTrigger = false;
//            var rigidBody = playerInTrigger.GetComponent<Rigidbody2D>();
//            if(rigidbody != null) {
//                rigidbody.isKinematic = false;
//            }
        }
    }
    
    public virtual void OnTriggerEnter2D(Collider2D other)
    {       
        // If the player enters the trigger zone...
        if (other.tag == "Player")
        {
            Debug.Log("player inside LADDER");
            var playerInTrigger = other.gameObject;
            playerInTrigger.GetComponent<PlayerControl>().insideLadderTrigger = true;
            var rigidBody = playerInTrigger.GetComponent<Rigidbody2D>();
            if(rigidbody != null) {
                rigidbody.isKinematic = true;
            }
        }
    }
}
