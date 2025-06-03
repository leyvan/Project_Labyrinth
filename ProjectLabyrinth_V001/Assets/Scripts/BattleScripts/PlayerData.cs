using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerData : MonoBehaviour
{
    [FormerlySerializedAs("rufio")] [SerializeField] private CharacterStats mainCharacterStats;
    
    [SerializeField] public XPDataScriptableObject XPLevelConfig;

    public void LevelUp()
    {
        if (mainCharacterStats.level < XPLevelConfig.MaxLevel)
        {
            mainCharacterStats.level++;
            mainCharacterStats.requiredXP = XPLevelConfig.GetRequiredXP(mainCharacterStats.level);
        }
    }

    public void EarnExp(int earnedExp)
    {
        int currentXP = mainCharacterStats.requiredXP + earnedExp;

        if (currentXP >= mainCharacterStats.requiredXP)
        {
            LevelUp();
            currentXP = Mathf.Abs(mainCharacterStats.requiredXP - earnedExp);
            if (currentXP >= mainCharacterStats.requiredXP)
            {
                EarnExp(currentXP);
            }
            else
            {
                mainCharacterStats.currentXP = currentXP;
            }
        }
        else
        {
            mainCharacterStats.currentXP += earnedExp;
        }
    }

    public void EarnGold(int gold)
    {
        SetPlayerGold(GetPlayerGold() + gold);
    }
    
    public int GetPlayerLevel() { return mainCharacterStats.level; }
    public int GetPlayerCurrentXP() { return mainCharacterStats.currentXP; }
    public int GetPlayerGold() { return mainCharacterStats.gold; }
    
    public int GetRequiredExp(){return mainCharacterStats.requiredXP;}

    public void SetPlayerLevel(int level) { mainCharacterStats.level = level; }

    public void SetPlayerCurrentXP(int xp) { mainCharacterStats.currentXP = xp; }
    public void SetPlayerGold(int gold) { mainCharacterStats.gold = gold; }
    
    public CharacterStats GetMainCharacterStats() { return mainCharacterStats; }

    public float GetMainCharacterAttackDmg() { return mainCharacterStats.attack; }
    public float GetMainCharacterCurrentHealth() { return mainCharacterStats.curHealth; }
    public void SetMainCharacterCurrentHealth(float value) { mainCharacterStats.curHealth = value;}

    public void ResetHealth() { mainCharacterStats.curHealth = mainCharacterStats.maxHealth; }

}
