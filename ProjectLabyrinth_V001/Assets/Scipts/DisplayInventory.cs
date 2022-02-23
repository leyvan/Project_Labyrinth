using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public InventorySO inventory;
    
    public GameObject inventorySlot;

    public void AddInventorySlot()
    {
        Instantiate(inventorySlot, transform);
    }
}
