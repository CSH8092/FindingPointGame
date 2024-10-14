using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public enum ColorType
    {
        CyanDarkButton
    }

    [Header("Select Type")]
    public ColorType colorType;

    [Header("Components")]
    public Button button;
    public Image image_background;

    [Header("Components_Options")]
    public TextMeshProUGUI buttonText;
    public Image image_icon;
    public UnityEngine.UI.Outline outline;

    [Header("Colors")]
    Color setButtonColor;
    Color setOutLineColor;
    Color setContentColorText;
    Color setContentColorIcon;

    bool isSelected = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
        {
            SetClickColor();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isSelected && button.interactable)
        {
            SetDefaultColor();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSelected && button.interactable)
        {
            SetHoverColor();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            SetHoverColor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
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
        if (button.interactable)
        {
            SetSelectColor();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (button.interactable)
        {
            SetDefaultColor();
        }
    }

    public void SetInteractable(bool isOn)
    {
        button.interactable = isOn;

        if (isOn)
        {
            SetDefaultColor();
        }
        else
        {
            SetDisableColor();
        }
    }

    void OnEnable()
    {
        SetDefaultColor();
    }

    private void Awake()
    {
        if (isSelected)
        {
            return;
        }

        SetDefaultColor();
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
        if (image_background != null)
        {
            image_background.color = setButtonColor;
        }
        if (buttonText != null)
        {
            buttonText.color = setContentColorText;
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
        if (!button.interactable)
        {
            SetDisableColor();
            return;
        }

        isSelected = false;

        switch (colorType)
        {
            case ColorType.CyanDarkButton:
                setButtonColor = UI_Color.black;
                setContentColorText = UI_Color.white_150;
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
            case ColorType.CyanDarkButton:
                setButtonColor = UI_Color.black;
                setContentColorText = UI_Color.white;
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
            case ColorType.CyanDarkButton:
                setButtonColor = UI_Color.black;
                setContentColorText = UI_Color.white_200;
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
            case ColorType.CyanDarkButton:
                setButtonColor = UI_Color.black;
                setContentColorText = UI_Color.white_200;
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
            case ColorType.CyanDarkButton:
                setButtonColor = UI_Color.black;
                setContentColorText = UI_Color.white_150;
                setOutLineColor = UI_Color.cyan_50;
                setContentColorIcon = UI_Color.cyan_50;
                break;
            default:
                break;
        }

        SetUI();
    }
}
