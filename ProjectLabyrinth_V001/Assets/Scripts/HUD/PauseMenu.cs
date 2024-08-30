using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private Button closeMenuButton;
    private TextMeshProUGUI menuTitle;

    private GameObject menuObjectContainer;
    
    private GameObject startWindow;
    private GameObject characterInfoWindow;
    private GameObject optionsWindow;

    private GameObject inventoryMenu;

    private GameObject partyLeaderDisplay;
    
    private List<Button> startWindowButtons = new List<Button>();
    private List<Button> characterInfoWindowButtons = new List<Button>();
    private List<Button> optionsWindowButtons = new List<Button>();

    private List<Button> inventoryButtons = new List<Button>();

    private static List<GameObject> inventorySlots;

    private GameObject lastOpenWindow;

    private Dictionary<string, int> buttonNameAsIndex = new Dictionary<string, int>()
    {
        {"CharacterMenuButton", 0},
        {"SettingsButton", 1},
        {"QuitButton", 2},
        {"Party", 0},
        {"InventoryButton", 1},
        {"SkillsAndEquipmentButton", 2},
        {"Audio", 0},
        {"Graphics", 1},
        {"Guide", 2},
        {"InventoryPrevious", 0},
        {"InventoryNext", 1},
        {"InventoryReturnToMenu", 2}
    };

    private void Awake()
    {
        closeMenuButton = transform.Find("CloseMenuButton").GetComponent<Button>();
        menuObjectContainer = transform.Find("SubMenus").gameObject;
        partyLeaderDisplay = transform.Find("PartyLeaderDisplay").gameObject;

        startWindow = menuObjectContainer.transform.Find("StartMenu").gameObject;
        characterInfoWindow = menuObjectContainer.transform.Find("CharacterMenu").gameObject;
        optionsWindow = menuObjectContainer.transform.Find("SettingsMenu").gameObject;

        startWindowButtons.AddRange(startWindow.transform.Find("ListOfButtons").GetComponentsInChildren<Button>());
        characterInfoWindowButtons.AddRange(characterInfoWindow.transform.Find("ListOfButtons").GetComponentsInChildren<Button>());
        optionsWindowButtons.AddRange(optionsWindow.transform.Find("ListOfButtons").GetComponentsInChildren<Button>());

        inventoryMenu = transform.Find("Inventory").gameObject;
        inventoryButtons.AddRange(inventoryMenu.transform.Find("InventoryButtons").GetComponentsInChildren<Button>());
    }

    private void Start()
    {
        startWindowButtons[buttonNameAsIndex["CharacterMenuButton"]].onClick.AddListener(DisplayCharacterWindow);
        startWindowButtons[buttonNameAsIndex["SettingsButton"]].onClick.AddListener(DisplaySettingsWindow);
        startWindowButtons[buttonNameAsIndex["QuitButton"]].onClick.AddListener(QuitGame);
        
        characterInfoWindowButtons[buttonNameAsIndex["InventoryButton"]].onClick.AddListener(DisplayInventoryMenu);
        
        inventoryButtons[buttonNameAsIndex["InventoryReturnToMenu"]].onClick.AddListener(HideInventoryMenu);
    }

    private void DisplayCharacterWindow()
    {
        if (lastOpenWindow != null) {
            lastOpenWindow.SetActive(false);
        }
        
        lastOpenWindow = characterInfoWindow;
        characterInfoWindow.SetActive(true);
        Debug.Log("Displaying Character Pop-Up Window");
    }
    
    private void DisplaySettingsWindow()
    {
        if (lastOpenWindow != null) {
            lastOpenWindow.SetActive(false);
        }
        
        lastOpenWindow = optionsWindow;
        optionsWindow.SetActive(true);
        Debug.Log("Displaying Settings Pop-Up Window");
    }
    
    private void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void DisplayCharacterMenu()
    {
    } 

    private void DisplayPartyMenu()
    {
    }

    private void DisplaySkillsAndEquipmentMenu()
    {
    }

    private void DisplayInventoryMenu()
    {
        startWindow.SetActive(false);
        lastOpenWindow.SetActive(false);
        partyLeaderDisplay.SetActive(false);
        
        inventoryMenu.SetActive(true);
    }

    private void HideInventoryMenu()
    {
        inventoryMenu.SetActive(false);
        startWindow.SetActive(true);
        lastOpenWindow.SetActive(true);
        partyLeaderDisplay.SetActive(true);
    }
    
    private void DisplayAudioMenu()
    {
    }

    private void DisplayGraphicsMenu()
    {
    }

    private void DisplayGuideMenu()
    {
    }
    
}
