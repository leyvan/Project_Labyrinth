using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character", order = 1)]
public class CharacterStats : ScriptableObject
{
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
    public string chName;

    public int level;



    public float maxHealth;
    public float curHealth;

    public float maxMana;
    public float curMana;

    public float attack;
    public float magic;
    public float defense;
    public float speed;
    public float luck;
}
