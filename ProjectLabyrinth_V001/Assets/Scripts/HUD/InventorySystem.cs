using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InventoryUtility;


//Use Game Events here

public class InventorySystem : MonoBehaviour
{
    private Dictionary<int, ItemSlot> inventory = new Dictionary<int, ItemSlot>();
    private GameObject inventorySpaces;

    private void Awake()
    {
        inventorySpaces = GameObject.FindGameObjectWithTag("InventoryUI").transform.Find("InventorySpaces").gameObject;
    }
    
//Adding to the Inventory
    private void AddItemToInventory(GameObject itemGameObject)
    {
        var itemScript = itemGameObject.GetComponent<NewItem>();
        foreach (KeyValuePair<int, ItemSlot> item in inventory)
        {
            if (item.Value.itemName == itemScript.ItemName)
            {
                item.Value.itemID = item.Key;
                AddToExistingItemInInv(item.Value, itemScript);
                break;
            }
        }
        AddNewItemToInventory(itemScript);
    }

    private void AddToExistingItemInInv(ItemSlot newItem, NewItem newItemScript)
    {
        newItem.itemAmount += newItemScript.ItemStack;

        AddToExistingItemInInventoryHUD(newItem);
    }

    private void AddNewItemToInventory(NewItem newItemScript)
    {
        var lastIndex = inventory.Count;
        while (inventory.ContainsKey(lastIndex))
        {
            lastIndex++;
        }

        ItemSlot newItemSlot = new ItemSlot(lastIndex, newItemScript.ItemName, newItemScript.ItemStack, newItemScript.ItemType);
        inventory.Add(lastIndex, newItemSlot);
        
        GameEvents.current.AddNewItemTypeToInventory(newItemSlot);
        AddNewToInventoryHUD(newItemSlot);
    }

//Removing from the Inventory

    private void RemoveItemFromInventory(GameObject itemGameObject)
    {
        var itemScript = itemGameObject.GetComponent<NewItem>();
        foreach (KeyValuePair<int, ItemSlot> item in inventory)
        {
            if (item.Value.itemName == itemScript.ItemName)
            {
                if (item.Value.itemAmount > 1)
                {
                    RemoveFromExistingItem(item.Value, itemScript);
                    break;
                }
                else if(item.Value.itemAmount == 0)
                {
                    DeleteItem(item.Key);
                    break;
                }
            }
        }

    }

    private void DeleteItem(int itemID)
    {
        inventory.Remove(itemID);
    }

    private void RemoveFromExistingItem(ItemSlot itemValue, NewItem itemScript)
    {
        itemValue.itemAmount -= itemScript.ItemStack;
    }

    
    // UI Scripts - Solely for connecting UI Components to Code
    private void AddNewToInventoryHUD(ItemSlot newItem)
    {
        var itemSlotObject = inventorySpaces.transform.GetChild(inventory.FirstOrDefault(x => x.Value == newItem).Key);
        
        if (itemSlotObject == null) return;
        itemSlotObject.GetComponent<ItemUISlot>().ItemInInventory(newItem.itemImage);
    }

    private void AddToExistingItemInInventoryHUD(ItemSlot item)
    {
        var itemSlotObject = inventorySpaces.transform.GetChild(inventory.FirstOrDefault(x => x.Value == item).Key);

        if (itemSlotObject == null) return;
        itemSlotObject.GetComponent<ItemUISlot>().IncrementItem(item.itemAmount);
    }
    
    
    private void RemoveFromInventoryHUD(ItemSlot itemToRemove)
    {
        var itemSlotObject = inventorySpaces.transform.GetChild(inventory.FirstOrDefault(y => y.Value == itemToRemove).Key);

        if (itemSlotObject == null) return;
        itemSlotObject.GetComponent<ItemUISlot>().ItemOutOfInventory();
    }
}


public class ItemSlot
{
    public Sprite itemImage;
    public int itemID;
    public string itemName;
    public int itemAmount;
    public GameObject itemUI;

    public InventoryItemType itemType;
    
    public BaseSkill skill = null;
    //public Consumable consumble = null;

    public ItemSlot(int _itemID, string _itemName, int _itemAmount, InventoryItemType _itemType)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemAmount = _itemAmount;
        itemType = _itemType;

    }
}
