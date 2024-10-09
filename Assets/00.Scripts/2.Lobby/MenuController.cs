using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class MenuController : MonoBehaviour
{
    [Header("Resources")]
    public Material material_Hologram;

    [Header("Target Object")]
    public GameObject object_ParentMenu;
    public GameObject prefab_MenuButton;
    public Cloth object_ClothLeft;
    public Cloth object_ClothRight;
    public GameObject object_LeftButton;
    public GameObject object_RightButton;
    [SerializeField]
    Material material_Screen;
    Material skyboxMaterial;

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
    public TextMeshPro text_selectStage;
    public TextMeshProUGUI text_selectStagebyUI;

    // Values
    List<MenuObjectCom> list_MenuObjects = new List<MenuObjectCom>();
    List<CapsuleCollider> list_Capsule= new List<CapsuleCollider>();

    float angle;
    float x, y, z;

    int selectStageNum = 0;
    int prevStageIndex = -1;

    Sequence sequence_screen;

    Ray ray;
    RaycastHit raycastHit;
    int layerMask;

    private void Awake()
    {
        sc = SingletonCom.Instance;
    }

    void Start()
    {
        skyboxMaterial = RenderSettings.skybox;
        layerMask = LayerMask.GetMask("ButtonObject");

        ReadNSetData();

        button_Left.onClick.AddListener(() => ClickMenuChangeButton(false));
        button_Right.onClick.AddListener(() => ClickMenuChangeButton(true));
    }

    void Update()
    {
        //CheckHoverEvent();
        ClickMenuChangeButton();

        if (sc.isMenuArrange)
        {
            MenuArrange();
        }
    }

    void ClickMenuChangeButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = component_cameraCom.camera_this.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out raycastHit, 100, layerMask))
            {
                if (raycastHit.transform.tag.Equals("Button"))
                {
                    Debug.LogFormat("Click {0} Button", raycastHit.transform.gameObject.name);
                    if (raycastHit.transform.name.Contains("_Left"))
                    {
                        ClickMenuChangeButton(false); // 버튼 누르면 반대 방향으로 움직여야 하므로 flag 반대
                    }
                    else if (raycastHit.transform.name.Contains("_Right"))
                    {
                        ClickMenuChangeButton(true);
                    }
                    else
                    {
                        ClickGameStart();
                    }
                }
            }
        }
    }

    void ClickGameStart()
    {
        Debug.LogFormat("<color=yellow>Click {0} Stage!</color>", selectStageNum);
        SingletonCom.Instance.curr_StageNum = selectStageNum;
        SceneLoader.Instance.LoadSceneByName("03.InGame");
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
        GameObject[] fbxFiles = Resources.LoadAll<GameObject>("Stages");

        int totalCount = fbxFiles.Length;
        int i = 0;

        foreach(GameObject fbx in fbxFiles)
        {
            // Parent Menu Transform의 Z축에서 부터 시작
            angle = (i++ * Mathf.PI * 2 / totalCount) - Mathf.PI / 2;
            x = Mathf.Cos(angle) * value_farDistance;
            z = Mathf.Sin(angle) * value_farDistance;

            Vector3 new_position = new Vector3(x, 0, z);
            GameObject menuObject = Instantiate(prefab_MenuButton, object_ParentMenu.transform);
            menuObject.transform.position = new_position;
            menuObject.transform.LookAt(object_ParentMenu.transform);
            menuObject.name = fbx.name; // string.Format("Menu Button {0}", i);

            MenuObjectCom menuCom = menuObject.GetComponent<MenuObjectCom>();
            GameObject model = Instantiate(fbx, menuCom.transform);

            if(model.TryGetComponent<ObjectHandler>(out ObjectHandler handler))
            {
                menuCom.objectHandler = handler;
            }

            // Setting Material
            //menuCom.SetMaterial(material_Hologram);

            list_MenuObjects.Add(menuCom);
            list_Capsule.Add(menuObject.GetComponent<CapsuleCollider>());
        }

        count_menu = totalCount;

        object_ClothLeft.capsuleColliders = list_Capsule.ToArray();
        object_ClothRight.capsuleColliders = list_Capsule.ToArray();
    }

    void ClickMenuChangeButton(bool isRight)
    {
        float value = 360 / count_menu;
        int setStageNum;
        if (isRight)
        {
            value *= -1;
            setStageNum = --selectStageNum;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(object_RightButton.transform.DOLocalMoveY(0.26f, 0.2f).SetEase(Ease.InSine));
            sequence.Append(object_RightButton.transform.DOLocalMoveY(0.42f, 0.2f).SetEase(Ease.InSine));
        }
        else
        {
            setStageNum = ++selectStageNum;


            Sequence sequence = DOTween.Sequence();
            sequence.Append(object_LeftButton.transform.DOLocalMoveY(0.26f, 0.2f).SetEase(Ease.InSine));
            sequence.Append(object_LeftButton.transform.DOLocalMoveY(0.42f, 0.2f).SetEase(Ease.InSine));
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
        string[] split = stageName.Split('.');
        text_selectStage.text = split[1];
        //text_selectStagebyUI.text = stageName;

        if (material_Screen != null)
        {
            if (material_Screen.HasProperty("_Brightness"))
            {
                material_Screen.SetFloat("_Brightness", 1);

                // 랜덤으로 Brightness 바꿔주기? & 리소스 많이 잡아먹는지 확인하고 적당할때 kill 해주기 or 일정시간마다 실행
                sequence_screen = DOTween.Sequence();
                sequence_screen.Append(DOTween.To(() => material_Screen.GetFloat("_Brightness"), x => material_Screen.SetFloat("_Brightness", x), 0f, 0.2f).SetEase(Ease.InOutBack));
                sequence_screen.AppendInterval(0.1f);
                sequence_screen.Append(DOTween.To(() => material_Screen.GetFloat("_Brightness"), x => material_Screen.SetFloat("_Brightness", x), 1f, 0.2f).SetEase(Ease.InOutBack));
                sequence_screen.Play();
            }
        }

        if (prevStageIndex != -1)
        {
            list_MenuObjects[prevStageIndex].SetHighlight(false);
        }

        list_MenuObjects[stageNum].SetHighlight(true);
        prevStageIndex = stageNum;

        ///SetSkyBoxRandom();
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
                new_skyboxMaterial.SetColor("_TopColor", Color.green); // 일단 고정
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
