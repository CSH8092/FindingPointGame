using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public GameObject object_SettingMdoal;

    [Header("Language")]
    public Toggle toggle_kr;
    public Toggle toggle_en;
    UI_Toggle uiToggle_kr;
    UI_Toggle uiToggle_en;
    [Space(10)]
    public Localization[] list_targetLocalClass;

    [Header("Theme")]
    public Toggle toggle_dark;
    public Toggle toggle_light;
    UI_Toggle uiToggle_dark;
    UI_Toggle uiToggle_light;

    [Header("Button")]
    public Button button_Close;

    string key_language = "curr_language";
    string key_theme = "curr_theme";

    void Start()
    {
        button_Close.onClick.AddListener(() => ShowHideSettingModal(false));

        toggle_kr.onValueChanged.AddListener((x) => SetLanguageKR(x));
        toggle_en.onValueChanged.AddListener((x) => SetLanguageEN(x));
        toggle_dark.onValueChanged.AddListener((x) => SetThemeDark(x));
        toggle_light.onValueChanged.AddListener((x) => SetThemeLight(x));

        UI_Toggle toggle;
        if (toggle_kr.TryGetComponent<UI_Toggle>(out toggle))
        {
            uiToggle_kr = toggle;
        }
        if (toggle_en.TryGetComponent<UI_Toggle>(out toggle))
        {
            uiToggle_en = toggle;
        }
        if (toggle_dark.TryGetComponent<UI_Toggle>(out toggle))
        {
            uiToggle_dark = toggle;
        }
        if (toggle_light.TryGetComponent<UI_Toggle>(out toggle))
        {
            uiToggle_light = toggle;
        }

        // Init
        toggle_kr.isOn = false;
        toggle_en.isOn = false;
        toggle_dark.isOn = false;
        toggle_light.isOn = false;

        // Load
        LoadSetting();
    }

    void Update()
    {
        
    }

    void SaveSettingLangauge()
    {
        PlayerPrefs.SetInt(key_language, (int)SingletonCom.curr_language);
        PlayerPrefs.Save();

        Debug.LogFormat("Language {0} Setting Saved.", (int)SingletonCom.curr_language);
    }

    void SaveSettingTheme()
    {
        PlayerPrefs.SetInt(key_theme, (int)SingletonCom.curr_theme);
        PlayerPrefs.Save();

        Debug.LogFormat("Theme {0} Setting Saved.", (int)SingletonCom.curr_theme);
    }

    void LoadSetting()
    {
        int value_language = PlayerPrefs.GetInt(key_language);
        if(value_language == 0)
        {
            toggle_en.isOn = true;
        }
        else
        {
            toggle_kr.isOn = true;
        }

        int value_theme = PlayerPrefs.GetInt(key_theme);
        if (value_theme == 0)
        {
            toggle_dark.isOn = true;
        }
        else
        {
            toggle_light.isOn = true;
        }

        Debug.LogFormat("{0} {1} Setting Loaded.", value_language, value_theme);
    }

    public void ShowHideSettingModal(bool isOn)
    {
        object_SettingMdoal.SetActive(isOn);
    }

    void UpdateLanguage()
    {
        for(int i=0;i< list_targetLocalClass.Length; i++)
        {
            list_targetLocalClass[i].SetTranslate();
        }
    }

    void SetLanguageKR(bool isOn)
    {
        if (isOn)
        {
            SingletonCom.curr_language = Localization.CurrentLanguage.KOREAN;
            uiToggle_kr.SetOnOff(true);
            SaveSettingLangauge();

            UpdateLanguage();
        }
        else
        {
            uiToggle_kr.SetOnOff(false);
        }
    }

    void SetLanguageEN(bool isOn)
    {
        if (isOn)
        {
            SingletonCom.curr_language = Localization.CurrentLanguage.ENGLISH;
            uiToggle_en.SetOnOff(true);
            SaveSettingLangauge();

            UpdateLanguage();
        }
        else
        {
            uiToggle_en.SetOnOff(false);
        }
    }

    void SetThemeDark(bool isOn)
    {
        if (isOn)
        {
            SingletonCom.curr_theme = ConstString.UIThemeType.theme_dark;
            uiToggle_dark.SetOnOff(true);
            SaveSettingTheme();
        }
        else
        {
            uiToggle_dark.SetOnOff(false);
        }
    }

    void SetThemeLight(bool isOn)
    {
        if (isOn)
        {
            SingletonCom.curr_theme = ConstString.UIThemeType.theme_light;
            uiToggle_light.SetOnOff(true);
            SaveSettingTheme();
        }
        else
        {
            uiToggle_light.SetOnOff(false);
        }
    }
}
