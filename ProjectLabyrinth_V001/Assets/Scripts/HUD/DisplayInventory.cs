using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public InventorySO inventory;
    public GameObject inventorySlot;

    public List<GameObject> skillSlots = new List<GameObject>();

    [SerializeField] public Text inventoryCountText;
    public int inventoryCount = 0;

    public void UpdateInventorySlot(BaseSkill _skill)
    {
        Debug.Log("UpdateInventorySlot ============================");
        var count = 0;
        foreach (InventorySlot item in inventory.skillInventory)
        {
            if (_skill == item.skill)
            {
                if(item.amount > 1)
                {
                    int _amount = item.amount;
                    skillSlots[0].transform.GetChild(0).GetComponent<Text>().text = _amount.ToString();
                    count++;
                    Debug.Log("Heloo");
                }
                else
                {
                    inventorySlot.transform.GetChild(0).GetComponent<Text>().text = "1";
                    var obj = Instantiate(inventorySlot, transform);
                    skillSlots.Add(obj);
                    Debug.Log("Bye");
                    inventoryCount += 1;    
                }
                
            }
        }
        
        inventoryCountText.text = inventoryCount.ToString();
    }

    public void ReloadInventoryOnLoad()
    {
        var tempList = new List<InventorySlot>(inventory.skillInventory);
        tempList.Reverse();
        foreach (InventorySlot item in tempList)
        {
            int _amount = item.amount;
            var obj = Instantiate(inventorySlot, transform);
            obj.transform.GetChild(0).GetComponent<Text>().text = _amount.ToString();
        }
        
        inventoryCountText.text = inventoryCount.ToString();
    }
}
