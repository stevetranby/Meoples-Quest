using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryItem {
	public string ItemID;
	public string UseAction;
	public string UseAnimation;
}

public class PlayerInventory : MonoBehaviour {

	public List<InventoryItem> inventoryArray;
	public Dictionary<string, InventoryItem> inventoryBag;

	// Use this for initialization
	void Start () {
		inventoryArray = new List<InventoryItem>();
		inventoryBag = new Dictionary<string,InventoryItem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void addItem(InventoryItem item) {
		inventoryBag.Add(item.ItemID, item);
		inventoryArray.Add(item);
	}

	void useItem(string ItemID) {
		var item = inventoryBag[ItemID];
		//start corouting item.UseAction;
		//change anim to: item.UseAnimation;
	}


	void dropItem(string ItemID) {
		//var item = inventoryBag.Remove(ItemID);
		// todo: drop back in world

	}
}
