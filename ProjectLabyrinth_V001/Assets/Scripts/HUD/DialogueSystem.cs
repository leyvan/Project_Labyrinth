using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

	public Text nameText;
	public Text dialogueText;
	public Canvas dialogueCanvas;
	public Image charImageFrame;
	
	private GameObject player;
	private Transform playerCamTarget;
	private Cinemachine.CinemachineFreeLook freeLookCam;
	private int resetLens = 40;
	//public Animator animator;

	public bool inDialogue;
	public string currentChar;
	private Sprite currentCharImage;

	private Queue<string> sentences;
	private List<string> characters;
	private List<Sprite> charImages;
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
		charImages = new List<Sprite>();
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
		charImages.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		for (int i = 0; i < dialogue.characterNames.Length; i++)
        {
			characters.Add(dialogue.characterNames[i]);
			charImages.Add(dialogue.characterPortraits[i]);
        }

		currentChar = characters[1];
		currentCharImage = charImages[1];
		
		nameText.text = currentChar;
		charImageFrame.sprite = currentCharImage;
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
			currentCharImage = charImages[1];

		} else{
			currentChar = characters[0];
			currentCharImage= charImages[0];
		}

		
		if(sentence != "")
        {
			nameText.text = currentChar;
			charImageFrame.sprite = currentCharImage;
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