using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//7
using UnityEngine.AI;

public class AI_Behaviour_V6 : MonoBehaviour
{
    public Transform player;
    public GameObject prefab;
    //1
    private Transform patrolRoute;
    //2
    public List<Transform> locations;

    //8
    private int locationIndex = 0;
    //9
    private NavMeshAgent agent;


    void Start()
    {
        //10
        agent = GetComponent<NavMeshAgent>();

        patrolRoute = prefab.transform.Find("PatrolPath");
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


    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            agent.destination = player.position;
            Debug.Log("Enemy detected!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Enemy out of range.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            agent.isStopped = true;
            Debug.Log("Critical hit!");
        }
    }
    */
}