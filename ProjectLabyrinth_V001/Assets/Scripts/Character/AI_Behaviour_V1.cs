using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//7
using UnityEngine.AI;

public class AI_Behaviour_V1 : MonoBehaviour
{
    private Transform player;

    public GameObject alert;
    private bool chasePlayer;
    public Transform patrolRoute;
    public List<Transform> locations;
    public PartyListScriptableObject partyList;
    private GameObject thisEnemy;
    private Rigidbody rb;
    private Animator _animator;
    
    private int locationIndex = 0;
    private NavMeshAgent agent;
    private int _lives = 3;
    public bool cantMove;

    private bool agentIsActive;

    [SerializeField] public bool tutorialAgent;
    
    public int Lives
    {
        get { return _lives; }
        private set
        {
            _lives = value;

            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy down.");
            }
        }
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        _animator = this.transform.GetChild(0).GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        GetThisEnemy();
        
        _animator.SetBool("walking?", false);

        SetupPatrolRoute();
    }
    void Update()
    {
        if (!agentIsActive) return;
        if (cantMove) return;
        
        if(chasePlayer != true)
        {
            if (patrolRoute != null) {
                if (agent.remainingDistance < 0.2f && !agent.pathPending)
                {
                    MoveToNextPatrolLocation();
                }
            }
        }
        else
        {
            agent.destination = player.transform.position;
        }
        
        if (agent.acceleration > 0.1f)
        {
            _animator.SetBool("running?", false);
            _animator.SetBool("walking?", true);
        }
    }

    private void SetupPatrolRoute()
    {
        if (!tutorialAgent)
        {
            Debug.Log("THIS IS THE PARENT " + transform.parent.gameObject.name);
            patrolRoute = transform.parent.Find("PatrolRoute_1");
        } 
        
        
        if (agent == null || patrolRoute == null)
        {
            cantMove = true;
            Debug.Log("NO PATROL ROUTE OR NAV AGENT FOUND ======================");
        }
        else
        {
            InitializePatrolRoute();
            
        }
    }

    void GetThisEnemy()
    {
        var enemyName = this.gameObject.name.Replace("(Clone)", "");
        thisEnemy = Resources.Load("Prefabs/Misc/"+enemyName+" Battle") as GameObject;

        PopulateEnemyParty();
    }

    void PopulateEnemyParty()
    {
        //Get enemy type
        var rand = Random.Range(1, 4);
        for(var i = 0; i < rand; i++)
        {
            partyList.enemyParty.Add(thisEnemy);
        }
        
    }

    //4   
    void InitializePatrolRoute()
    {
        //5
        foreach (Transform child in patrolRoute)
        {
            //6
            locations.Add(child);
        }

        agentIsActive = true;
        
        Debug.Log("PATROL ROUTE INITIALIZED: ============================");
        MoveToNextPatrolLocation();
    }

    void MoveToNextPatrolLocation()
    {
        //15

        if (locations.Count == 0)
            return;

        //12
        agent.destination = locations[locationIndex].position;
        //16
        locationIndex = (locationIndex + 1) % locations.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            alert.SetActive(true);
            chasePlayer = true;
            Debug.Log("Enemy detected!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            locations.Clear();
            //InitializePatrolRoute();
            alert.SetActive(false);
            chasePlayer = false;
            Debug.Log("Enemy out of range.");
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Critical hit!");

            //LevelManager.reSpawnLocation = this.transform;
            StartCoroutine(LevelManager.Instance.LoadBattleScene());
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        if(partyList != null){
            partyList.enemyParty.Clear();
        }
        
    }
}