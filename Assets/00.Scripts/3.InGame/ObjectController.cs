using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ObjectController : MonoBehaviour
{
    float time_animation = 0.5f;

    // Components
    [Header("Buttons")]
    public Button button_diffColor;
    public Button button_diffPos;
    public Button button_diffRot;
    public Button button_diffSize;
    public Button button_diffOther;
    public Button button_quitButton;

    [Header("Prefabs")]
    public GameObject prefab_PinPoint;

    [Header("Components")]
    //public GameObject camera_ovservation;
    public CameraCom component_cameraCom;
    public CameraCom component_mainCam;

    // Values
    GameObject object_TargetObject;
    Vector3 object_OriginPos;
    Quaternion object_OriginRot;
    bool isObservationMode = false;

    Ray ray;
    RaycastHit raycastHit;
    int layerMask;

    ConstString.PinType currentPinType = ConstString.PinType.type_color;

    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("TargetObject");

        button_diffColor.onClick.AddListener(() => SetPinType(ConstString.PinType.type_color));
        button_diffPos.onClick.AddListener(() => SetPinType(ConstString.PinType.type_position));
        button_diffRot.onClick.AddListener(() => SetPinType(ConstString.PinType.type_rotation));
        button_diffSize.onClick.AddListener(() => SetPinType(ConstString.PinType.type_size));
        button_diffOther.onClick.AddListener(() => SetPinType(ConstString.PinType.type_other));

        button_quitButton.onClick.AddListener(() => EndObservationMode());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndObservationMode();
        }

        if (isObservationMode)
        {
            CheckPinPoint();
        }
    }

    public void ChangeObservationMode(GameObject object_Target)
    {
        // Remember Target Object
        object_TargetObject = object_Target;
        object_OriginPos = object_TargetObject.transform.position;
        object_OriginRot = object_TargetObject.transform.localRotation;

        GameObject object_mesh = object_TargetObject;
        if (object_TargetObject.transform.childCount > 0)
        {
            object_mesh = object_TargetObject.transform.GetChild(0).gameObject;
        }

        // Turn Off OutLine
        //if (object_Target.TryGetComponent<Outline>(out Outline outline))
        //{
        //    outline.enabled = false;
        //}

        // Set Target Object Layer
        object_mesh.layer = 7;
        for (int i = 0; i < object_mesh.transform.childCount; i++)
        {
            object_mesh.transform.GetChild(i).gameObject.layer = 7;
        }

        // Set Target Object Position
        object_TargetObject.transform.DOMove(new Vector3(0, -0.3f, -26), time_animation).SetEase(Ease.InOutBack);
        object_TargetObject.transform.DORotate(new Vector3(0, 0, 0), time_animation).SetEase(Ease.InOutQuad);

        // Set Target Object to Camera Com
        component_cameraCom.transform.position = new Vector3(0, 0, -33f);
        component_cameraCom.SetTargetObject(object_Target);

        isObservationMode = true;
        component_cameraCom.gameObject.SetActive(isObservationMode);
        SingletonCom.Instance.isObservationMode = this.isObservationMode;

        // Set MainCam Culling Mask
        component_mainCam.camera_this.cullingMask &= ~(1 << LayerMask.NameToLayer("PinObject"));
    }

    void EndObservationMode()
    {
        Debug.Log("End Observation Mode");

        GameObject object_mesh = object_TargetObject;
        if (object_TargetObject.transform.childCount > 0)
        {
            object_mesh = object_TargetObject.transform.GetChild(0).gameObject;
        }

        // Set Target Object Layer
        object_mesh.layer = 9;
        for (int i = 0; i < object_mesh.transform.childCount; i++)
        {
            object_mesh.transform.GetChild(i).gameObject.layer = 9;
        }

        // Set Target Object Position
        object_TargetObject.transform.DOMove(object_OriginPos, time_animation).SetEase(Ease.InOutQuad);
        object_TargetObject.transform.DOLocalRotate(object_OriginRot.eulerAngles, time_animation).SetEase(Ease.InOutQuad);

        isObservationMode = false;
        component_cameraCom.gameObject.SetActive(isObservationMode);
        SingletonCom.Instance.isObservationMode = this.isObservationMode;

        // Set MainCam Culling Mask
        component_mainCam.camera_this.cullingMask |= 1 << LayerMask.NameToLayer("PinObject");
    }

    void CheckPinPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = component_cameraCom.camera_this.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 10000, layerMask))
            {
                GameObject pinObject = Instantiate(prefab_PinPoint, component_cameraCom.object_target.transform);
                PinPoint pinPoint = pinObject.GetComponent<PinPoint>();
                pinPoint.CreatePinPoint(component_cameraCom.camera_this, raycastHit, currentPinType);
            }
        }
    }

    void SetPinType(ConstString.PinType type)
    {
        currentPinType = type;
        Debug.LogFormat("Set Pin Type : {0}", currentPinType.ToString());
    }
}
