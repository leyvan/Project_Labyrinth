using System.Collections;
using System.Collections.Generic;


public class BaseEnemy
{
    public enum Species
    {
        VULPES,
        LUPES,
        OTHER
    }

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

    public Species species;
    public Element element;

    public string name;

    public int hpMax;
    public int hpCur;

    public int mpMax;
    public int mpCur;

    public int attack;
    public int magic;
    public int defense;
    public int speed;
    public int luck;

    public int stress = 100;

    //Character's Attack, defense and speed will go up through certain actions, either by integer, or multipliers
    public float atkMult, defMult, spdMult = 1.0f;
    public float atkMod, defMod, spdMod = 0;
}
