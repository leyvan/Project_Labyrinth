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
    public string skillName;
    public Attribute attribute;

    public int manaCost;
    public int power;
}
