using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerHUD playerHUD;

    private NavMeshSurface navMesh;

    [SerializeField]
    private GameObject spawnPoints;

    public bool inBattle;

    private GameObject temp;
    private GameObject mainParent;

    void Awake()
    {
        Instance = this;

        FindObjects();
        navMesh = GameObject.FindObjectOfType<NavMeshSurface>();
        mainParent = GameObject.FindGameObjectWithTag("Parent");


    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;     //Confines the cursor to game screen
        if (SceneManager.GetActiveScene().name == "StartLevel")
        {
            inBattle = false;
        }

        if (inBattle == true)
        {
            Cursor.visible = true;
            player.GetComponent<Player_Behaviour>().SetControllerMode("Battle");
            playerHUD.TogglePlayerHUD(Player_Behaviour.ControllerMode.BattleMode);

            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            Cursor.visible = false;

            player.GetComponent<Player_Behaviour>().SetControllerMode("Level01");
            playerHUD.TogglePlayerHUD(Player_Behaviour.ControllerMode.OverWorldMode);

            if (navMesh == null) return;
            navMesh.BuildNavMesh();
        }



    }

    void FindObjects()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        playerHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<PlayerHUD>();
    }

    void StartBattle()
    {
        player.GetComponent<Player_Behaviour>().SetControllerMode("Battle");
        playerHUD.TogglePlayerHUD(Player_Behaviour.ControllerMode.BattleMode);

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void OnLevelWasLoaded(int level)
    {
        ReloadInventory();
    }

    private void ReloadInventory()
    {
        if (SceneManager.GetActiveScene().name != "Battle")
        {
            player.transform.GetChild(3).GetChild(0).GetChild(2).GetComponent<DisplayInventory>().ReloadInventoryOnLoad();
        }
    }


    public IEnumerator LoadBattleScene()
    {
        mainParent.SetActive(false);
        AsyncOperation async = SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);
        while (!async.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        Scene battleScene = SceneManager.GetSceneByName("Battle");
        GameObject[] allObjects = battleScene.GetRootGameObjects();

        foreach (GameObject go in allObjects)
        {
            go.transform.SetParent(temp.transform, false);

        }
 

        SceneManager.SetActiveScene(battleScene);

        temp.SetActive(true);
        inBattle = true;
        StartBattle();
    }

    
    public IEnumerator LoadLevelScene()
    {
        Scene levelScene = SceneManager.GetSceneByName("Level01");
        SceneManager.SetActiveScene(levelScene);
        mainParent.SetActive(true);


        AsyncOperation async = SceneManager.UnloadSceneAsync("Battle");
        while (!async.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        inBattle = false;
        Destroy(temp);
        
    }
    

}

