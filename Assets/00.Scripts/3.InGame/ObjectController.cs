using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ObjectController : MonoBehaviour
{
    // Components
    [Header("Buttons")]
    public Button button_diffColor;
    public Button button_diffPos;
    public Button button_diffRot;
    public Button button_diffSize;

    [Header("Prefabs")]
    public GameObject prefab_PinPoint;

    [Header("Components")]
    public CameraCom component_cameraCom;

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
                GameObject pinObject = Instantiate(prefab_PinPoint, component_cameraCom.object_target.transform);
                PinPoint pinPoint = pinObject.GetComponent<PinPoint>();
                pinPoint.CreatePinPoint(component_cameraCom.camera_this, raycastHit.point, currentPinType);
            }
        }
    }

    void SetPinType(ConstString.PinType type)
    {
        currentPinType = type;
    }
}
