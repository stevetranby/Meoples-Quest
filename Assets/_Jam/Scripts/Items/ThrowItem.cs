using UnityEngine;
using System.Collections;

public class ThrowItem : ItemPickup
{
    public override bool UseAction(GameObject player, bool facingRight)
    {
        Debug.Log("[throwitem] using actions");
        // throw
        // is item in hand?
        this.gameObject.transform.parent = null;
        this.gameObject.transform.localPosition = this.gameObject.transform.localPosition + Vector3.up * 1;
        
        // Disable The Item's Physics
        this.GetComponent<Rigidbody2D>().isKinematic = false;
        this.GetComponent<BoxCollider2D>().enabled = true;
        this.GetComponent<CircleCollider2D>().isTrigger = true;
        this.GetComponent<CircleCollider2D>().enabled = true;
        
        // make sure we can't pick up right away
        this.cantPickupTimer = .2f;
        
        var itemRigidBody = this.GetComponent<Rigidbody2D>();
        
        var playerVelocity = this.GetComponent<Rigidbody2D>().velocity;
        float vspeed = 800f;
        float hspeed = facingRight ? vspeed/2f : -vspeed/2f;
        Debug.Log("throwing with force " + hspeed + "," + vspeed);
        itemRigidBody.velocity = playerVelocity;
        itemRigidBody.AddForce(new Vector2(hspeed, vspeed));

        bool keepHolding = false;
        return keepHolding;
    }
}
