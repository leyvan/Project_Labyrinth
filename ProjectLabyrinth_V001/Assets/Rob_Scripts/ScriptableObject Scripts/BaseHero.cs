using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseHero: MonoBehaviour
{
    public enum Element
    {
        PHYSICAL,
        FIRE,
        WATER,
        WIND,
        ELECTRIC,
        LIGHT,
        DARK
    }

    public Element element;

    public string name;

    public int level;

    public int hpMax;
    public int hpCur;

    public int mpMax;
    public int mpCur;

    public int attack;
    public int magic;
    public int defense;
    public int speed;
    public int luck;

    //public bool channel = false; For later
    public int stress = 100;

    //Character's Attack, defense and speed will go up through certain actions, either by integer, or multipliers
    public float atkMult, defMult, spdMult = 1.0f;
    public int atkMod, defMod, spdMod = 0;
}
