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
    private Slider enemyHealth;
    private Text turnNumberText;
    public int mobValue = 1;
    public int extensions;

    private string turnList;
    public List<GameObject> PlayerParty = new List<GameObject> ();
    public List<GameObject> EnemyParty = new List<GameObject>();

    LinkedList<string> TurnOrder = new LinkedList<string>();
    private bool turnSuccess;
    private bool battleOutcome;
    public bool startTurn;
    private int turnNumber = 1;

    [HideInInspector]
    public bool onClickBool;

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

    public List<GameObject> activeMenuOptions = new List<GameObject>();
    public InventorySO playerInventory;
    private string buttonPressed;

//-----------------------------------------------------------------------------------------------------//------------------------------------------------------------------//


    // Start is called before the first frame update
    void Start()
    {
        EnemyParty.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        PlayerParty.AddRange(GameObject.FindGameObjectsWithTag("Ally"));

        yesButton = menu.transform.GetChild(4).GetChild(0).GetComponent<Button>();      //Ref to yesbutton
        turnNumberText = menu.transform.GetChild(0).GetComponent<Text>();               //Ref to turn number text


        BattleSetUp();      //Set Up Battle Stats

        // Have to hard code things for now
        state = BattleState.PLAYER;
        command = RoundControl.FIGHT;
        rufioTurn = RufioTurnCommand.OFF;
        allyTurn = AllyTurnCommand.OFF;
        turnSuccess = false;
        battleOutcome = false;
        startTurn = true;

        StartCoroutine(TurnBasedBattle());
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
                    yield return new WaitUntil(() => onClickBool == true);      //Wait until button bool is true, which means button was pressed
                    yield return new WaitForSeconds(1);
                    PlayerTurn(/* Put here the button the player pressed and ref to button pressed*/);
                    break;
                case BattleState.ENEMY:
                    Debug.Log("Enemy Turn ");
                    startTurn = false;
                    yield return new WaitForSeconds(1);
                    EnemyTurn();
                    break;
                case BattleState.ESCAPE:
                    Escaping();
                    break;
            }
        }
        else
        {
            bool outcome = Victory();
            // changes the scene here depending on outcome of battle
            if (outcome == true)
            {
                Debug.Log("WON!!!");
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
            enemyHealth = EnemyParty[0].transform.GetChild(0).GetChild(0).GetComponent<Slider>();
            eName = enemy1.chName;
            eMaxHealth = enemy1.maxHealth;
            eCurrentHealth = eMaxHealth;
            eAttack = enemy1.attack;
        }

    }

    private void SkillSetUp()
    {
        var currentInventory = playerInventory.GetInventory();
        //Vector3 pos = menu.transform.GetChild(3).GetChild(0).transform.position;

        foreach(Transform child in menu.transform.GetChild(3).transform)
        {
            foreach (InventorySlot item in currentInventory)
            {
                if(child.gameObject.name == item.skill.skillName)
                {
                    child.gameObject.SetActive(true);
                    //pos.y -= 46;
                    
                }
                
            }
        }


    }

    public void GetActionName(string action)
    {
        buttonPressed = action;
    }

    //Player Turn Execution
    private void PlayerTurn(/*var for ref to player object and var for ref to button pressed*/)
    {

        // Hard code some if/case statements for each button
        startTurn = false; // this should take the menu off the screen
        onClickBool = false;        //Resets button
        Debug.Log("Executing Player Actions");

        if(buttonPressed == "FireBall")
        {
            Debug.Log("Fire Ball");
        }
        if(buttonPressed == "LightningBolt")
        {
            Debug.Log("Lightning Bolt");
        }

        // if(buttonPressed == gun)

        DealDamage();

        state = BattleState.ENEMY;
        StartCoroutine(TurnBasedBattle());
    }

    //Enemy Turn Execution
    private void EnemyTurn(/* var for ref to enemy maybe*/)
    {
        Debug.Log("Execute Enemy Turn Actions");

        TakeDamage();

        //Iterate turn number and display
        turnNumber++;
        turnNumberText.text = "Turn:" + turnNumber.ToString();

        state = BattleState.PLAYER;
        StartCoroutine(TurnBasedBattle());
    }

    //On button click do this
    public void TurnExecution()  //Turn confirmation
    {
        onClickBool = true;
        Debug.Log(onClickBool);
    }

    //Deal damage to enemy
    public void DealDamage(/*GameObject player, GameObject enemy*/)
    {
        if (eAlive == true)
        {
            eCurrentHealth -= rAttack;
            enemyHealth.value = eCurrentHealth/ eMaxHealth;
        }
        else
        {
            eAlive = false;
            StartCoroutine(TurnBasedBattle());
        }
            
    }

    //Deal Damage to player
    public void TakeDamage()
    {
        if (rAlive == true)
        {
            rCurrentHealth -= eAttack;
            playerHealth.value = rCurrentHealth/rMaxHealth;
        }
        else
        {
            rAlive = false;
            StartCoroutine(TurnBasedBattle());
        }
    }


    void Escaping()
    {
        int escapeValue = 1;
        int randomValue = Random.Range(1, 11);

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

    private bool IsDefeated()
    {
        if (rCurrentHealth > 0 && eCurrentHealth > 0)
        {
            rAlive = true;
            eAlive = true;
            return false;
        }
        else
        {
            Debug.Log("Battle Ended");
            rAlive = false;
            eAlive = false;
            return true;
        }
    }

    private bool Victory()
    {
        if (rCurrentHealth >  0)
        {
            rAlive = true;
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
