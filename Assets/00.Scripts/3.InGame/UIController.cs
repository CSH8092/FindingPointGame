using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject[] object_pass;

    [Header("Slider")]
    public Slider slider_answer;
    public Slider slider_wrong;

    [Header("Button")]
    public Button button_BackToLobby;

    [Header("Toggle")]
    public Toggle[] toggle_list;

    [Header("Text")]
    public TextMeshProUGUI text_sliderCount;
    public TextMeshProUGUI text_sliderWrong;
    public TextMeshProUGUI[] text_list;

    // Values
    int count_MaxA;
    int count_MaxW;

    void Start()
    {
        button_BackToLobby.onClick.AddListener(() => BackToLobby());

        slider_answer.onValueChanged.AddListener(ChangeValue_A);
        slider_wrong.onValueChanged.AddListener(ChangeValue_W);

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
    }

    public void InitToggleCheck()
    {
        for (int i = 0; i < toggle_list.Length; i++)
        {
            toggle_list[i].isOn = false;
        }
    }

    public int[] GetToggleCheck()
    {
        int[] list_toggleCheck = new int[5];
        for (int i = 0; i < toggle_list.Length; i++)
        {
            list_toggleCheck[i] = toggle_list[i].isOn ? 1 : 0;
        }
        return list_toggleCheck;
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
        for (int i = 0; i < text_list.Length; i++)
        {
            string_read = Localization.GetStringByKey(stringkey.ToString(),"[error]");
            text_list[i].text = string_read;
            stringkey++;
        }
    }
}
