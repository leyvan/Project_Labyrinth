using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New XP Config", menuName = "Combat/XPConfig", order = 13)]
public class XPDataScriptableObject : ScriptableObject
{
    [Header("LEVEL CURVE")]
    public AnimationCurve levelCurve;
    public int MaxLevel;
    public int RequiredXPCap;
    
    public int GetRequiredXP(int level)
    {
        int requiredXP = Mathf.RoundToInt(levelCurve.Evaluate(Mathf.InverseLerp(0f, MaxLevel, level)) * RequiredXPCap);
        return requiredXP;
    }
}
