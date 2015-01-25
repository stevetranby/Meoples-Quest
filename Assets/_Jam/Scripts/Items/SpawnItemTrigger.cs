using UnityEngine;
using System.Collections;

public class SpawnItemTrigger : ItemPickup {

    public ItemPickup spawnItemType;
    public ItemPickup triggerItemType;
    public AudioClip asdf;

    public override void OnTriggerEnter2D(Collider2D other)
    {       
        //Debug.Log("Spawn Trigger Triggered! other.tag = " + other.tag);
        this.enabled = false;

        // If the player enters the trigger zone...

        if (other.tag == "Tool")
        {
            var toolItem = other.gameObject;
            //Debug.Log("player inside item: " + other.gameObject + " :: " + toolItem.name + ", " + triggerItemType.name );

            //if (toolItem.name == triggerItemType.name)
            {
                var clone = Instantiate(spawnItemType, transform.position, transform.rotation);
                // TODO: attach to parent item tree
                Debug.Log("clone created");

//                AudioSource.PlayClipAtPoint(fuse, transform.position);
            }
        }
        Destroy(this);
    }
}
