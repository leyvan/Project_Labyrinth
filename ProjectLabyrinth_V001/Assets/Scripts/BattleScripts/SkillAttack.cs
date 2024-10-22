using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack: MonoBehaviour
{
    private float dmgMult = 2;
    Object[] skills;

    private void Awake()
    {
        skills = Resources.LoadAll("Base_ScriptableObjects");
    }
    public float SelectSkill(string attack)
    {
        foreach(BaseSkill skill in skills)
        {
            if(attack == skill.skillName)
            {
                return SkillFunc(skill, skill.attribute);
            }
        }

        return 0;
    }

    public float SkillFunc(BaseSkill skill, Attribute type)
    {
        if(type == Attribute.FIRE)
        {
            Debug.Log("Fire Attack");
            return (skill.power);
        }
        else if (type == Attribute.ELECTRIC)
        {
            return (skill.power);
        }
        else if (type == Attribute.WIND)
        {
            return (skill.power);
        }
        else if (type == Attribute.WATER)
        {
            return (skill.power);
        }
        else if (type == Attribute.PHYSICAL)
        {
            return (skill.power);
        }
        else if (type == Attribute.DARK)
        {
            return (skill.power);
        }
        else if (type == Attribute.LIGHT)
        {
            Debug.Log("Self Healing...");
            return (skill.power);
        }
        return skill.power;
        
    }

}
