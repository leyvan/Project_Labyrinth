using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory List", menuName = "Inventory", order = 5)]
public class InventorySO : ScriptableObject
{
    public List<InventorySlot> skillInventory = new List<InventorySlot>();


    public void fillInventory(BaseSkill _skill, int _amount)
    {
        bool hasItem = false;
        for(int i = 0; i < skillInventory.Count; i++)
        {
            if(skillInventory[i].skill == _skill)
            {
                skillInventory[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if(hasItem == false)
        {
            skillInventory.Add(new InventorySlot(_skill, _amount));
        }
    }

}

[System.Serializable]
public class InventorySlot
{
    public BaseSkill skill;
    public int amount;
    public InventorySlot(BaseSkill _skill, int _amount)
    {
        skill = _skill;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
