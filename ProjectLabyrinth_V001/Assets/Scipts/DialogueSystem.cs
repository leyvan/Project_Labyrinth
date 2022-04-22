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
	public string currentChar;

	private Queue<string> sentences;
	private List<string> characters;
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
		characters = new List<string>();
	}

	public void StartDialogue(Dialogue dialogue)
	{
		//animator.SetBool("IsOpen", true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		dialogueCanvas.gameObject.SetActive(true);
		nameText = dialogueCanvas.GetComponentInChildren<Text>();
		player.GetComponent<Player_Behaviour>().canMove = false;



		sentences.Clear();
		characters.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		foreach(string character in dialogue.characterNames)
        {
			characters.Add(character);
        }

		currentChar = characters[1];
		nameText.text = currentChar;
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
		 
		if(currentChar == characters[0]){
			currentChar = characters[1];

		} else{
			currentChar = characters[0];
		}

		
		if(sentence != "")
        {
			nameText.text = currentChar;
			StartCoroutine(TypeSentence(sentence));
		}
		else
        {
			characters.Reverse();
			DisplayNextSentence();
        }
		
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
		Cursor.lockState = CursorLockMode.None;

		player.GetComponent<Player_Behaviour>().canMove = true;
		dialogueCanvas.gameObject.SetActive(false);
	}

}