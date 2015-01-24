using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour
{

    public string itemId;
    public string title;
    public string soundName;
    public string iconName;
    public float mass;

    // Use this for initialization
    void Start()
    {
        itemId = this.gameObject.name;
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
}
