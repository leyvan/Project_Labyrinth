using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUISlot : MonoBehaviour
{
    [SerializeField] private bool itemSlotTaken;
    private Sprite itemImage;
    [SerializeField] private Sprite defaultImage;
    private TextMeshProUGUI stackNumber;

    public Sprite ItemImage
    {
        get => itemImage;
        set => itemImage = value;
    }

    public bool ItemSlotTaken
    {
        get => itemSlotTaken;
        set => itemSlotTaken = value;
    }

    public void ItemInInventory(Sprite newItemImage)
    {
        itemSlotTaken = true;
        itemImage = newItemImage;
        stackNumber.text = "1";

        this.GetComponent<Image>().sprite = itemImage;
    }

    public void ItemOutOfInventory()
    {
        itemSlotTaken = false;
        itemImage = defaultImage;

        this.GetComponent<Image>().sprite = itemImage;
    }

    public void IncrementItem(int newStackNumber)
    {
        stackNumber.text = stackNumber.ToString();
    }

    public void DecrementItem(int newStackNumber)
    {
        stackNumber.text = stackNumber.ToString();
    }
}
