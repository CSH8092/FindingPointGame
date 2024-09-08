using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [Header("Buttons")]
    public Button button_Start;
    public Button button_Exit;

    void Start()
    {
        button_Start.onClick.AddListener(() => ClickStartButton());
        button_Exit.onClick.AddListener(() => ClickExitButton());
    }

    void Update()
    {
        
    }

    void ClickStartButton()
    {
        SceneLoader.Instance.LoadSceneByName("02.Lobby");
    }

    void ClickExitButton()
    {
        Application.Quit();
    }
}
