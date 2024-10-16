using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ModalManager;

public class UI_Modal : MonoBehaviour, ITheme
{
    public enum ColorType
    {
        Dark_CyanModal,
        Light_CyanModal
    }

    [Header("Select Type")]
    public ColorType colorType;

    [Header("Components")]
    public Image image_background;
    public TextMeshProUGUI text_Title;
    public TextMeshProUGUI[] text_Content;

    [Header("Components_Options")]
    public Image image_icon;
    public UnityEngine.UI.Outline outline;

    [Header("Colors")]
    Color setModalColor;
    Color setTextTitle;
    Color setTextContent;
    Color setOutLineColor;
    Color setContentColorIcon;

    void Start()
    {
        RememberThis();

        UpdateTheme();
    }

    public void UpdateTheme()
    {
        switch (SingletonCom.curr_theme)
        {
            case ConstString.UIThemeType.theme_dark:
                colorType = ColorType.Dark_CyanModal;
                break;
            case ConstString.UIThemeType.theme_light:
                colorType = ColorType.Light_CyanModal;
                break;
            default:
                colorType = ColorType.Dark_CyanModal;
                break;
        }

        SetDefaultColor();
    }

    public void RememberThis()
    {
        IThemeController.Instance.list_UIComponents.Add(this);
    }

    void SetUI()
    {
        if (image_background != null)
        {
            image_background.color = setModalColor;
        }
        if (text_Title != null)
        {
            text_Title.color = setTextTitle;
        }
        if (text_Content != null)
        {
            for(int i = 0; i < text_Content.Length; i++)
            {
                text_Content[i].color = setTextContent;
            }
        }
        if (outline != null)
        {
            outline.effectColor = setOutLineColor;
        }
        if (image_icon != null)
        {
            image_icon.color = setContentColorIcon;
        }
    }

    void SetDefaultColor()
    {
        switch (colorType)
        {
            case ColorType.Dark_CyanModal:
                setModalColor = UI_Color.black;
                setTextTitle = UI_Color.cyan;
                setTextContent = UI_Color.white;
                setOutLineColor = UI_Color.cyan;
                setContentColorIcon = UI_Color.none;
                break;
            case ColorType.Light_CyanModal:
                setModalColor = UI_Color.white;
                setTextTitle = UI_Color.cyan;
                setTextContent = UI_Color.black;
                setOutLineColor = UI_Color.cyan;
                setContentColorIcon = UI_Color.none;
                break;
            default:
                break;
        }

        SetUI();
    }
}
