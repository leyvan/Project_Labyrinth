using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PickUp : MonoBehaviour
{
    public bool playerInBounds;
    private GameObject player;

    void Update()
    {
        if (playerInBounds == true && Input.GetKey(KeyCode.E))
        {
            PickThisItemUp();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.gameObject;
            playerInBounds = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            playerInBounds = false;
        }
    }

    private void PickThisItemUp()
    {
        var prefab = Resources.Load("Prefabs/Items/" + this.gameObject.name)as GameObject;
        //player.GetComponent<Player_Behaviour>().StoreObjectInInventory(prefab);
        Destroy(this.gameObject);
    }
}
