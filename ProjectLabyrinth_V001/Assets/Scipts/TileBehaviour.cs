using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Corridor")
        {
            Debug.Log("Trigger Activated");
            this.transform.position = Vector3.forward * this.transform.localScale.z;
        }
    }
}
