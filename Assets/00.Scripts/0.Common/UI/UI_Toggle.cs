using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Toggle : MonoBehaviour, ITheme, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public enum ColorType
    {
        Dark_CyanToggle,
        Light_CyanToggle
    }

    [Header("Select Type")]
    public ColorType colorType;

    [Header("Components")]
    public Toggle toggle;
    public Image image_toggle;
    public Image image_toggleOn;

    [Header("Components_Options")]
    public TextMeshProUGUI toggleText;
    public Image image_icon;
    public UnityEngine.UI.Outline outline;

    [Header("Colors")]
    Color setButtonColor;
    Color setButtonOnColor;
    Color setOutLineColor;
    Color setContentColorText;
    Color setContentColorIcon;

    bool isSelected = false;

    private void Awake()
    {
        RememberThis();
    }

    private void Start()
    {
        UpdateTheme();
    }

    public void UpdateTheme()
    {
        switch (SingletonCom.curr_theme)
        {
            case ConstString.UIThemeType.theme_dark:
                colorType = ColorType.Dark_CyanToggle;
                break;
            case ConstString.UIThemeType.theme_light:
                colorType = ColorType.Light_CyanToggle;
                break;
            default:
                colorType = ColorType.Dark_CyanToggle;
                break;
        }

        if (isSelected)
        {
            SetSelectColor();
        }
        else
        {
            SetDefaultColor();
        }
    }

    public void RememberThis()
    {
        IThemeController.Instance.list_UIComponents.Add(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (toggle.interactable)
        {
            SetClickColor();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isSelected && toggle.interactable)
        {
            SetDefaultColor();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSelected && toggle.interactable)
        {
            SetHoverColor();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toggle.interactable)
        {
            SetHoverColor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toggle.interactable)
        {
            if (isSelected)
            {
                SetSelectColor();
            }
            else
            {
                SetDefaultColor();
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (toggle.interactable)
        {
            SetSelectColor();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (toggle.interactable)
        {
            SetDefaultColor();
        }
    }

    public void SetInteractable(bool isOn)
    {
        toggle.interactable = isOn;

        if (isOn)
        {
            SetDefaultColor();
        }
        else
        {
            SetDisableColor();
        }
    }

    public void SetOnOff(bool isOn)
    {
        if (isOn)
        {
            SetSelectColor();
        }
        else
        {
            SetDefaultColor();
        }
    }

    void SetUI()
    {
        if (image_toggle != null)
        {
            image_toggle.color = setButtonColor;
        }
        if (image_toggleOn != null)
        {
            image_toggleOn.color = setButtonOnColor;
        }

        if (toggleText != null)
        {
            toggleText.color = setContentColorText;
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

    public void SetColorType(ColorType color)
    {
        colorType = color;
        SetDefaultColor();
    }

    void SetDefaultColor()
    {
        if (!toggle.interactable)
        {
            SetDisableColor();
            return;
        }

        isSelected = false;

        switch (colorType)
        {
            case ColorType.Dark_CyanToggle:
                setButtonColor = UI_Color.black;
                setButtonOnColor = UI_Color.cyan;
                setContentColorText = UI_Color.white_150;
                setOutLineColor = UI_Color.cyan;
                setContentColorIcon = UI_Color.none;
                break;
            case ColorType.Light_CyanToggle:
                setButtonColor = UI_Color.white;
                setButtonOnColor = UI_Color.cyan;
                setContentColorText = UI_Color.black_150;
                setOutLineColor = UI_Color.cyan;
                setContentColorIcon = UI_Color.none;
                break;
            default:
                break;
        }

        SetUI();
    }

    void SetHoverColor()
    {
        switch (colorType)
        {
            case ColorType.Dark_CyanToggle:
                setButtonColor = UI_Color.black;
                setButtonOnColor = UI_Color.cyan;
                setContentColorText = UI_Color.white;
                setOutLineColor = UI_Color.cyan;
                setContentColorIcon = UI_Color.cyan_50;
                break;
            case ColorType.Light_CyanToggle:
                setButtonColor = UI_Color.white;
                setButtonOnColor = UI_Color.cyan;
                setContentColorText = UI_Color.black;
                setOutLineColor = UI_Color.cyan;
                setContentColorIcon = UI_Color.cyan_50;
                break;
            default:
                break;
        }

        SetUI();
    }

    void SetClickColor()
    {
        switch (colorType)
        {
            case ColorType.Dark_CyanToggle:
                setButtonColor = UI_Color.black;
                setButtonOnColor = UI_Color.cyan;
                setContentColorText = UI_Color.white_200;
                setOutLineColor = UI_Color.cyan;
                setContentColorIcon = UI_Color.cyan;
                break;
            case ColorType.Light_CyanToggle:
                setButtonColor = UI_Color.white;
                setButtonOnColor = UI_Color.cyan;
                setContentColorText = UI_Color.black_200;
                setOutLineColor = UI_Color.cyan;
                setContentColorIcon = UI_Color.cyan;
                break;
            default:
                break;
        }

        SetUI();
    }

    void SetSelectColor()
    {
        isSelected = true;

        switch (colorType)
        {
            case ColorType.Dark_CyanToggle:
                setButtonColor = UI_Color.black;
                setButtonOnColor = UI_Color.cyan;
                setContentColorText = UI_Color.white_200;
                setOutLineColor = UI_Color.cyan;
                setContentColorIcon = UI_Color.cyan;
                break;
            case ColorType.Light_CyanToggle:
                setButtonColor = UI_Color.white;
                setButtonOnColor = UI_Color.cyan;
                setContentColorText = UI_Color.black_200;
                setOutLineColor = UI_Color.cyan;
                setContentColorIcon = UI_Color.cyan;
                break;
            default:
                break;
        }

        SetUI();
    }

    void SetDisableColor()
    {
        switch (colorType)
        {
            case ColorType.Dark_CyanToggle:
                setButtonColor = UI_Color.black;
                setButtonOnColor = UI_Color.cyan_50;
                setContentColorText = UI_Color.white_150;
                setOutLineColor = UI_Color.cyan_50;
                setContentColorIcon = UI_Color.cyan_50;
                break;
            case ColorType.Light_CyanToggle:
                setButtonColor = UI_Color.white;
                setButtonOnColor = UI_Color.cyan_50;
                setContentColorText = UI_Color.black_150;
                setOutLineColor = UI_Color.cyan_50;
                setContentColorIcon = UI_Color.cyan_50;
                break;
            default:
                break;
        }

        SetUI();
    }
}
