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

    //8
    private int locationIndex = 0;
    //9
    private NavMeshAgent agent;
    private int _lives = 3;

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
        agent = GetComponent<NavMeshAgent>();
        patrolRoute = this.gameObject.transform.parent.parent.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //3
        InitializePatrolRoute();
        //11
        MoveToNextPatrolLocation();
    }

    void Update()
    {
        //13
        if (agent.remainingDistance < 0.2f && !agent.pathPending)
        {
            //14
            MoveToNextPatrolLocation();
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
            SceneManager.LoadScene("Battle");

        }
    }
}