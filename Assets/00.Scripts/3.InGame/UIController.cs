using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Button")]
    public Button button_BackToLobby;

    [Header("Toggle")]
    public Toggle[] toggle_list;

    [Header("Text")]
    public TextMeshProUGUI[] text_list;

    void Start()
    {
        button_BackToLobby.onClick.AddListener(() => BackToLobby());

        SetStageMessage();
    }


    void Update()
    {
        
    }

    void BackToLobby()
    {
        SceneLoader.Instance.LoadSceneByName("02.Lobby");
    }

    void SetStageMessage()
    {
        //SingletonCom.Instance.curr_StageNum
        for (int i = 0; i < text_list.Length; i++)
        {
            text_list[i].text = "message sample " + i;
        }
    }

    public void InitToggleCheck()
    {
        for (int i = 0; i < toggle_list.Length; i++)
        {
            toggle_list[i].isOn = false;
        }
    }
}
