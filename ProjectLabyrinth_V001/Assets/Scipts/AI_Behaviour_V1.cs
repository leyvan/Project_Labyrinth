using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//7
using UnityEngine.AI;

public class AI_Behaviour_V1 : MonoBehaviour
{
    private Transform player;

    public GameObject alert;  //Red Alert Text 
    //1
    public Transform patrolRoute;
    //2
    public List<Transform> locations;

    public PartyListScriptableObject partyList;

    private GameObject thisEnemy;

    private Rigidbody rb;
    private Animator _animator;

    //8
    private int locationIndex = 0;
    //9
    private NavMeshAgent agent;
    private int _lives = 3;
    public bool cantMove;

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

        //10

        rb = this.GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        _animator = this.transform.GetChild(0).GetComponent<Animator>();

        if(agent != null)
        {
            patrolRoute = this.gameObject.transform.parent.parent.transform;
        }
        else
        {
            cantMove = true;
        }

        

        player = GameObject.FindGameObjectWithTag("Player").transform;

        //3
        if (patrolRoute != null)
        {

            InitializePatrolRoute();
            //11
            MoveToNextPatrolLocation();
        }

        GetThisEnemy();

    }

    void Update()
    {
        if (cantMove == true)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            _animator.SetBool("walking?", false);
            return;
        }
        //13
        if(patrolRoute != null)
        {
            if (agent.remainingDistance < 0.2f && !agent.pathPending)
            {
                //14
                MoveToNextPatrolLocation();
            }
        }


        if (agent.acceleration > 0.1f)
        {

            _animator.SetBool("running?", false);
            _animator.SetBool("walking?", true);
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
            agent.destination = player.position;
            Debug.Log("Enemy detected!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            locations.Clear();
            InitializePatrolRoute();
            alert.SetActive(false);
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