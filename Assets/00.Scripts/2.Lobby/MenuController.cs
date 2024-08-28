using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class MenuController : MonoBehaviour
{
    [Header("Target Object")]
    Material skyboxMaterial;
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
    List<MenuObjectCom> list_MenuObjects = new List<MenuObjectCom>();

    float angle;
    float x, y, z;

    int selectStageNum = 0;
    int prevStageIndex = -1;

    private void Awake()
    {
        sc = SingletonCom.Instance;
    }

    void Start()
    {
        skyboxMaterial = RenderSettings.skybox;

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
        // 1. Resources에 있는 mesh로 Menu 생성
        CreateMenu();

        // 2. Json 등의 파일 읽어와서 이미 unlock한 stage 처리 필요 -> material 설정

        // 3. 가장 최근에 실행했던 메뉴로 돌려줌

        int index = 0;
        SetSelectMenu(0, index);
    }

    public void CreateMenu()
    {
#if true
        Mesh[] meshes = Resources.LoadAll<Mesh>("Stages");

        int totalCount = meshes.Length;
        int i = 0;
        foreach (Mesh mesh in meshes)
        {
            // Parent Menu Transform의 Z축에서 부터 시작
            angle = (i++ * Mathf.PI * 2 / totalCount) - Mathf.PI / 2;
            x = Mathf.Cos(angle) * value_farDistance;
            z = Mathf.Sin(angle) * value_farDistance;

            Vector3 new_position = new Vector3(x, 0, z);
            GameObject menuObject = Instantiate(prefab_MenuButton, object_ParentMenu.transform);
            menuObject.transform.position = new_position;
            menuObject.transform.LookAt(object_ParentMenu.transform);
            menuObject.name = mesh.name; // string.Format("Menu Button {0}", i);

            MenuObjectCom menuCom = menuObject.GetComponent<MenuObjectCom>();
            menuCom.SetMeshData(mesh);
            list_MenuObjects.Add(menuCom);
        }
        count_menu = totalCount;
#else
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

            MenuObjectCom menuCom = menuObject.GetComponent<MenuObjectCom>();


            list_MenuObjects.Add(menuCom);
        }
#endif
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

        if (prevStageIndex != -1)
        {
            list_MenuObjects[prevStageIndex].SetHighlight(false);
        }

        list_MenuObjects[stageNum].SetHighlight(true);
        prevStageIndex = stageNum;

        SetSkyBoxRandom();
    }

    void SetSkyBoxRandom()
    {
        // 좀만 더 해보고, 괜찮으면 animation 넣어서 smooth하게 해주기

        if (skyboxMaterial != null)
        {
            Material new_skyboxMaterial = new Material(skyboxMaterial);
            
            // Set New Color
            if (new_skyboxMaterial.HasProperty("_TopColor"))
            {
                new_skyboxMaterial.SetColor("_TopColor", Color.cyan); // 일단 고정
            }
            if (new_skyboxMaterial.HasProperty("_BottomColor"))
            {
                new_skyboxMaterial.SetColor("_BottomColor", Color.magenta);
            }

            // Set New Vector
            if (new_skyboxMaterial.HasProperty("_Up"))
            {
                Random rand = new Random();

                float x = (float)rand.NextDouble();
                float y = (float)rand.NextDouble();
                float z = rand.Next(0, 1);

                Vector3 new_Up = new Vector3(x, y, z);
                new_skyboxMaterial.SetVector("_Up", new_Up);
            }

            RenderSettings.skybox = new_skyboxMaterial;
        }
    }
}
