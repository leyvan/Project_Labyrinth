using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack: MonoBehaviour
{
    private float dmgMult = 2;
    [SerializeField] private SkillListScriptableObject skillConfig;
    
    public float SelectSkill(string attack)
    {
        var skill = skillConfig.GetSkillByName(attack);
        return skill.power;
    }
}
