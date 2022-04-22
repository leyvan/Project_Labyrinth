using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Button startButton;
    private Button optionsButton;
    private Button quitButton;

    private void Awake()
    {
        startButton = this.transform.GetChild(1).GetComponent<Button>();
        optionsButton = this.transform.GetChild(2).GetComponent<Button>();
        quitButton = this.transform.GetChild(3).GetComponent<Button>();
    }

    private void Start()
    {
        startButton.onClick.AddListener(() => StartGame());
        optionsButton.onClick.AddListener(() => Options());
        quitButton.onClick.AddListener(() => QuitGame());
    }

    void StartGame()
    {
        SceneManager.LoadScene("StartLevel");
    }

    void Options()
    {
        Debug.Log("Show Options");
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
