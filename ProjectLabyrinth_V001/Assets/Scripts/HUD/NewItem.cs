using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private ItemType itemType;
    [SerializeField] private int itemStack;
    [SerializeField] private Sprite itemImage;

    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    public ItemType ItemType
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
