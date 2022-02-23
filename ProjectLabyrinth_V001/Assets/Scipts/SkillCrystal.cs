using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCrystal : MonoBehaviour, IInteractable
{
    bool playerInBounds = false;
    GameObject player;


    public BaseSkill skill;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInBounds = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInBounds = false;
            player = null;
        }
    }

    public void Interact()
    {
        if(playerInBounds)
        { RemoveFromOverWorld();  }
        else 
        { return; }
    }

    public void RemoveFromOverWorld()
    {
        player.GetComponent<Player_Behaviour>().inventory.fillInventory(skill, 1);
        player.transform.GetChild(3).GetChild(0).GetChild(2).GetComponent<DisplayInventory>().AddInventorySlot();
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
