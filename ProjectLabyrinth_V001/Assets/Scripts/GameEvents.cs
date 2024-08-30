using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState;
using System;

public class GameEvents : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static GameEvents current;
    void Awake()
    {
        current = this;
    }
    
    public event Action<bool> onPlayerInMenu;
    public void PlayerInMenu(bool isPlayerInMenu)
    {
        if (onPlayerInMenu != null)
        {
            onPlayerInMenu(isPlayerInMenu);
        }
    }

    public event Action<GameObject> onAddItemToInventory;
    public void AddItemToInventory(GameObject item)
    {
        if (onAddItemToInventory != null)
        {
            onAddItemToInventory(item);
        }
    }
    
    public event Action<GameObject> onRemoveItemFromInventory;
    public void RemoveItemFromInventory(GameObject item)
    {
        if (onRemoveItemFromInventory != null)
        {
            onRemoveItemFromInventory(item);
        }
    }
    
    public event Action<int> onIncrementItemStack;
    public void IncrementItemStack(int newItemStackSize)
    {
        if (onIncrementItemStack != null)
        {
            onIncrementItemStack(newItemStackSize);
        }
    }
    
    public event Action<ItemSlot> onAddNewItemTypeToInventory;
    public void AddNewItemTypeToInventory(ItemSlot newItem)
    {
        if (onAddNewItemTypeToInventory != null)
        {
            onAddNewItemTypeToInventory(newItem);
        }
    }

}
