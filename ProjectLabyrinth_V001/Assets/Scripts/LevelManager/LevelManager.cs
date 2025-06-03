using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerHUD playerHUD;
    private Player_Behaviour playerBehaviour;
    private HealthManager _healthManager;

    public NavMeshSurface navMesh;

    public bool inBattle;

    private GameObject temp;
    private GameObject mainParent;

    void Awake()
    {
        Instance = this;

        FindObjects();
        mainParent = GameObject.FindGameObjectWithTag("Parent");
    }

    void Start()
    {
        GameEvents.current.onLevelEnvironmentLoaded += OnAssetsReady;
        
        Cursor.lockState = CursorLockMode.Confined;     //Confines the cursor to game screen
        if (SceneManager.GetActiveScene().name == "StartLevel")
        {
            inBattle = false;
            player.GetComponent<PlayerData>().SetPlayerLevel(1);
        }

        if (inBattle)
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
        }
        
    }

    private void OnAssetsReady()
    {
        if (navMesh == null)
        {
            navMesh = GameObject.FindGameObjectWithTag("NavMeshSurface").GetComponent<NavMeshSurface>();
            navMesh.BuildNavMesh();
        }
        
        Debug.Log("ASSETS LOADED AND NAVMESH FOUND =======================");
        GameEvents.current.BuiltNavmesh();
    }

    void FindObjects()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<PlayerHUD>();
        playerBehaviour = player.GetComponent<Player_Behaviour>();
    }

    void StartBattle()
    {
        player.GetComponent<Player_Behaviour>().SetControllerMode("Battle");
        playerHUD.TogglePlayerHUD(Player_Behaviour.ControllerMode.BattleMode);

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


    void OnLevelWasLoaded(int level)
    {
        ReloadInventory();
    }
    
    private void ReloadInventory()
    {
        if (SceneManager.GetActiveScene().name != "Battle")
        {
            playerHUD.GetDisplayInventory().ReloadInventoryOnLoad();
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
        
        GameEvents.current.TriggerBattleEnded();
        
        AsyncOperation async = SceneManager.UnloadSceneAsync("Battle");
        while (!async.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        
        inBattle = false;
        Destroy(temp);
    }
    

}

