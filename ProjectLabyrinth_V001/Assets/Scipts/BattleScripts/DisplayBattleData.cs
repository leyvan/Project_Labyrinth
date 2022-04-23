using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayBattleData : MonoBehaviour
{
    private BattleController battleController;
    private BattleData data;

    private TextMeshProUGUI enemiesDefeated;
    private TextMeshProUGUI itemsUsed;
    private TextMeshProUGUI turnsTaken;

    private void Awake()
    {
        battleController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleController>();
        data = battleController.GetCurrentData();

        enemiesDefeated = this.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        turnsTaken = this.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        itemsUsed = this.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        
    }

    private void Start()
    {
        DisplayEndScreenData();
    }

    public void DisplayEndScreenData()
    {
        enemiesDefeated.text = data.killedEnemies.ToString();
        turnsTaken.text = data.turnsTaken.ToString();
        itemsUsed.text = data.numberOfItemsUsed.ToString();  
    }

    public void ReturnToLevel()
    {
        StartCoroutine(LevelManager.Instance.LoadLevelScene());
    }
}
