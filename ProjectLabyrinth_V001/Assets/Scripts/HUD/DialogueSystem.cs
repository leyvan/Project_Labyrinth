using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

	public Text nameText;
	public Text dialogueText;
	public Canvas dialogueCanvas;
	public Image charImageFrame;
	
	private CinemachineVirtualCamera npcCamera;
	private int resetLens = 40;
	//public Animator animator;

	public bool inDialogue;
	public string currentChar;
	private Sprite currentCharImage;

	private Queue<string> sentences;
	private List<string> characters;
	private List<Sprite> charImages;
    // Use this for initialization
    
    void Start()
	{
		sentences = new Queue<string>();
		charImages = new List<Sprite>();
		characters = new List<string>();
	}

	public void StartDialogue(Dialogue dialogue, GameObject vcam)
	{
		npcCamera = vcam.GetComponent<CinemachineVirtualCamera>();
		//animator.SetBool("IsOpen", true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		dialogueCanvas.gameObject.SetActive(true);
		nameText = dialogueCanvas.GetComponentInChildren<Text>();

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
		npcCamera.enabled = false;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.None;

		GameEvents.current.DialogueEventEnded();
		dialogueCanvas.gameObject.SetActive(false);
	}

}