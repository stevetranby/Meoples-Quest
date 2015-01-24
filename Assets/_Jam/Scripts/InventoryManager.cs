using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

    List<InventoryItem> inventoryItems;
    List<InventoryItem> craftableItems;

    void CreateInventoryItems() 
    {
        // rock
        // rope
    }

    void CreateCraftItems()
    {
        // pickaxe
        {
            var item = new InventoryItem();
            item.itemId = "pickaxe";
            item.title = "Pick Axe";
            item.iconName = "";
            item.soundName = "";
        }
        // grappling hook
        {
            var item = new InventoryItem();
            item.itemId = "grapplinghook";
            item.title = "Grappling Hook";
            item.iconName = "";
            item.soundName = "";
        }
        //
        {
            var item = new InventoryItem();
            item.itemId = "grapplinghook";
            item.title = "Grappling Hook";
            item.iconName = "";
            item.soundName = "";
        }
        // 
        {
            var item = new InventoryItem();
            item.itemId = "grapplinghook";
            item.title = "Grappling Hook";
            item.iconName = "";
            item.soundName = "";
        }
    }

	// Use this for initialization
	void Start () {
        inventoryItems = new List<InventoryItem>();

        // TEST CREATE
        var item = new InventoryItem();
        inventoryItems.Add(item);
	}
	
    void TryCraft(InventoryItem item1, InventoryItem item2) 
    {

    }
}
