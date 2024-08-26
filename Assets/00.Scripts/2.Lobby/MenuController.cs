using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Components
    [Header("Buttons")]
    public Button button_Left;
    public Button button_Right;

    [Header("Texts")]
    public TextMeshProUGUI text_selectStage;

    [Header("Components")]
    public CameraCom component_cameraCom;

    // Values
    int selectStageNum;

    void Start()
    {
        button_Left.onClick.AddListener(() => ClickMenuChangeButton(true));
        button_Right.onClick.AddListener(() => ClickMenuChangeButton(false));
    }

    void Update()
    {
        
    }

    void ClickMenuChangeButton(bool isLeft)
    {
        float value = 90;
        if (isLeft)
        {
            value *= -1;
        }

        component_cameraCom.ObjectRotate_SetValue(value);
    }
}
