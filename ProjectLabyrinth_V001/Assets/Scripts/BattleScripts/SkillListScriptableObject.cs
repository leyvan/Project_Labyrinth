using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillsConfig", menuName = "Combat/SkillsConfig", order = 13)]
[System.Serializable]
public class SkillListScriptableObject : ScriptableObject
{
    [Header("List of Usable Skills")]
    public List<BaseSkill> skills = new List<BaseSkill>();

    public BaseSkill GetSkillByName(string name)
    {
        return skills.Find(x => x.skillName == name);
    }
}
