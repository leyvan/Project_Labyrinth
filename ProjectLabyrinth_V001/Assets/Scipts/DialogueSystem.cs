using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

	public Text nameText;
	public Text dialogueText;
	public Canvas dialogueCanvas;
	private GameObject player;
	private Transform playerCamTarget;
	private Cinemachine.CinemachineFreeLook freeLookCam;
	private int resetLens = 40;
	//public Animator animator;

	public bool inDialogue;

	private Queue<string> sentences;

    // Use this for initialization

    void Awake()
    {
		player = GameObject.FindGameObjectWithTag("Player");
		playerCamTarget = player.transform.GetChild(1).transform;
		freeLookCam = GameObject.FindGameObjectWithTag("ThirdPersonCam").GetComponent<Cinemachine.CinemachineFreeLook>();
    }
    void Start()
	{
		sentences = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue)
	{
		//animator.SetBool("IsOpen", true);
		Cursor.visible = true;

		dialogueCanvas.gameObject.SetActive(true);
		player.GetComponent<Player_Behaviour>().canMove = false;

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		//animator.SetBool("IsOpen", false);
		freeLookCam.m_Lens.FieldOfView = resetLens;
		freeLookCam.m_LookAt = playerCamTarget;
		Cursor.visible = false;
		player.GetComponent<Player_Behaviour>().canMove = true;
		dialogueCanvas.gameObject.SetActive(false);
	}

}