using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField]
    private GameObject spawnPoints;

    void Awake()
    {
        Instance = this;
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



}

