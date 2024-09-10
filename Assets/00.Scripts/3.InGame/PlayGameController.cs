using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayGameController : MonoBehaviour
{
    [Header("Target Object")]
    Material skyboxMaterial;
    public GameObject object_ParentMenu;
    public GameObject prefab_MenuButton;
    public Cloth object_ClothLeft;
    public Cloth object_ClothRight;
    public GameObject object_LeftButton;
    public GameObject object_RightButton;
    [SerializeField]
    Material material_Screen;

    [SerializeField]
    int count_menu = 8;
    [SerializeField]
    float value_farDistance = 10f;

    // Components
    [Header("Components")]
    public CameraCom component_cameraCom;
    SingletonCom sc;

    [Header("Texts")]
    public TextMeshPro text_selectStage;
    public TextMeshProUGUI text_selectStagebyUI;

    // Values
    List<MenuObjectCom> list_MenuObjects = new List<MenuObjectCom>();
    List<CapsuleCollider> list_Capsule = new List<CapsuleCollider>();

    float angle;
    float x, y, z;

    int selectStageNum = 0;
    int prevStageIndex = -1;

    Sequence sequence_screen;

    private void Awake()
    {
        sc = SingletonCom.Instance;
    }

    void Start()
    {
        skyboxMaterial = RenderSettings.skybox;
        layerMask = LayerMask.GetMask("ButtonObject");

        ReadNSetData();
    }

    void Update()
    {
        ClickMenuChangeButton();
    }

    Ray ray;
    RaycastHit raycastHit;
    int layerMask;
    void ClickMenuChangeButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = component_cameraCom.camera_this.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 100, layerMask))
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
        Debug.LogFormat("<color=yellow>Click {0} Object!</color>", selectStageNum);
    }

    void ReadNSetData()
    {
        CreateMenu();

        int index = 0;
        SetSelectMenu(0, index);
    }

    public void CreateMenu()
    {
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

            object_RightButton.transform.DOLocalMoveY(0.26f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InSine);
        }
        else
        {
            setStageNum = ++selectStageNum;

            object_LeftButton.transform.DOLocalMoveY(0.26f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InSine);
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
        //text_selectStage.text = stageName;
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

        //if (prevStageIndex != -1)
        //{
        //    list_MenuObjects[prevStageIndex].SetHighlight(false);
        //}

        //list_MenuObjects[stageNum].SetHighlight(true);
        prevStageIndex = stageNum;
    }
}
