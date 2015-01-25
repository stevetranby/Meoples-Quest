using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    public string ItemID;               // How much health the crate gives the player.
    public AudioClip collect;               // The sound of the crate being collected.

    private PlayerInventory inventory;  // Reference to the pickup spawner.
    private Animator anim;                  // Reference to the animator component.
    private bool landed;                    // Whether or not the crate has landed.

    public bool keepHolding;
    public bool usePhysicsWhenActivated;

    public Image itemHUD;
    public GameObject playerInTrigger;
    public float cantPickupTimer = 0;

    void Awake()
    {
        // Setting up the references.
        inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
        itemHUD = GameObject.Find("activeItemHUD").GetComponent<Image>();
//      anim = transform.root.GetComponent<Animator>();
    }

    public virtual bool UseAction(GameObject player, bool facingRight)
    {
        Debug.Log("enter UseAction");
        // implement in base
        keepHolding = true;
        return keepHolding;
    }

    void Update()
    {
        // check if action button
        if (Input.GetButtonUp("Fire1") && this.GetComponent<CircleCollider2D>().isTrigger)
        {
            Debug.Log("FIRE!" + Time.time + " [" + playerInTrigger + "]");

            // is item in hand?
            if (playerInTrigger)
            {
                Debug.Log("player exists!" + Time.time);
                var controller = playerInTrigger.GetComponent<PlayerControl>();
                if (controller.activeItem == null && cantPickupTimer <= 0)
                {
                    Debug.Log("FIRE: player inside item");
                    controller.activeItem = this;
                    controller.cantUseTimer = .1f;
                    //          playerInTrigger.activateItem(this.ItemID);
                    //          inventory.addItem(this);

                    var sr = this.GetComponent<SpriteRenderer>();
                    itemHUD.sprite = sr.sprite; //.GetComponent<SpriteRenderer>().sprite = sr.sprite;


                    // TODO: should probaby have separate "inventory" item with just image
                    // - and then a full physics object if necessary for a given action
                    // - ItemPickup should define whether it needs physics or not

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
                } else {
                    // put into player inventory instead of holding
                    Debug.Log("Adding trigger item " + controller.triggerItem + " into inventory");
                    bool success = controller.addToInventory(controller.triggerItem);
                    if(! success) { 
                        // TODO: alert player
                    }
                }
            }
        }

        if (cantPickupTimer > 0)
        {
            cantPickupTimer -= Time.deltaTime;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("player exiting item");            
            playerInTrigger = null;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {       
        // If the player enters the trigger zone...
        if (other.tag == "Player" && landed)
        {
            Debug.Log("player inside item: " + other.gameObject);
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
            //gameObject.AddComponent<Rigidbody2D>();
            landed = true;  
        }
    }
}
