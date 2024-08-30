using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnderConstruction : MonoBehaviour
{
    private Button returnToMenuButton;
    // Start is called before the first frame update
    private void Awake()
    {
        returnToMenuButton = this.transform.GetChild(0).GetChild(0).GetComponent<Button>();
    }
    void Start()
    {
        returnToMenuButton.onClick.AddListener(() => ReturnToMainMenu());
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
