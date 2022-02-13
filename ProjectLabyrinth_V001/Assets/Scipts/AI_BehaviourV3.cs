using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_BehaviourV3 : MonoBehaviour
{
    public float radius = 8f;
    public float speed = 1f;
    public Vector3 offset;

    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
                (radius * Mathf.Sin(Time.time * speed)) + offset.x,
                +offset.y,
                (radius * Mathf.Sin(Time.time * speed)) + offset.z);
    }
}
