using System.Collections;
using System.Collections.Generic;
using InventoryUtility;
using UnityEngine;

public class NewItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private InventoryItemType itemType;
    [SerializeField] private int itemStack;
    [SerializeField] private Sprite itemImage;

    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    public InventoryItemType ItemType
    {
        get => itemType;
        set => itemType = value;
    }

    public int ItemStack
    {
        get => itemStack;
        set => itemStack = value;
    }

    public Sprite ItemImage
    {
        get => itemImage;
        set => itemImage = value;
    }
}
