using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { HEALING, ATTACKING, DEFENSE, BUFF };

[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 12)]
public class BaseItem : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public int numHeld;
    
}
