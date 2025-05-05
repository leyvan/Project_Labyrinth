using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public enum MenuState
{
    START,
    RUFIO,
    ALLY1,
}

public class Menu : MonoBehaviour
{
    [FormerlySerializedAs("bc")] public BattleController battleController;
    public TextMeshProUGUI text;
    public MenuState menuState;

    public GameObject basicMenu, rufioMenu, confirmation;

    public GameObject basicMenuFirst, rufioMenuFirst, confirmMenuFirst;

    private bool loop = true;

    void Start()
    {
        BattleEvents.current.onBattleMenuToggle += MenuToggle;
        
        menuState = MenuState.START;
    }

    // START TURN -------------------------------------------------

    public void MenuToggle(bool isActive)
    {
        if (isActive)
        {
            menuState = MenuState.START;
            text.gameObject.SetActive(isActive);
            text.SetText("Menu");
            basicMenu.SetActive(isActive);
            //rufioMenu.SetActive(false);
            //confirmation.SetActive(false);
        }

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected game object
        EventSystem.current.SetSelectedGameObject(basicMenuFirst);
    }

    // RUFIO TURN --------------------------------------------------

    public void RufioMenu()
    {
        menuState = MenuState.RUFIO;
        text.SetText("Fight");
        basicMenu.SetActive(false);
        rufioMenu.SetActive(true);
        confirmation.SetActive(false);

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected game object
        EventSystem.current.SetSelectedGameObject(rufioMenuFirst);
    }

    // ALLY TURN ---------------------------------------------------

    public void AllyMenu()
    {

    }

    // SKILL MENU --------------------------------------------------

    public void SkillMenu()
    {

    }

    // ITEM MENU ---------------------------------------------------

    public void ItemMenu()
    {

    }

    // TURN CONFIRMATION -------------------------------------------

    public void ConfirmMenuOn()
    {
        text.SetText("All Set?");
        basicMenu.SetActive(false);
        rufioMenu.SetActive(false);
        confirmation.SetActive(true);

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected game object
        EventSystem.current.SetSelectedGameObject(confirmMenuFirst);
    }

    // When "No" is selected
    public void ConfirmMenuNo()
    {
        // PLAN: detect the number of party members, and go back to either an ally menu or rufio's menu
        switch (menuState)
        {
            case MenuState.START:
                menuState = MenuState.START;
                text.gameObject.SetActive(true);
                text.SetText("Menu");
                basicMenu.SetActive(true);
                rufioMenu.SetActive(false);
                confirmation.SetActive(false);

                //clear selected object
                EventSystem.current.SetSelectedGameObject(null);
                //set a new selected game object
                EventSystem.current.SetSelectedGameObject(basicMenuFirst);
                break;
            case MenuState.RUFIO:
                text.SetText("Fight");
                basicMenu.SetActive(false);
                rufioMenu.SetActive(true);
                confirmation.SetActive(false);

                //clear selected object
                EventSystem.current.SetSelectedGameObject(null);
                //set a new selected game object
                EventSystem.current.SetSelectedGameObject(rufioMenuFirst);
                break;
            case MenuState.ALLY1:
                break;
        }
    }

    // When "Yes" is selected, or escape fails
    public void ConfirmMenuOff()
    {
        text.gameObject.SetActive(false);
        basicMenu.SetActive(false);
        rufioMenu.SetActive(false);
        confirmation.SetActive(false);
        //loop = true;
        battleController.TurnExecution();

    }
}
