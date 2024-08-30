using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory List", menuName = "Inventory", order = 5)]
public class InventorySO : ScriptableObject
{
    public List<InventorySlot> skillInventory = new List<InventorySlot>();
    public List<InventorySlot> consumablesInventory = new List<InventorySlot>();

    public void fillInventory(BaseSkill _skill, int _amount)
    {
        bool hasItem = false;
        for(int i = 0; i < skillInventory.Count; i++)
        {
            if(skillInventory[i].skill == _skill)
            {
                skillInventory[i].AddAmount(_amount);
                skillInventory[i].noMoreLeft = false;
                hasItem = true;
                break;
            }
        }
        if(hasItem == false)
        {
            skillInventory.Add(new InventorySlot(_skill, _amount));

        }
    }

    public void fillConsumablesInv(BaseItem consumable, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < consumablesInventory.Count; i++)
        {
            if (consumablesInventory[i].skill == consumable)
            {
                consumablesInventory[i].AddAmountToConsumable(_amount);
                consumablesInventory[i].noMoreLeft = false;
                hasItem = true;
                break;
            }
        }
        if (hasItem == false)
        {
            consumablesInventory.Add(new InventorySlot(null, 0,consumable, _amount));

        }
    }

    public List<InventorySlot> GetSkillInventory()
    {
        return skillInventory;
    }

}

[System.Serializable]
public class InventorySlot
{
    public BaseSkill skill;
    public BaseItem consumable;
    public int amount;
    public int consumableAmount;
    public bool noMoreLeft;
    public InventorySlot(BaseSkill _skill = null, int _amount = 0, BaseItem consum = null, int _consumableAmount = 0)
    {
        skill = _skill;
        consumable = consum;
        amount = _amount;
        consumableAmount = _consumableAmount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void AddAmountToConsumable(int value)
    {
        consumableAmount += value;
    }
}
