using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Other : MonoBehaviour, ITheme
{
    public enum ColorType
    {
        DarkUIMode,
        LightUIMode
    }

    [Header("Select Type")]
    public ColorType colorType;

    [Header("Components")]
    public Camera camera_target;
    public Image[] image_target;
    public TextMeshPro[] text_target;
    public TextMeshProUGUI[] text_targetUgui;

    [Header("Colors")]
    Color setBackgroundColor;
    Color setTextColor;
    Color setTextColorUgui;

    public void UpdateTheme()
    {
        switch (SingletonCom.curr_theme)
        {
            case ConstString.UIThemeType.theme_dark:
                colorType = ColorType.DarkUIMode;
                break;
            case ConstString.UIThemeType.theme_light:
                colorType = ColorType.LightUIMode;
                break;
            default:
                colorType = ColorType.DarkUIMode;
                break;
        }

        SetDefaultColor();
    }

    public void RememberThis()
    {
        IThemeController.Instance.list_UIComponents.Add(this);
    }

    public void RemoveThis()
    {
        IThemeController.Instance.list_UIComponents.Remove(this);
    }

    private void Start()
    {
        RememberThis();

        UpdateTheme();
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        RemoveThis();
    }

    void SetUI()
    {
        if (camera_target != null)
        {
            camera_target.backgroundColor = setBackgroundColor;
        }
        if (image_target != null)
        {
            for (int i = 0; i < image_target.Length; i++)
            {
                image_target[i].color = setBackgroundColor;
            }
        }
        if (text_target != null)
        {
            for(int i=0;i< text_target.Length; i++)
            {
                text_target[i].color = setTextColor;
            }
        }
        if (text_targetUgui != null)
        {
            for (int i = 0; i < text_targetUgui.Length; i++)
            {
                text_targetUgui[i].color = setTextColorUgui;
            }
        }
    }

    void SetDefaultColor()
    {
        switch (colorType)
        {
            case ColorType.DarkUIMode:
                setBackgroundColor = UI_Color.black;
                setTextColor = UI_Color.white;
                setTextColorUgui = UI_Color.cyan;
                break;
            case ColorType.LightUIMode:
                setBackgroundColor = UI_Color.white;
                setTextColor = UI_Color.black;
                setTextColorUgui = UI_Color.cyan;
                break;
            default:
                break;
        }

        SetUI();
    }
}
