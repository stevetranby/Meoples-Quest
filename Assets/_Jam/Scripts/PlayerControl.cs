using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    [HideInInspector]
    public bool
        facingRight = true;         // For determining which way the player is currently facing.
    [HideInInspector]
    public bool
        jump = false;               // Condition for whether the player should jump.

    public float moveForce = 365f;          // Amount of force added to move the player left and right.
    public float maxSpeed = 5f;             // The fastest the player can travel in the x axis.
    public AudioClip[] jumpClips;           // Array of clips for when the player jumps.
    public float jumpForce = 1000f;         // Amount of force added when the player jumps.
    public AudioClip[] taunts;              // Array of clips for when the player taunts.
    public float tauntProbability = 50f;    // Chance of a taunt happening.
    public float tauntDelay = 1f;           // Delay for when the taunt should happen.

    private int tauntIndex;                 // The index of the taunts array indicating the most recent taunt.
    private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private bool grounded = false;          // Whether or not the player is grounded.
    private Animator anim;                  // Reference to the player's animator component.



    // Ladders
    public bool insideLadderTrigger = false;
    public bool isClimbing = false;
    public bool climbingDirectionUp = true;
    public bool activateItem = false;
    public float launchSpeed = 10f;
    public float cantUseTimer = 0;
    public ItemPickup activeItem = null;
    public ItemPickup triggerItem = null;

    public enum SpecialTrait
    {
        Strong,
        Jumper,
        Floater
    }
    ;

    void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("groundCheck");
        anim = GetComponent<Animator>();
    }

	// Check for input here, handle elsewhere
    void Update()
    {
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

        // If the jump button is pressed and the player is grounded then the player should jump.
        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;

        if (Input.GetButtonUp("Fire1"))
        {
            Debug.Log("Player Control Trying to Launch! [" + activeItem + ", " + cantUseTimer + ", " + Time.time);
            if (activeItem && cantUseTimer <= 0)
            {
                Debug.Log("Launch!");
                activateItem = true;
            }
        }

        // If trying to move vertically

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0 && insideLadderTrigger)
        {
            isClimbing = true;
            rigidbody2D.isKinematic = true;
            climbingDirectionUp = Input.GetAxis("Vertical") > 0;
        } else
        {
            isClimbing = false;
            rigidbody2D.isKinematic = false;
        }

        if (cantUseTimer > 0)
        {
            cantUseTimer -= Time.deltaTime;
        }
    }

	// handle physics actions
    void FixedUpdate()
    {
        // Cache the horizontal input.
        float h = Input.GetAxis("Horizontal");

        // The Speed animator parameter is set to the absolute value of the horizontal input.
        anim.SetFloat("Speed", Mathf.Abs(h));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * rigidbody2D.velocity.x < maxSpeed)
        {
            // ... add a force to the player.
            rigidbody2D.AddForce(Vector2.right * h * moveForce);
        }

        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
        {
            // ... set the player's velocity to the maxSpeed in the x axis.
            rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
        }

        // If the input is moving the player right and the player is facing left...
        if (h > 0 && !facingRight)
        {   // ... flip the player.
            Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
        } else if (h < 0 && facingRight)
        {
            // ... flip the player.
            Flip();
        }

        // If the player should jump...
        if (isClimbing)
        {
            // TODO: check this - rigidbody.isKinematic = false;           
            var p = gameObject.transform.localPosition;
            var ladderSpeed = 4f;
            gameObject.transform.localPosition = new Vector3(p.x, p.y + ladderSpeed * Time.deltaTime, 0);
            //rigidbody2D.AddForce(new Vector2(0f, 10f));
        } else if (jump)
        {
            Debug.Log("trying to jump");
            // Set the Jump animator trigger parameter.
            anim.SetTrigger("Jump");

            // Play a random jump audio clip.
            int i = Random.Range(0, jumpClips.Length);
            AudioSource.PlayClipAtPoint(jumpClips [i], transform.position);

            // Add a vertical force to the player.
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));

            // Make sure the player can't jump again until the jump conditions from Update are satisfied.
            jump = false;
        }

        if (activateItem)
        {
            Debug.Log("Launching!");
            UseActionItem();            

            activeItem = null;
            triggerItem = null;
            activateItem = false;
        }
    }
    
    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// Adds to inventory.
    /// </summary>
    /// <returns><c>true</c>, if item was added, <c>false</c> otherwise.</returns>
    /// <param name="item">Item.</param>
    public bool addToInventory(ItemPickup item)
    {
        return true;
    }

    public void UseActionItem()
    {
        Debug.Log("enter UseActionItem");

        // get item function script       
        if (activeItem)
        {
            bool keepHolding = activeItem.UseAction(this.gameObject, facingRight);
            if (! keepHolding)
            {
                activeItem.itemHUD.sprite = null;
                activeItem = null;
            }
        }
    }     
}
