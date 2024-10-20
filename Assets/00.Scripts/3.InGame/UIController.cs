using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject[] object_pass;
    public RectTransform rect_panelCheckList;

    [Header("Button")]
    public Button button_BackToLobby;
    public Button button_QuitToGame;

    [Header("Slider")]
    public Slider slider_answer;
    public Slider slider_wrong;

    [Header("Toggle")]
    public UI_Toggle[] toggle_list;

    [Header("Text")]
    public TextMeshProUGUI text_sliderCount;
    public TextMeshProUGUI text_sliderWrong;

    // Values
    int count_MaxA;
    int count_MaxW;

    void Start()
    {
        button_BackToLobby.onClick.AddListener(() => BackToLobby());
        button_QuitToGame.onClick.AddListener(() => QuitToGame());

        slider_answer.onValueChanged.AddListener(ChangeValue_A);
        slider_wrong.onValueChanged.AddListener(ChangeValue_W);

        for (int i = 0; i < toggle_list.Length; i++)
        {
            int index = i;
            toggle_list[i].toggle.onValueChanged.AddListener((x) => SetToggleUIState(index, x));
        }
        toggle_list[0].toggle.onValueChanged.AddListener((x) => CheckNoProblemToggle(x));

        SetStageMessage();
    }


    void Update()
    {
        
    }

    public void InitUI(int maxval_a, int maxval_w)
    {
        SetSlider_A_Init(maxval_a);
        SetSlider_W_Init(maxval_w);

        for(int i=0;i< object_pass.Length; i++)
        {
            object_pass[i].SetActive(false);
        }

        InitToggleCheck();

        ShowHidePanelCheckList(true);
    }

    Vector2 panel_on = new Vector2(-300, -60);
    Vector2 panel_off = new Vector2(150, -60);
    void ShowHidePanelCheckList(bool isOn)
    {
        if (isOn)
        {
            rect_panelCheckList.DOAnchorPos(panel_on, 2f);
        }
        else
        {
            rect_panelCheckList.DOAnchorPos(panel_off, 2f);
        }
    }

    void CheckNoProblemToggle(bool isOn)
    {
        Toggle target;
        for (int i = 1; i < toggle_list.Length; i++)
        {
            target = toggle_list[i].toggle;
            target.isOn = false;

            toggle_list[i].SetInteractable(!isOn);
        }
    }

    void SetToggleUIState(int toggleIndex, bool isOn)
    {
        Toggle target = toggle_list[toggleIndex].toggle;

        if(target.TryGetComponent<UI_Toggle>(out UI_Toggle toggle))
        {
            toggle.SetOnOff(isOn);
        }
    }

    public void InitToggleCheck()
    {
        for (int i = 0; i < toggle_list.Length; i++)
        {
            toggle_list[i].toggle.isOn = false;
        }
    }

    public int[] GetToggleCheck()
    {
        // Index 0 : No Problem
        int[] list_toggleCheck = new int[toggle_list.Length - 1];

        for (int i = 1; i < toggle_list.Length; i++)
        {
            list_toggleCheck[i - 1] = toggle_list[i].toggle.isOn ? 1 : 0;
        }
        return list_toggleCheck;
    }

    public void CheckNoProblemState()
    {
        bool result = true;
        for (int i = 0; i < toggle_list.Length; i++)
        {
            if (toggle_list[i].toggle.isOn)
            {
                result = false;
            }
        }

        if (result)
        {
            // 사용자가 아무것도 체크하지 않고, 제출 버튼 누른 상태
            toggle_list[0].toggle.isOn = true;
        }
    }

    public bool AddCountPass()
    {
        bool isFull = false;
        for(int i=0;i< object_pass.Length; i++)
        {
            if (!object_pass[i].activeSelf)
            {
                object_pass[i].gameObject.SetActive(true);
                return true;
            }
        }
        return isFull;
    }

    public bool AddCountSlider_A()
    {
        float newVal = slider_answer.value + 1;
        slider_answer.value = newVal;

        if (newVal >= count_MaxA)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddCountSlider_W()
    {
        float newVal = slider_wrong.value + 1;
        slider_wrong.value = newVal;

        if (newVal >= count_MaxW)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SetSlider_A_Init(int maxval)
    {
        slider_answer.maxValue = maxval;
        slider_answer.value = 0;
        count_MaxA = maxval;

        text_sliderCount.text = string.Format("{0}/{1}", slider_answer.value, count_MaxA);
    }

    void SetSlider_W_Init(int maxval)
    {
        slider_wrong.maxValue = maxval;
        slider_wrong.value = 0;
        count_MaxW = maxval;

        text_sliderWrong.text = string.Format("{0}/{1}", slider_answer.value, count_MaxW);
    }

    void ChangeValue_A(float value)
    {
        text_sliderCount.text = string.Format("{0}/{1}", value, count_MaxA);
    }

    void ChangeValue_W(float value)
    {
        text_sliderWrong.text = string.Format("{0}/{1}", value, count_MaxW);
    }

    void BackToLobby()
    {
        SceneLoader.Instance.LoadSceneByName("02.Lobby");
    }

    void QuitToGame()
    {
        Debug.Log("Quit Game, InGame");
        Application.Quit();
    }

    void SetStageMessage()
    {
        Factory.ObjectType type = (Factory.ObjectType)SingletonCom.Instance.curr_StageNum;

        int stringkey = 0;
        switch (type)
        {
            case Factory.ObjectType.pudding:
                stringkey = 13;
                break;
            case Factory.ObjectType.donut:
                stringkey = 18;
                break;
            case Factory.ObjectType.pencil:
                stringkey = 23;
                break;
            case Factory.ObjectType.berry:
                stringkey = 28;
                break;
            default:
                return;
        }

        string string_read = "";
        string_read = Localization.GetStringByKey(50.ToString(), "No problem");
        toggle_list[0].toggleText.text = string_read;

        for (int i = 1; i < toggle_list.Length; i++)
        {
            string_read = Localization.GetStringByKey(stringkey.ToString(),"[error]");
            toggle_list[i].toggleText.text = string_read;
            stringkey++;
        }
    }
}
