using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTPEvent : MonoBehaviour
{
    private DialogueTrigger thisNPCtrigger;
    private GameObject teleporter;
    private void Awake()
    {
        thisNPCtrigger = gameObject.GetComponent<DialogueTrigger>();
        teleporter = GameObject.FindGameObjectWithTag("Level1");

    }

    private void Update()
    {
        if (teleporter.transform.GetChild(0).gameObject.activeSelf == true) return;
        if(thisNPCtrigger.spokeWith == true)
        {
            teleporter.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
