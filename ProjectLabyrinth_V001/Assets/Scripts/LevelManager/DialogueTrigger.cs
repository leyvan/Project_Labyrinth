using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;
    public GameObject vcam;
    private DialogueSystem dialgSystem;
    private int lens = 55;

    public bool spokeWith = false;

    void Awake()
    {
        vcam = transform.GetChild(0).Find("NPCCamera").gameObject;
        dialgSystem = FindObjectOfType<DialogueSystem>();
    }
    public void Interact()
    {
        TriggerDialogue();
        GameEvents.current.TriggerDialogue();
    }
    
    public void TriggerDialogue()
    {
        spokeWith = true;
        vcam.GetComponent<CinemachineVirtualCamera>().enabled = true;
        dialgSystem.StartDialogue(dialogue, vcam);
    }
}
