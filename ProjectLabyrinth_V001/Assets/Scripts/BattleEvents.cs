using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents current;
    void Awake()
    {
        if(current == null)
            current = this;
        else
            Destroy(gameObject);
    }
    
    public event Action<bool> onBattleMenuToggle;
    public void BattleMenuToggle(bool toggleBattleMenu)
    {
        onBattleMenuToggle?.Invoke(toggleBattleMenu);
    }
}
