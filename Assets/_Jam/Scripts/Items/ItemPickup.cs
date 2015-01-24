using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    public string ItemID;               // How much health the crate gives the player.
    public AudioClip collect;               // The sound of the crate being collected.

    private PlayerInventory inventory;  // Reference to the pickup spawner.
    private Animator anim;                  // Reference to the animator component.
    private bool landed;                    // Whether or not the crate has landed.
    
    private GameObject playerInTrigger;
    public float cantPickupTimer;

    void Awake()
    {
        // Setting up the references.
        inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
//      anim = transform.root.GetComponent<Animator>();
    }

    public virtual void UseAction(GameObject player, bool facingRight)
    {
        // implement in base
    }

    void Update()
    {
        // check if action button
        if (Input.GetButtonUp("Fire1") && this.GetComponent<CircleCollider2D>().isTrigger)
        {
            Debug.Log("FIRE!" + Time.time);

            // is item in hand?
            if (playerInTrigger)
            {
                var controller = playerInTrigger.GetComponent<PlayerControl>();
                if (cantPickupTimer <= 0 && controller.item1 == null)
                {
                    Debug.Log("FIRE: player inside item");
                    controller.item1 = this;
                    controller.cantUseTimer = .1f;
                    //          playerInTrigger.activateItem(this.ItemID);
                    //          inventory.addItem(this);

                    // Attach to player
                    transform.root.gameObject.transform.parent = playerInTrigger.transform;
                    transform.localPosition = Vector3.up * 1;


                    // Disable The Item's Physics
                    this.GetComponent<Rigidbody2D>().isKinematic = true;
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    this.GetComponent<CircleCollider2D>().isTrigger = false;
                    this.GetComponent<CircleCollider2D>().enabled = false;

                    // Destroy the crate.
                    //Destroy(transform.root.gameObject);

                    playerInTrigger = null;
                }
            }
        }

        if (cantPickupTimer > 0)
        {
            cantPickupTimer -= Time.deltaTime;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("player inside item");            
            playerInTrigger = null;//other.gameObject;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {       
        // If the player enters the trigger zone...
        if (other.tag == "Player" && landed)
        {
            Debug.Log("player inside item");
            playerInTrigger = other.gameObject;
                  
            if (collect)
            {
                AudioSource.PlayClipAtPoint(collect, transform.position);
            }
        }
        // Otherwise if the crate hits the ground...
        else if ((other.tag == "ground") && !landed)
        {
            // ... set the Land animator trigger parameter.
            //anim.SetTrigger("Land");
            
            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            landed = true;  
        }
    }
}
