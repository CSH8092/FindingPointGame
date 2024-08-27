using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Target Object")]
    public GameObject object_ParentMenu;
    public GameObject prefab_MenuButton;
    [SerializeField]
    int count_menu = 8;
    [SerializeField]
    float value_farDistance = 10f;

    // Components
    [Header("Components")]
    public CameraCom component_cameraCom;
    SingletonCom sc;

    [Header("Buttons")]
    public Button button_Left;
    public Button button_Right;

    [Header("Texts")]
    public TextMeshProUGUI text_selectStage;

    // Values
    List<GameObject> list_MenuObjects = new List<GameObject>();

    float angle;
    float x, y, z;
    int selectStageNum = 0;

    void Start()
    {
        sc = SingletonCom.Instance;

        ReadNSetData();

        button_Left.onClick.AddListener(() => ClickMenuChangeButton(true));
        button_Right.onClick.AddListener(() => ClickMenuChangeButton(false));
    }

    void Update()
    {
        if (sc.isMenuArrange)
        {
            MenuArrange();
        }
    }

    void MenuArrange()
    {
        sc.isMenuArrange = false;

        float min = 9999;
        int targetIndex = -1;
        for(int i=0;i< list_MenuObjects.Count; i++)
        {
            float distance = Vector3.Distance(component_cameraCom.camera_this.transform.position, list_MenuObjects[i].transform.position);
            if(distance < min)
            {
                min = distance;
                targetIndex = i;
            }
        }

        // 화면 Drag로 Menu 움직였을 때, 자동 Select Menu 보정
        Vector3 to = list_MenuObjects[targetIndex].transform.forward * -1;
        Vector3 from = component_cameraCom.camera_this.transform.position - object_ParentMenu.transform.position;
        float angle = Vector3.SignedAngle(to, from, Vector3.up);

        SetSelectMenu(angle, targetIndex);
    }

    void ReadNSetData()
    {
        // 1. menu button 마다 stage mesh 추가
        // 2. Json 등의 파일 읽어와서 이미 unlock한 stage 처리 필요 -> material 설정
        CreateMenu(count_menu);

        int index = 0;
        SetState(index);
    }

    public void CreateMenu(int count_menu)
    {
        for (int i = 0; i < count_menu; i++)
        {
            // Parent Menu Transform의 Z축에서 부터 시작
            angle = (i * Mathf.PI * 2 / count_menu) - Mathf.PI / 2;
            x = Mathf.Cos(angle) * value_farDistance;
            z = Mathf.Sin(angle) * value_farDistance;

            Vector3 new_position = new Vector3(x, 0, z);
            GameObject menuObject = Instantiate(prefab_MenuButton, object_ParentMenu.transform);
            menuObject.transform.position = new_position;
            menuObject.transform.LookAt(object_ParentMenu.transform);
            menuObject.name = string.Format("Menu Button {0}", i);

            list_MenuObjects.Add(menuObject);
        }
    }

    void ClickMenuChangeButton(bool isLeft)
    {
        float value = 360 / count_menu;
        int setStageNum;
        if (isLeft)
        {
            value *= -1;
            setStageNum = --selectStageNum;
        }
        else
        {
            setStageNum = ++selectStageNum;
        }

        // Index 조정
        if (setStageNum < 0)
        {
            setStageNum = count_menu - 1;
        }
        else if (setStageNum > (count_menu - 1))
        {
            setStageNum = 0;
        }

        SetSelectMenu(value, setStageNum);
    }

    void SetSelectMenu(float angle, int targetIndex)
    {
        component_cameraCom.ObjectRotate_SetValue(object_ParentMenu.transform.up, angle);
        SetState(targetIndex);
    }

    void SetState(int stageNum)
    {
        selectStageNum = stageNum;
        string stageName = object_ParentMenu.transform.GetChild(selectStageNum).name;
        text_selectStage.text = stageName;
    }
}
