using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Enemy : MonoBehaviour
{
    private int hpMax = 0; //max hp: depends on monster
    private int hp = 0; //current hp
    private int mpMax = 0; //max mp: depends on monster
    private int mp = 0;//current mp
    private int atk = 0; //attack value
    private int mag = 0; //magic value
    private int def = 0; //defense value
    private int moves = 1; //number of moves it has: assigned '1' to avoid a potential error
    private int lvl = 0;
    private double[] pow; //determines power of certain attacks (1.2 = 120)
    private string name;

    public Enemy(string name, int healthMax, int manaMax, int attack,
        int magic, int moves, double[] power, int defense, int level)
    {
        this.name = name;
        hpMax = healthMax;
        hp = healthMax;
        mpMax = manaMax;
        mp = manaMax;
        atk = attack;
        mag = magic;
        this.moves = moves;
        pow = power;
        def = defense;
        lvl = level;
    }

    public int Hp
    {
        get { return hp; }
    }

    public int Mp
    {
        get { return mp; }
    }

    public int Atk
    {
        get { return atk; }
    }

    public int Mag
    {
        get { return mag; }
    }

    public int Def
    {
        get { return def; }
    }

    public int Moves
    {
        get { return moves; }
    }

    public double[] Pow
    {
        get { return pow; }
    }

    public string EName
    {
        get { return name; }
    }

    public abstract void Attacking();
}
