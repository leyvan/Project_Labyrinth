using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CombatUtil;

[System.Serializable]
public class BaseEnemy
{
    public Species species;
    public Element element;

    public string name;

    public float hpMax;
    public float hpCur;

    public float mpMax;
    public float mpCur;

    public float attack;
    public float magic;
    public float defense;
    public float speed;
    public float luck;

    public int stress = 100;

    //Character's Attack, defense and speed will go up through certain actions, either by integer, or multipliers
    public float atkMult, defMult, spdMult = 1.0f;
    public float atkMod, defMod, spdMod = 0;

    public List<int> earnableXPRange;
    public List<int> earnableGoldRange;
    public List<BaseItem> earnableItems;

    public BaseEnemy(BaseEnemy _baseEnemy)
    {
        species = _baseEnemy.species;
        element = _baseEnemy.element;
        name = _baseEnemy.name;
        hpMax = _baseEnemy.hpMax;
        hpCur = _baseEnemy.hpCur;
        attack = _baseEnemy.attack;
        magic = _baseEnemy.magic;
        defense = _baseEnemy.defense;
        speed = _baseEnemy.speed;
        luck = _baseEnemy.luck;
        stress = 100;
        earnableXPRange = _baseEnemy.earnableXPRange;
        earnableGoldRange = _baseEnemy.earnableGoldRange;
        earnableItems = _baseEnemy.earnableItems;
    }
}
