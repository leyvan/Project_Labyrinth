using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillCrystal : MonoBehaviour, IInteractable
{
    bool playerInBounds = false;
    bool showText;
    GameObject player;
    Camera cameraToLookTowards;

    
    Vector3 targetPos;

    private TextMeshProUGUI helpText;

    public BaseSkill skill;

    private void Awake()
    {
        helpText = this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        cameraToLookTowards = Camera.main;
    }

    void Update()
    {
        if(showText == true)
        {
            helpText.transform.LookAt(cameraToLookTowards.transform);
            helpText.transform.rotation = Quaternion.LookRotation(cameraToLookTowards.transform.forward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInBounds = true;
            player = other.gameObject;
            

            helpText.gameObject.SetActive(true);
            showText = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            showText = false;
            helpText.gameObject.SetActive(false);

            playerInBounds = false;
            player = null;
            targetPos = Vector3.zero;
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
        showText = false;
        helpText.gameObject.SetActive(false);

        player.GetComponent<Player_Behaviour>().inventory.fillInventory(skill, 1);
        player.transform.GetChild(3).GetChild(1).GetChild(2).GetComponent<DisplayInventory>().UpdateInventorySlot(skill);
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
