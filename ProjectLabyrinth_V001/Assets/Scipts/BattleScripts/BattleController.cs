using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private Slider playerHealth;
    private List<Slider> enemyHealth = new List<Slider>();
    private Text turnNumberText;
    public int mobValue = 1;
    public int extensions;

    private string turnList;
    public List<GameObject> PlayerParty = new List<GameObject> ();
    public List<GameObject> EnemyParty = new List<GameObject>();

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
    private int deathCounter;

//-----------------------------------------------------------------------------------------------------//------------------------------------------------------------------//


    // Start is called before the first frame update
    void Start()
    {
        EnemyParty.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        PlayerParty.AddRange(GameObject.FindGameObjectsWithTag("Ally"));

        yesButton = menu.transform.GetChild(4).GetChild(0).GetComponent<Button>();      //Ref to yesbutton
        turnNumberText = menu.transform.GetChild(0).GetComponent<Text>();               //Ref to turn number text

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
        deathCounter = 0;

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
                case BattleState.PLAYER:
                    Debug.Log("Player Turn");
                    startTurn = true;
                    canSelectEnemy = true;
                    yield return new WaitUntil(() => onClickBool == true);      //Wait until button bool is true, which means button was pressed
                    canSelectEnemy = false;
                    PlayerTurn(/* Put here the button the player pressed and ref to button pressed*/);
                    yield return new WaitForSeconds(1);
                    SwitchTurns(BattleState.ENEMY);
                    break;
                case BattleState.ENEMY:
                    Debug.Log("Enemy Turn ");
                    startTurn = false;
                    foreach(GameObject enemy in EnemyParty)
                    {
                        EnemyTakeTurns(enemy);
                        yield return new WaitForSeconds(1);
                    }
                    SwitchTurns(BattleState.PLAYER);
                    yield return new WaitForSeconds(1);
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
                //SceneManager.LoadScene("Out", LoadSceneMode.Single);
                Debug.Log("YOU LOST!!!");
            }
        }
    }
    
    //Sets up stats for enemies and players
    private void BattleSetUp()
    {
        //Player Set Up
        if(rAlive == true)
        {
            playerHealth = PlayerParty[0].transform.GetChild(0).GetChild(0).GetComponent<Slider>();
            rName = rufio.name;
            rMaxHealth = rufio.maxHealth;
            rCurrentHealth = rMaxHealth;
            rAttack = rufio.attack;

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
        var currentInventory = playerInventory.GetInventory();
        //Vector3 pos = menu.transform.GetChild(3).GetChild(0).transform.position;
        int itemHeldNumb;
        int itemMaxUses;
        string ppText;

        foreach (Transform child in menu.transform.GetChild(3).transform)
        {
            foreach (InventorySlot item in currentInventory)
            {
                if(child.gameObject.name == item.skill.skillName)
                {
                    child.gameObject.SetActive(true);
                    itemHeldNumb = item.amount;
                    itemMaxUses = item.skill.maxUses;

                    ppText = "x" + itemMaxUses + " : " + itemHeldNumb;
                    child.transform.GetChild(0).GetComponent<Text>().text = ppText;
                    activeMenuOptions.Add(item);

                    //pos.y -= 46;
                    
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


        Debug.Log("Executing Player Actions");

        if(buttonPressed == "Fire Ball")
        {

            Debug.Log("Fire Ball");
        }
        else if(buttonPressed == "Lightning Bolt")
        {
            Debug.Log("Lightning Bolt");
        }
        else if (buttonPressed == "WhirldWind")
        {
            Debug.Log("WhirldWind");
        }
        else if (buttonPressed == "Heavy Slash")
        {
            Debug.Log("Heavy Slash");
        }
        else if (buttonPressed == "UpperCut")
        {
            Debug.Log("UpperCut");
        }
        else if (buttonPressed == "BasicAttack")
        {
            Debug.Log("BasicAttack");
        }

        if(buttonPressed != "BasicAttack")
        {
            var skill = activeMenuOptions.Find(i => i.skill.skillName == buttonPressed);
            UpdateItemList(skill);
        }


        // if(buttonPressed == gun)
        if (enemySelected == null)
        {
            enemySelected = EnemyParty[0];
        }
        DealDamage(GetEnemySelected());

    }

    //Enemy Turn Execution
    private void EnemyTakeTurns(GameObject enemy)
    {
        Debug.Log("Enemy #"+ EnemyParty.IndexOf(enemy));
        enemyTurnSelector.transform.position = new Vector3(enemy.transform.position.x, 0.01f, enemy.transform.position.z);
        TakeDamageFrom(enemy);
    }

    private void SwitchTurns(BattleState nextBattleState)
    {

        //Iterate turn number and display
        turnNumber++;
        turnNumberText.text = "Turn:" + turnNumber.ToString();

        state = nextBattleState;
        StartCoroutine(TurnBasedBattle());
    }

    void UpdateItemList(InventorySlot skillUsed)
    {
        if(skillUsed.skill.maxUses > 0)
        {
            skillUsed.skill.maxUses -= 1;
            
        }
        else if(skillUsed.amount > 0)
        {
            Debug.Log("You Can Reload");
        }
        else
        {
            lastAttackSelected.interactable = false;
            lastAttackSelected.transform.GetChild(0).GetComponent<Text>().text = "x0: 0";
        }

        
    }

    //On button click do this
    public void TurnExecution()  //Turn confirmation
    {
        onClickBool = true;
        Debug.Log(onClickBool);
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
            selector.transform.position = gameObject.transform.position + new Vector3(0, 1.5f, 0);
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
            currentEnemyHealth -= rAttack;

            enemyBehavior.TakeDamage(rAttack);
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
                //CurrentClickedGameObject(enemy);  //Change the name maybe

                var enemyAttack = enemyBehavior.GetAttackDmg();

                rCurrentHealth -= enemyAttack;
                playerHealth.value = rCurrentHealth / rMaxHealth;

        }
        else
        {
            rAlive = false;
            StartCoroutine(TurnBasedBattle());
        }
    }

    void SetUpEnemySelector()
    {
        if (EnemyParty.Count < 1) return;

        var firstEnemy = EnemyParty[0].transform;
        selector.transform.position = firstEnemy.position + new Vector3(0, 1.5f, 0);
        enemyTurnSelector.transform.position = firstEnemy.transform.position - new Vector3(0, firstEnemy.transform.position.y+0.01f, 0);

    }

    void Escaping()
    {
        int escapeValue = 1;
        int randomValue = Random.Range(1, 2);

        if (escapeValue == randomValue)
        {
            eAlive = false;
            Debug.Log("Escape Successful");
        }
        else
        {
            Debug.Log("Escape Failed");
        }
    }

    private void AllEnemiesDefeated()
    {
        Debug.Log("You Win");
        StartCoroutine(LevelManager.Instance.LoadLevelScene());
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
                if (mobBehavior.GetCurrentHealth() == 0)
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
                        selector.transform.position = EnemyParty[nextEnemyIndex].transform.position + new Vector3(0, 1.5f, 0);
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

    /*
    void BattleEvent()
    {

    }

    void DetectPlayerParty()
    {

    }

    void DetectEnemyParty()
    {

    }
    void RInput()
    {

    }

    void AInput()
    {

    }
    */
}
