using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ObejctControlCom : MonoBehaviour
{
    // Components
    [Header("Buttons")]
    public Button button_diffColor;
    public Button button_diffPos;
    public Button button_diffRot;
    public Button button_diffSize;

    [Header("Prefabs")]
    public GameObject prefab_PinPoint;

    [Header("Target Object")]
    public GameObject object_Target;
    public Camera camera_mainCam;

    // Values
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
    }

    void Update()
    {
        CheckPinPoint();
    }

    void CheckPinPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
            {
                GameObject pinObject = Instantiate(prefab_PinPoint, object_Target.transform);
                PinPoint pinPoint = pinObject.GetComponent<PinPoint>();
                pinPoint.CreatePinPoint(camera_mainCam, raycastHit.point, currentPinType);
            }
        }
    }

    void SetPinType(ConstString.PinType type)
    {
        currentPinType = type;
    }
}
