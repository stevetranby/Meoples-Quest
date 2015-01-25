using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

    List<InventoryItem> playerInventory;
    List<InventoryItem> inventoryItems;
    List<InventoryItem> craftableItems;
    Dictionary<GameObject, InventoryItem> playerActiveItem;

    void CreateInventoryItems() 
    {
        inventoryItems = new List<InventoryItem>();
        playerActiveItem = new Dictionary<GameObject, InventoryItem>();

//        // rock
//        {
//            var item = new InventoryItem();
//            item.itemId = "rockSmall";
//            item.title = "Small Rock";
//            item.iconName = "";
//            item.soundName = "";
//            inventoryItems.Add(item);
//        }
//
//        // rope
//        {
//            var item = new InventoryItem();
//            item.itemId = "rope";
//            item.title = "Rope";
//            item.iconName = "";
//            item.soundName = "";
//            inventoryItems.Add(item);
//        }
    }

    void CreateCraftItems()
    {
        craftableItems = new List<InventoryItem>();

//        // pickaxe
//        {
//            var item = new InventoryItem();
//            item.itemId = "pickaxe";
//            item.title = "Pick Axe";
//            item.iconName = "";
//            item.soundName = "";
//            craftableItems.Add(item);
//        }
//        // grappling hook
//        {
//            var item = new InventoryItem();
//            item.itemId = "grapplinghook";
//            item.title = "Grappling Hook";
//            item.iconName = "";
//            item.soundName = "";
//            craftableItems.Add(item);
//        }
    }

	// Use this for initialization
	void Start () {
        playerInventory = new List<InventoryItem>();

        CreateInventoryItems();
        CreateCraftItems();
	}
	
    void addItem(InventoryItem item) {
//        inventoryBag.Add(item.ItemID, item);
//        inventoryArray.Add(item);
    }
    
    void useItem(string ItemID) {
//        var item = inventoryBag[ItemID];
        //start corouting item.UseAction;
        //change anim to: item.UseAnimation;
    }
    
    
    void dropItem(string ItemID) {
        //var item = inventoryBag.Remove(ItemID);
        // todo: drop back in world
        
    }

    void TryCraft(InventoryItem item1, InventoryItem item2) 
    {

    }
}
