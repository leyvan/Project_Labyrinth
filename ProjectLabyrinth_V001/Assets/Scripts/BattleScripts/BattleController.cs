using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public enum BattleState
{
    START,
    EXECUTE,
    PLAYER, ENEMY,
    OUTCOME,
    WIN, LOSE, ESCAPE
}

public enum RoundControl
{
    FIGHT,
    TALK,
    SPECIAL,
    SPECIALCONF,
    PARTYSWAP,
    PARTYSWAPCONF,
    ESCAPE,
    ESCAPECONF,
    AUTO,
    AUTOCONF,
    ROUNDCONF
}

public enum RufioTurnCommand
{
    OFF, //Disables this enum
    MELEE,
    MELEECONF,
    GUN,
    GUNCONF,
    DEFEND,
    SKILL,
    SKILLCONF,
    ITEM,
    ITEMCONF
}

public enum AllyTurnCommand
{
    OFF, //Disables this enum
    ATTACK,
    ATTACKCONF,
    DEFEND,
    SKILL,
    SKILLCONF
}

public class BattleController : MonoBehaviour
{
    public BattleState state;
    public RoundControl command;
    public RufioTurnCommand rufioTurn;
    public AllyTurnCommand allyTurn;
    public CharacterStats rufio;
    public CharacterStats enemy1;
    public Canvas menu;
    private Button yesButton;
    
    private List<Slider> enemyHealth = new List<Slider>();
    private Text turnNumberText;
    public int mobValue = 1;
    public int extensions;

    private string turnList;
    public List<GameObject> PlayerParty = new List<GameObject> ();
    public List<GameObject> EnemyParty = new List<GameObject>();
    
    private GameObject playerInfo;
    private Slider battleHealthBar;

    private List<GameObject> DeadEnemies = new List<GameObject>();

    LinkedList<string> TurnOrder = new LinkedList<string>();
    private bool turnSuccess;
    private bool battleOutcome;
    public bool startTurn;
    private int turnNumber = 1;

    [HideInInspector]
    public bool onClickBool;
    public bool canSelectEnemy = false;

    //Rufio---------------------------------///---------------------------------------//
    //public GameObject currentPlayer;
    private string rName;
    private float rMaxHealth;
    [SerializeField] private float rCurrentHealth;  //Rufio Health
    private float rAttack;  //Rufio Attack Damage
    private float currentAttackDmg;
    [SerializeField] private bool rAlive = true;

    // Enemy---------------------------------///--------------------------------------//
    // Ogre
    //public GameObject currentEnemy;
    private string eName;
    private float eMaxHealth;
    [SerializeField] private float eCurrentHealth;  //Enemy Health
    private float eAttack;
    [SerializeField] private bool eAlive = true;

    public List<InventorySlot> activeMenuOptions = new List<InventorySlot>();
    public InventorySO playerInventory;
    private string buttonPressed;

    public Image selector;
    private GameObject enemySelected;
    public Image enemyTurnSelector;

    private Button lastAttackSelected;
    //private int deathCounter;

    private SkillAttack skillAttackScript;

    private CinemachineClearShot cameraGroup;
    private List<CinemachineVirtualCameraBase> vCamList = new List<CinemachineVirtualCameraBase>();

    private BattleData currentData;
    private GameObject playerChar;
    private Player_Behaviour _playerBehaviour;

//-----------------------------------------------------------------------------------------------------//------------------------------------------------------------------//
    

    // Start is called before the first frame update
    void Start()
    {
        EnemyParty.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        PlayerParty.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        playerChar = PlayerParty[1].gameObject;
        playerInfo = PlayerParty[0];
        battleHealthBar = playerInfo.GetComponentInChildren<Slider>();
        _playerBehaviour = playerChar.GetComponent<Player_Behaviour>();

        currentData = new BattleData();

        cameraGroup = GameObject.FindGameObjectWithTag("CameraSwitch").GetComponent<CinemachineClearShot>();
        for(int i = 0; i < (cameraGroup.ChildCameras.Count());i++)
        {
            vCamList.Add(cameraGroup.ChildCameras[i]);
        }

        yesButton = menu.transform.GetChild(4).GetChild(0).GetComponent<Button>();      //Ref to yesbutton
        turnNumberText = menu.transform.GetChild(0).GetComponent<Text>();               //Ref to turn number text

        skillAttackScript = gameObject.GetComponent<SkillAttack>();

        Debug.Log(EnemyParty.Count);
        BattleSetUp();      //Set Up Battle Stats

        // Have to hard code things for now
        state = BattleState.PLAYER;
        command = RoundControl.FIGHT;
        rufioTurn = RufioTurnCommand.OFF;
        allyTurn = AllyTurnCommand.OFF;
        turnSuccess = false;
        battleOutcome = false;
        startTurn = true;
        //deathCounter = 0;

        StartCoroutine(TurnBasedBattle());
    }

    void Update()
    {
        if(canSelectEnemy == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit))
                {
                    if (raycastHit.transform != null)
                    {
                        Debug.Log("Something Selected");
                        Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red);
                        CurrentClickedGameObject(raycastHit.transform.gameObject);
                    }
                }
            }
        }

    }

    public IEnumerator TurnBasedBattle()
    {
        // detects whether party or enemy team is defeated
        battleOutcome = IsDefeated();  //
        if (!battleOutcome)
        {
            switch (state)
            {
                case BattleState.START:
                    break;
                case BattleState.PLAYER:
                    //Camera Switch
                    vCamList[1].m_Priority = 10;
                    vCamList[0].m_Priority = 11;

                    Debug.Log("Player Turn");
                    startTurn = true;
                    canSelectEnemy = true;
                    yield return new WaitUntil(() => onClickBool == true);      //Wait until button bool is true, which means button was pressed
                    canSelectEnemy = false;
                    PlayerTurn();
                    yield return new WaitForSeconds(1f);
                    SwitchTurns(BattleState.ENEMY);
                    break;
                case BattleState.ENEMY:
                    //Camera Switch

                    vCamList[1].m_Priority = 11;
                    vCamList[0].m_Priority = 10;
                    Debug.Log("Enemy Turn ");
                    startTurn = false;
                    foreach(GameObject enemy in EnemyParty)
                    {
                        enemyTurnSelector.transform.position = new Vector3(enemy.transform.position.x, 0.01f, enemy.transform.position.z);
                        yield return new WaitForSeconds(2f);
                        EnemyTakeTurns(enemy);
                        yield return new WaitForSeconds(1f);
                    }
                    SwitchTurns(BattleState.PLAYER);
                    break;
                case BattleState.ESCAPE:
                    Escaping();
                    break;
            }
        }
        else
        {
            bool outcome = Results();
            // changes the scene here depending on outcome of battle
            if (outcome == true)
            {

                AllEnemiesDefeated();
                //SceneManager.LoadScene("Out", LoadSceneMode.Single);
            }
            else
            {
                
                Debug.Log("YOU LOST!!!");
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
    
    //Sets up stats for enemies and players
    private void BattleSetUp()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        
        //Player Set Up
        if(rAlive == true)
        {
            rName = rufio.name;
            rMaxHealth = _playerBehaviour.GetMaxHealth();
            rCurrentHealth = HealthManager.Instance.GetPersistentHealth();
            rAttack = _playerBehaviour.GetAttackDmg();
            currentAttackDmg = _playerBehaviour.GetAttackDmg();

            battleHealthBar.value = rCurrentHealth / rMaxHealth;
            
            SkillSetUp();
        }
        
        if(eAlive == true)
        {
            //Fix this Later --------------

            if(EnemyParty.Count > 0)
            {
                eName = enemy1.chName;
                eMaxHealth = enemy1.maxHealth;
                eCurrentHealth = eMaxHealth;
                eAttack = enemy1.attack;
                foreach (GameObject mob in EnemyParty)
                {
                    var enemyBehavior = mob.GetComponent<EnemyCombatAI>();
                    enemyBehavior.SetHealth(eCurrentHealth, eMaxHealth);
                    enemyBehavior.SetDamage(eAttack);
                }
            }
        }

        SetUpEnemySelector();
    }

    private void SkillSetUp()
    {
        var currentInventory = playerInventory.GetSkillInventory();
        //Vector3 pos = menu.transform.GetChild(3).GetChild(0).transform.position;
        int itemHeldNumb;
        int itemUsesLeft;
        string ppText;

        foreach (Transform child in menu.transform.GetChild(3).transform)
        {
            foreach (InventorySlot item in currentInventory)
            {
                if(child.gameObject.name == item.skill.skillName)
                {

                    child.gameObject.SetActive(true);

                    ResetItemList(item);

                    itemHeldNumb = item.amount;
                    itemUsesLeft = item.skill.usesLeft;

                    ppText = "x" + itemUsesLeft + " : " + itemHeldNumb;
                    child.transform.GetChild(0).GetComponent<Text>().text = ppText;
                    activeMenuOptions.Add(item);

                    if (item.noMoreLeft == true)
                    {
                        child.gameObject.GetComponent<Button>().interactable = false;
                    }

                }
                
            }
        }
    }


    public void GetActionName(Button action)
    {
        lastAttackSelected = action;
        var actionName = action.name;
        buttonPressed = actionName;
    }

    //Player Turn Execution
    private void PlayerTurn(/*var for ref to player object and var for ref to button pressed*/)
    {
        // Hard code some if/case statements for each button
        startTurn = false; // this should take the menu off the screen
        onClickBool = false;        //Resets button

        if (enemySelected == null)
        {
            enemySelected = EnemyParty[0];
        }

        Debug.Log("Executing Player Actions");
        if (buttonPressed == "BasicAttack")
        {
            currentAttackDmg = rAttack;
            PlayerParty[0].GetComponentInChildren<Animator>().SetTrigger("PhysicalAttack");
            Debug.Log("BasicAttack");
            DealDamage(GetEnemySelected());
        }
        else if(buttonPressed == "Self Heal")
        {
            currentAttackDmg = 0;
            var amountToHealBy = skillAttackScript.SelectSkill(buttonPressed);
            currentData.numberOfItemsUsed += 1;
            HealPlayer(amountToHealBy);
            UpdateItemList(activeMenuOptions.Find(i => i.skill.skillName == "Self Heal"));
        }
        else
        {
            currentAttackDmg = skillAttackScript.SelectSkill(buttonPressed);
            var skill = activeMenuOptions.Find(i => i.skill.skillName == buttonPressed);
            currentData.numberOfItemsUsed += 1;
            if(skill.skill.skillName == "Heavy Slash")
            {
                PlayerParty[0].GetComponentInChildren<Animator>().SetTrigger("SwordAttack");
            }
            else {
                _playerBehaviour.DoMagicAttackAnim();
            }

            
            Debug.Log(currentData.numberOfItemsUsed);
            UpdateItemList(skill);

            DealDamage(GetEnemySelected());
        }
    }

    //Enemy Turn Execution
    private void EnemyTakeTurns(GameObject enemy)
    {
        Debug.Log("Enemy #"+ EnemyParty.IndexOf(enemy));
        

        TakeDamageFrom(enemy);
    }

    private void SwitchTurns(BattleState nextBattleState)
    {
        if(buttonPressed != "BasicAttack" )
        {
            var skill = activeMenuOptions.Find(i => i.skill.skillName == buttonPressed);
            CheckItemList(skill);
        }


        //Iterate turn number and display
        if(nextBattleState == BattleState.PLAYER)
        {
            turnNumber++;
            turnNumberText.text = "Turn:" + turnNumber.ToString();
            currentData.turnsTaken += 1;
        }


        state = nextBattleState;
        StartCoroutine(TurnBasedBattle());
    }

    void UpdateItemList(InventorySlot skillUsed)
    {
        if(skillUsed.skill.usesLeft > 0)
        { 
            skillUsed.skill.usesLeft -= 1;
            var ppText = "x" + skillUsed.skill.usesLeft + " : " + skillUsed.amount;
            lastAttackSelected.transform.GetChild(0).GetComponent<Text>().text = ppText;
        
        }
        else
        {
            if (skillUsed.amount > 0)
            {
                skillUsed.amount -= 1;
                skillUsed.skill.usesLeft = skillUsed.skill.maxUses;
                var ppText = "x" + skillUsed.skill.usesLeft + " : " + skillUsed.amount;
                lastAttackSelected.transform.GetChild(0).GetComponent<Text>().text = ppText;
            }
            else
            {
                lastAttackSelected.interactable = false;
                lastAttackSelected.transform.GetChild(0).GetComponent<Text>().text = "x0: 0";
            }
 
        }
    }

    void CheckItemList(InventorySlot skillUsed)
    {
        if(buttonPressed == "Escape") return;
        if (skillUsed.skill.usesLeft <= 0)
        {
            if(skillUsed.amount <= 0)
            {
                lastAttackSelected.interactable = false;
                lastAttackSelected.transform.GetChild(0).GetComponent<Text>().text = "";
            }
            else
            {
                skillUsed.amount -= 1;
                skillUsed.skill.usesLeft = skillUsed.skill.maxUses;
                var ppText = "x" + skillUsed.skill.usesLeft + " : " + skillUsed.amount;
                lastAttackSelected.transform.GetChild(0).GetComponent<Text>().text = ppText;
            }

        }
    }

    void ResetItemList(InventorySlot skillSlot)
    {

        if (skillSlot.amount > 0)
        {
            skillSlot.skill.usesLeft = skillSlot.skill.maxUses;
            skillSlot.amount -= 1;
        }
        else
        {
            if(skillSlot.skill.usesLeft > 0)
            {
                skillSlot.skill.usesLeft = skillSlot.skill.maxUses;
                skillSlot.amount = 0;
            }
            else
            {
                skillSlot.noMoreLeft = true;
            }
            

        }
            
        

    }

    //On button click do this
    public void TurnExecution()  //Turn confirmation
    {
        if(buttonPressed != "Escape")
        {
            onClickBool = true;
            Debug.Log(onClickBool);
        }
    }

    


    public GameObject GetEnemySelected()
    {
        return enemySelected;
    }

    void CurrentClickedGameObject(GameObject gameObject)
    {

        if(gameObject.tag == "Enemy")
        {
            enemySelected = gameObject;
            selector.transform.position = gameObject.transform.position + new Vector3(0, 3f, 0);
            Debug.Log("Enemy Selected, " +  enemySelected);
        }  
        
    }

    //Deal damage to enemy
    public void DealDamage(GameObject enemy)
    {
        if (eAlive == true)
        {
            var enemyBehavior = enemy.GetComponent<EnemyCombatAI>();
            var currentEnemyHealth = enemyBehavior.GetCurrentHealth();
            currentEnemyHealth -= currentAttackDmg;

            if(currentAttackDmg != 0)
            {
                enemyBehavior.TakeDamage(currentAttackDmg);
            }
        }
        else
        {
            eAlive = false;
            StartCoroutine(TurnBasedBattle());
        }
            
    }

    //Deal Damage to player
    public void TakeDamageFrom(GameObject enemy)
    {
        if (rAlive == true)
        {
            var enemyBehavior = enemy.GetComponent<EnemyCombatAI>();
            enemyBehavior.DoAttack();
            //CurrentClickedGameObject(enemy);  //Change the name maybe

            var enemyAttack = enemyBehavior.GetAttackDmg();

            StartCoroutine(TakeDamageAnim(enemyAttack));

        }
        else
        {
            rAlive = false;
            StartCoroutine(TurnBasedBattle());
        }
    }

    void HealPlayer(float healPower )
    {
        rCurrentHealth += healPower;
        if(rCurrentHealth > rMaxHealth)
        {
            rCurrentHealth = rMaxHealth;
            HealthManager.Instance.SetPersistentHealth(rCurrentHealth);
            battleHealthBar.value = rCurrentHealth / rMaxHealth;
            
        }
        else
        {
            HealthManager.Instance.SetPersistentHealth(rCurrentHealth);
            battleHealthBar.value = rCurrentHealth / rMaxHealth;
        }
        
    }

    IEnumerator TakeDamageAnim(float enemyAttack)
    {
        yield return new WaitForSeconds(1f);
        _playerBehaviour.DoTakeHitAnimation();

        

        rCurrentHealth -= enemyAttack;
        HealthManager.Instance.SetPersistentHealth(rCurrentHealth);
        battleHealthBar.value = rCurrentHealth / rMaxHealth;
        
    }

    void SetUpEnemySelector()
    {
        if (EnemyParty.Count < 1) return;

        var firstEnemy = EnemyParty[0].transform;
        selector.transform.position = firstEnemy.position + new Vector3(0, 3f, 0);
        enemyTurnSelector.transform.position = firstEnemy.transform.position - new Vector3(0, firstEnemy.transform.position.y+0.01f, 0);

    }

    public void Escaping()
    {
        int escapeValue = 1;
        int randomValue = Random.Range(1, 5);  //1/4

        if (escapeValue == randomValue)
        {
            eAlive = false;
            Debug.Log("Escape Successful");
            StartCoroutine(LevelManager.Instance.LoadLevelScene());
        }
        else
        {
            Debug.Log("Escape Failed");
            state = BattleState.ENEMY;
            StartCoroutine(TurnBasedBattle());

        }
    }

    private void AllEnemiesDefeated()
    {
        Debug.Log("You Win");
        //End Screen Display

        GameObject.FindGameObjectWithTag("BattleHUD").transform.GetChild(5).gameObject.SetActive(true);
    }

    private bool IsDefeated()
    {
        if (rCurrentHealth > 0)
        {
                 
            int i;
            for(i = 0; i < EnemyParty.Count ;i++)
            {
                Debug.Log("Enemy Party Count: " + EnemyParty.Count);
                var mob = EnemyParty[i];
                var mobBehavior = mob.GetComponent<EnemyCombatAI>();
                if (mobBehavior.GetCurrentHealth() <= 0)
                {
                    int nextEnemyIndex;
                    if(EnemyParty.IndexOf(mob) != (EnemyParty.Count-1)){
                        nextEnemyIndex = EnemyParty.IndexOf(mob) + 1;
                    }
                    else{
                        nextEnemyIndex = 0;
                    }
                    

                    if (enemySelected == mob)
                    {
                        selector.transform.position = EnemyParty[nextEnemyIndex].transform.position + new Vector3(0, 3f, 0);
                        enemySelected = EnemyParty[nextEnemyIndex];
                    }
                    

                    mobBehavior.SetDead(true);
                    mob.SetActive(false);
                    DeadEnemies.Add(mob);
                    enemyTurnSelector.transform.position = EnemyParty[nextEnemyIndex].transform.position - new Vector3(0, EnemyParty[0].transform.position.y + 0.01f, 0);
                }

                Debug.Log("Enemy Death Count" + DeadEnemies.Count);
                //if (DeadEnemies.Count >= EnemyParty.Count) return true;
            }

            if(DeadEnemies.Count > 0)
            {
                currentData.killedEnemies += 1;
                Debug.Log(currentData.killedEnemies);
                var firstDeadEnemy = EnemyParty.IndexOf(DeadEnemies[0]);
                EnemyParty.RemoveRange(firstDeadEnemy, DeadEnemies.Count());

                Debug.Log("New Enemy Party Count: " + EnemyParty.Count);
            }
            

            if (EnemyParty.Count < 1) return true;

            DeadEnemies.Clear();
            return false;
        }
        else if (rCurrentHealth <= 0)
        {
            Debug.Log("You Died");
            rAlive = false;
            eAlive = true;
            return true;
        }
        else
        {
            Debug.Log("Battle Ended");
            rAlive = false;
            eAlive = false;
            return true;
        }
    }

    private bool Results()
    {
        if (rCurrentHealth >  0)
        {
            return true;
        }
        else { return false; }
    }

    public BattleData GetCurrentData()
    {
        return currentData;
    }
}
