using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPopUp : MonoBehaviour
{

	private TextMeshProUGUI helpText;
	private GameObject helpCanvas;
	private GameObject player;
	
	private Image example;

	private bool alreadySeen;
	public bool onItemPickUp;

	private GameObject item;

	public Tutorials thisTutorial;

	private Queue<string> sentenceQueue;

	// Use this for initialization

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		helpCanvas = GameObject.FindGameObjectWithTag("Help");

		item = this.transform.GetChild(0).gameObject;
		if(item != null)
        {
			onItemPickUp = true;
		}
	}

    private void Update()
    {
        if(onItemPickUp == true)
        {
			if(item == null)
            {
				onItemPickUp = false;
				DisplayHelpText(thisTutorial);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
			if(alreadySeen == false)
			DisplayHelpText(thisTutorial);
        }
    }

    public void DisplayHelpText(Tutorials tutorial)
	{
		helpText = helpCanvas.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
		example = helpCanvas.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();

		sentenceQueue = new Queue<string>();

		alreadySeen = true;

		helpCanvas.transform.GetChild(0).gameObject.SetActive(true);
		example.sprite = tutorial.providedExample;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		player.GetComponent<Player_Behaviour>().canMove = false;

		sentenceQueue.Clear();

		foreach (string sentence in tutorial.sentences)
		{
			sentenceQueue.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentenceQueue.Count == 0)
		{
			return;
		}

		string sentence = sentenceQueue.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}
	


	IEnumerator TypeSentence(string sentence)
	{
		helpText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			helpText.text += letter;
			yield return null;
		}
	}

	public void EndDialogue()
	{
		//animator.SetBool("IsOpen", false);

		player.GetComponent<Player_Behaviour>().canMove = true;
		helpCanvas.transform.GetChild(0).gameObject.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
