using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;
    public Cinemachine.CinemachineFreeLook freeLookCam;
    private DialogueSystem dialgSystem;
    private int lens = 55;

    public bool spokeWith = false;

    void Awake()
    {
        freeLookCam = GameObject.FindGameObjectWithTag("ThirdPersonCam").GetComponent<Cinemachine.CinemachineFreeLook>();
        dialgSystem = FindObjectOfType<DialogueSystem>();
    }
    public void Interact()
    {
        TriggerDialogue();
    }



    public void TriggerDialogue()
    {
        spokeWith = true;
        freeLookCam.m_Lens.FieldOfView = lens;
        freeLookCam.m_LookAt = this.transform;
        dialgSystem.StartDialogue(dialogue);
    }
}
