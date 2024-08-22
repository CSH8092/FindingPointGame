using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObejctControlCom : MonoBehaviour
{
    // Components
    [Header("Prefabs")]
    public GameObject prefab_PinPoint;

    [Header("Target Object")]
    public GameObject object_Target;

    // Values
    Ray ray;
    RaycastHit raycastHit;
    int layerMask;

    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("TargetObject");
    }

    void Update()
    {
        SetPinPoint();
    }


    void SetPinPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
            {
                GameObject pinObject = Instantiate(prefab_PinPoint, object_Target.transform);
                PinPoint pinPoint = pinObject.GetComponent<PinPoint>();
                pinPoint.SetPinPosition(raycastHit.point);

                SingletonCom.Instance.list_PinPoints.Add(pinPoint);
            }
        }
    }
}
