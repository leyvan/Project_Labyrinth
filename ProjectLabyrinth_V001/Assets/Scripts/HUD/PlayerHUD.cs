using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    private Player_Behaviour playerScript;
    private GameObject playerOverworldHUD;
    private GameObject inventoryHUD;
    
    [SerializeField] private DisplayInventory displayInventory; 
    // Start is called before the first frame update
    void Awake()
    {
        playerScript = this.transform.parent.GetComponent<Player_Behaviour>();
        playerOverworldHUD = this.transform.GetChild(0).gameObject;
        inventoryHUD = this.transform.GetChild(1).gameObject;
    }

    void Start()
    {
        TogglePlayerHUD(playerScript.currentMode);
    }

    public void ToggleInventory()
    {
        inventoryHUD.SetActive(!inventoryHUD.activeSelf);
    }

    public void TogglePlayerHUD(Player_Behaviour.ControllerMode mode)
    {
        if (mode == Player_Behaviour.ControllerMode.BattleMode)
        {
            playerOverworldHUD.SetActive(false);
        }
        else
        {
            playerOverworldHUD.SetActive(true);
        }
    }

    public DisplayInventory GetDisplayInventory()
    {
        return displayInventory;
    }

}
