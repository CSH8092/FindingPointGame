using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ModalManager;

public class PlayGameController : MonoBehaviour
{
    [Header("Target Object")]
    Material skyboxMaterial;
    public GameObject object_Answer;
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
    public UIController component_uiController;
    public ObjectController component_objectController;
    public CameraCom component_cameraCom;
    SingletonCom sc;

    // Values
    List<CapsuleCollider> list_Capsule = new List<CapsuleCollider>();
    Stack<IObject> stack_Object = new Stack<IObject>();
    GameObject[] meshes;

    Ray ray;
    RaycastHit raycastHit;
    int layerMask;

    // Game Controllers
    int count_AllNum = 20; // 20개로 일단 고정
    int count_AllWrong = 3; // 3개로 고정

    bool flag_isFirstStart = true;
    bool flag_wait = false;

    private void Awake()
    {
        sc = SingletonCom.Instance;
    }

    void Start()
    {
        skyboxMaterial = RenderSettings.skybox;
        layerMask = LayerMask.GetMask("ButtonObject");

        flag_isFirstStart = true;

        component_uiController.InitUI(count_AllNum, count_AllWrong);
        LoadData();
        StartStageGame();
    }

    void Update()
    {
        if (!sc.isObservationMode)
        {
            ClickMenuChangeButton();
        }
    }

    void LoadData()
    {
        // Get Stage Mesh
        meshes = Resources.LoadAll<GameObject>("Stages");
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
                        ClickFieldButton(false);
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

        ButtonSet[] set = new ButtonSet[] { new ButtonSet("Start", SettingStartObjects) };
        ModalManager.Instance.ShowModal("2", "9", set);
    }

    void SettingStartObjects()
    {
        // Setting UI
        CreateStageObjects();
    }

    public void CreateStageObjects()
    {
        component_uiController.InitToggleCheck();

        // Get Stage Mesh
        int currentStageIndex = SingletonCom.Instance.curr_StageNum;

//#if UNITY_EDITOR
//        // donut test
//        currentStageIndex = 1;
//#endif

        GameObject mesh_Target = meshes[currentStageIndex];

        if (flag_isFirstStart)
        {
            // Show Sample
            Instantiate(mesh_Target, object_Answer.transform);
            flag_isFirstStart = false;
        }

        // Create Object
        Factory.ObjectType type = (Factory.ObjectType)currentStageIndex; // Factory.ObjectType.pudding;
        IObject objectTarget = Factory.CreateObject(type, mesh_Target);
        stack_Object.Push(objectTarget);

        // Init & Move Object Data
        GameObject tmp = stack_Object.Peek().Object_Target;
        tmp.transform.position = new Vector3(0, 1, -20);
        tmp.transform.rotation = Quaternion.Euler(Vector3.zero);
        //tmp.transform.position = new Vector3(-20, 1, -5);
        //tmp.transform.DOMove(new Vector3(0, 1, -20), 1);

        /* Cloths는 폐기, 나중에 미닫이문 추가 예정
        // Set Cloths Collider
        list_Capsule.Add(menuObject.GetComponent<CapsuleCollider>());
        object_ClothLeft.capsuleColliders = list_Capsule.ToArray();
        object_ClothRight.capsuleColliders = list_Capsule.ToArray();
        */

        flag_wait = false;
    }

    void ClickFieldButton(bool isRight)
    {
        if (flag_wait)
        {
            return;
        }

        flag_wait = true;

        if (isRight)
        {
            component_uiController.CheckNoProblemState();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(object_RightButton.transform.DOLocalMoveY(0.26f, 0.2f).SetEase(Ease.InSine));
            sequence.Append(object_RightButton.transform.DOLocalMoveY(0.42f, 0.2f).SetEase(Ease.InSine));

            // Control Object
            SendAnswer(stack_Object.Peek().array_diffPoint);
            GameObject target = stack_Object.Pop().Object_Target;
            target.transform.DOMove(new Vector3(20, 1, -5), 1).OnComplete(() => StartCoroutine(DestroyTarget(target)));

            // Floor Animation
            component_cameraCom.ObjectRotate_SetValue(object_ParentMenu.transform.up, -90);
        }
        else
        {
            if (component_uiController.AddCountPass())
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(object_LeftButton.transform.DOLocalMoveY(0.26f, 0.2f).SetEase(Ease.InSine));
                sequence.Append(object_LeftButton.transform.DOLocalMoveY(0.42f, 0.2f).SetEase(Ease.InSine));

                // Control Object
                GameObject target = stack_Object.Peek().Object_Target;
                Action destroyAction = () => StartCoroutine(DestroyTarget(target));

                stack_Object.Peek().FadeInOut(false, destroyAction);
                stack_Object.Pop();

                Debug.LogFormat("<color=yellow>패스!</color>");
            }
            else
            {
                Debug.LogFormat("<color=red>패스 횟수 초과!</color>");

                ButtonSet[] set = new ButtonSet[] { new ButtonSet("Ok", null) };
                ModalManager.Instance.ShowModal("3", "10", set);

                flag_wait = false;
            }
        }
    }

    void SendAnswer(int[] answer)
    {
        // 답변 제출 및 비교 & 결과 도출
        int[] submit = component_uiController.GetToggleCheck();

        bool result = true;
        for(int i = 0; i < answer.Length; i++)
        {
            if (!answer[i].Equals(submit[i]))
            {
                result = false;
            }
        }

        Debug.Log("비교 결과 (answer, submit) : \n" + string.Join(',', answer) + " || " + string.Join(',', submit));

        if (result)
        {
            Debug.LogFormat("<color=green>맞췄습니다!</color>");
            if (component_uiController.AddCountSlider_A())
            {
                Debug.LogFormat("<color=cyan>게임 종료</color>");

                ButtonSet[] set = new ButtonSet[] { new ButtonSet("Ok", EndGameStage) };
                ModalManager.Instance.ShowModal("7", "11", set);
            }
        }
        else
        {
            Debug.LogFormat("<color=red>틀렸습니다!</color>");
            if (component_uiController.AddCountSlider_W())
            {
                Debug.LogFormat("<color=cyan>게임 종료</color>");

                ButtonSet[] set = new ButtonSet[] { new ButtonSet("Ok", EndGameStage) };
                ModalManager.Instance.ShowModal("8", "12", set);
            }
        }
    }

    void EndGameStage()
    {
        component_uiController.button_BackToLobby.onClick.Invoke();
    }

    IEnumerator DestroyTarget(GameObject target)
    {
        /* Cloths는 폐기, 나중에 미닫이문 추가 예정
        // Set Cloths Collider
        list_Capsule.RemoveAt(0);
        object_ClothLeft.capsuleColliders = list_Capsule.ToArray();
        object_ClothRight.capsuleColliders = list_Capsule.ToArray();
        */

        ///PinController.DeleteAllPinObjects();


        target.SetActive(false);
        GameObject.Destroy(target);

        yield return new WaitForSeconds(0.15f);

        CreateStageObjects();

        // Floor Animation
        component_cameraCom.ObjectRotate_SetValue(object_ParentMenu.transform.up, -90);

        yield return null;
    }
}
