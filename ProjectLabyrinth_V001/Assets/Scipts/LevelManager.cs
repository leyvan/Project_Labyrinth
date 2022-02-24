using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private GameObject player;

    [SerializeField]
    private GameObject spawnPoints;

    void Awake()
    {
        Instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;     //Confines the cursor to game screen
        if(SceneManager.GetActiveScene().name == "Battle")
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
            
        }


    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != "Battle")
        {
            player.transform.GetChild(3).GetChild(0).GetChild(2).GetComponent<DisplayInventory>().ReloadInventoryOnLoad();
        }

        
    }

}

