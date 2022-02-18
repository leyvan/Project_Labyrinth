using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory List", menuName = "Inventory", order = 5)]
public class InventorySO : ScriptableObject
{
    public List<GameObject> itemInventory = new List<GameObject>();
    public List<GameObject> skillInventory = new List<GameObject>();


    public void fillInventory(GameObject item = null, GameObject skill = null)
    {
        if(item != null)
        {
            itemInventory.Add(item);
        }

        if (skill != null)
        {
            skillInventory.Add(skill);
        }
    }

    public void removeInventory(GameObject item = null, GameObject skill = null)
    {
        if(item != null && itemInventory.Contains(item))
        {
            itemInventory.Remove(item);
        }

        if(skill != null && skillInventory.Contains(skill))
        {
            skillInventory.Remove(skill);
        }
    }
}
