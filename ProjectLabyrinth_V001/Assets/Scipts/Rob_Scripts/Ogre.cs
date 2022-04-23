using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Ogre : Enemy
{
    private int critRate;
    public Ogre(string name, int healthMax, int manaMax, int attack, int magic, int moves, double[] power,
        int defense, int critical, int level) : base(name, healthMax, manaMax, attack, magic, moves, power, defense, level)
    {
        critRate = critical;
    }

    public override void Attacking()
    {
        /*
        if (damage < 1)
        {
            damage = 1;
        }


        if (crit > 1.0)
        {

        }
        */

    }
    /*
    public double Critical()
    {
        int crit = rand.Next(1, 101);
        if (critRate > crit)
        {
            return 1.25;
        }
        else
        {
            return 1.00;
        }
    }*/
}