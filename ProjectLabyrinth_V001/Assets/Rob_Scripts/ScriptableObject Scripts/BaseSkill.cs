using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attribute
{
    PHYSICAL,
    FIRE,
    WATER,
    ELECTRIC,
    WIND,
    LIGHT,
    DARK
}

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill", order = 11)]
public class BaseSkill : ScriptableObject
{
    public GameObject skillCrystal;
    public string skillName;
    public Attribute attribute;

    [TextArea(15, 20)]  public string description;

    public int maxUses;  //How many times you can use it, like pp in pokemon
    public int power;
}
