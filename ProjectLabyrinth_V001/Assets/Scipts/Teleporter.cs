using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour, IInteractable
{
	bool playerInBounds = false;

	GameObject player;
	string destination;

	[SerializeField]
	private string objectTag;

	void Awake()
    {
		SetDestination();
    }
	void Update()
	{
		// Rotate the game object that this script is attached to by 15 in the X axis,
		// 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
		// rather than per frame.
		transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
	}

	public void Interact()
    {
		Teleport();
    }

	public string SetDestination()
    {
		objectTag = this.transform.parent.tag;
		switch (objectTag)
        {
			case "Level1":
				destination = "Start";
				break;
			case "Tp1":
				destination = "Tp2";
				break;
			case "Tp3":
				destination = "Tp4";
				break;
			case "Tp5":
				destination = "Boss";
				break;
			default:
				break;
		}

		return destination;

    }

	void Teleport()
    {
		if(playerInBounds == false || player == false || destination == null) return;

		if(destination == "Boss")
        {
			SceneManager.LoadScene("BossBattle");
        }
		else if(destination == "Start")
        {
			SceneManager.LoadScene("Level01");
		}
		else
        {
			var destinationPos = GameObject.FindGameObjectWithTag(destination).transform.position;
			var offset = new Vector3(3, 0, 0);
			player.transform.position = destinationPos + offset;
		}

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
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
}
