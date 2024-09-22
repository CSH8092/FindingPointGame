using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public ObjectController component_objectController;
    public CameraCom component_cameraCom;
    SingletonCom sc;

    [Header("UI")]
    public Button button_BackToLobby;

    // Values
    List<MenuObjectCom> list_MenuObjects = new List<MenuObjectCom>();
    List<CapsuleCollider> list_Capsule = new List<CapsuleCollider>();
    Stack<GameObject> stack_Object = new Stack<GameObject>();

    Ray ray;
    RaycastHit raycastHit;
    int layerMask;

    private void Awake()
    {
        sc = SingletonCom.Instance;

        button_BackToLobby.onClick.AddListener(() => BackToLobby());
    }

    void Start()
    {
        skyboxMaterial = RenderSettings.skybox;
        layerMask = LayerMask.GetMask("ButtonObject");

        StartStageGame();
    }

    void Update()
    {
        if (!sc.isObservationMode)
        {
            ClickMenuChangeButton();
        }
    }

    void BackToLobby()
    {
        SceneLoader.Instance.LoadSceneByName("02.Lobby");
    }

    void ClickMenuChangeButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = component_cameraCom.camera_this.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
            {
                if (raycastHit.transform.tag.Equals("Button"))
                {
                    Debug.LogFormat("Click {0} Button", raycastHit.transform.gameObject.name);
                    if (raycastHit.transform.name.Contains("_Left"))
                    {
                        ClickFieldButton(false); // 버튼 누르면 반대 방향으로 움직여야 하므로 flag 반대
                    }
                    else if (raycastHit.transform.name.Contains("_Right"))
                    {
                        ClickFieldButton(true);
                    }
                    else
                    {
                        component_objectController.ChangeObservationMode(raycastHit.transform.gameObject);
                    }
                }
            }
        }
    }

    void StartStageGame()
    {
        Debug.Log("Start In Game Scene, " + SingletonCom.Instance.curr_StageNum);
        CreateStageObjects();
    }

    public void CreateStageObjects()
    {
        // Get Stage Mesh
        GameObject[] meshes = Resources.LoadAll<GameObject>("Stages");
        GameObject mesh_Target = meshes[SingletonCom.Instance.curr_StageNum];

        // Set Stage Object
        GameObject menuObject = Instantiate(prefab_MenuButton); //, object_ParentMenu.transform
        menuObject.transform.position = new Vector3(-20, 1, -5);
        menuObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        menuObject.name = mesh_Target.name;
        MenuObjectCom menuCom = menuObject.GetComponent<MenuObjectCom>();

        // Set OutLine
        GameObject model = Instantiate(mesh_Target, menuCom.transform);
        for (int i = 0; i < model.transform.childCount; i++)
        {
            model.transform.GetChild(i).gameObject.AddComponent<Outline>();
        }

        list_MenuObjects.Add(menuCom);

        menuCom.SetRandomPoint();

        // Set Cloths Collider
        list_Capsule.Add(menuObject.GetComponent<CapsuleCollider>());
        object_ClothLeft.capsuleColliders = list_Capsule.ToArray();
        object_ClothRight.capsuleColliders = list_Capsule.ToArray();

        // Save Object Data
        stack_Object.Push(menuObject);

        stack_Object.Peek().transform.DOMove(new Vector3(0, 1, -20), 1);
    }

    void ClickFieldButton(bool isRight)
    {
        if (isRight)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(object_RightButton.transform.DOLocalMoveY(0.26f, 0.2f).SetEase(Ease.InSine));
            sequence.Append(object_RightButton.transform.DOLocalMoveY(0.42f, 0.2f).SetEase(Ease.InSine));

            // Control Object
            GameObject target = stack_Object.Pop();
            target.transform.DOMove(new Vector3(20, 1, -5), 1).OnComplete(() => StartCoroutine(DestroyTarget(target)));
            Debug.LogError("here! 1");

            // Floor Animation
            component_cameraCom.ObjectRotate_SetValue(object_ParentMenu.transform.up, -90);
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(object_LeftButton.transform.DOLocalMoveY(0.26f, 0.2f).SetEase(Ease.InSine));
            sequence.Append(object_LeftButton.transform.DOLocalMoveY(0.42f, 0.2f).SetEase(Ease.InSine));

            // Control Object
            GameObject target = stack_Object.Pop();
            target.transform.DOMove(new Vector3(0, 50, 20), 1).OnComplete(() => StartCoroutine(DestroyTarget(target)));
            Debug.LogError("here! 2");

            // Floor Animation
            //component_cameraCom.ObjectRotate_SetValue(object_ParentMenu.transform.up, 90);
        }
    }

    IEnumerator DestroyTarget(GameObject target)
    {
        // Set Cloths Collider
        list_Capsule.RemoveAt(0);
        object_ClothLeft.capsuleColliders = list_Capsule.ToArray();
        object_ClothRight.capsuleColliders = list_Capsule.ToArray();

        PinController.DeleteAllPinObjects();

        yield return new WaitForSeconds(0.2f);

        target.SetActive(false);
        GameObject.Destroy(target);

        yield return new WaitForSeconds(0.2f);

        CreateStageObjects();

        // Floor Animation
        component_cameraCom.ObjectRotate_SetValue(object_ParentMenu.transform.up, -90);

        yield return null;
    }
}
