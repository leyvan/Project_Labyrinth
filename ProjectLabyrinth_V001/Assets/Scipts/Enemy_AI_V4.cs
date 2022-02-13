using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI_V4 : MonoBehaviour
{
    public float radius = 10f;
    public float speed = 1f;
    public Vector3 offset;
    public bool _Found = false;

    public GameObject player;

    void Start()
    {

        offset = transform.position;
      
    }

    void Update()
    {
        if (_Found == false)
        {
            transform.position = new Vector3(
                        (radius * Mathf.Cos(Time.time * speed)) + offset.x,
                        +offset.y,
                        (radius * Mathf.Sin(Time.time * speed)) + offset.z);
        }

        if (_Found == true)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _Found = true;
            Debug.Log("Target Found!");
        }
    }
}
