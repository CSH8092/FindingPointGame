using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalManager : MonoBehaviour
{
    public GameObject object_Modal;

    public TextMeshProUGUI text_title;
    public TextMeshProUGUI text_content;
    public Button[] button_objectList;
    public TextMeshProUGUI[] button_textList;

    ModalManager() { }

    private static ModalManager instance = null;
    public static ModalManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ModalManager();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void ShowModal(string title, string content, ButtonSet[] buttons)
    {
        string re_title = Localization.GetStringByKey(title);
        string re_content = Localization.GetStringByKey(content);

        text_title.text = re_title;
        text_content.text = re_content;

        for(int i = 0; i < button_objectList.Length; i++)
        {
            button_objectList[i].onClick.RemoveAllListeners();
            button_objectList[i].gameObject.SetActive(false);
        }

        if (buttons != null)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                button_textList[i].text = Localization.GetStringByString(buttons[i].comment);

                OnEvent e = buttons[i].onClick;
                if (e != null)
                {
                    button_objectList[i].onClick.AddListener(delegate { e(); });
                }
                button_objectList[i].onClick.AddListener(delegate { HidePopUp(); });
                button_objectList[i].gameObject.SetActive(true);
            }
        }

        ShowPopUp();
    }

    void ShowPopUp()
    {
        object_Modal.SetActive(true);
    }

    void HidePopUp()
    {
        object_Modal.SetActive(false);
    }

    public delegate void OnEvent();
    public class ButtonSet
    {
        public string comment;
        public OnEvent onClick;

        public ButtonSet(string c, OnEvent e)
        {
            comment = c;
            onClick = e;
        }
    }
}
