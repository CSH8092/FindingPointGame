using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PinPoint : MonoBehaviour
{
    [Header("Objects")]
    public GameObject object_head;
    public Renderer render_head;
    public LineRenderer linerender_line;
    public GameObject object_point;
    Camera camera_targetCam;

    [Header("Contents")]
    [SerializeField]
    ConstString.PinType thisType;

    // Values
    Vector3 screenSpace;
    Vector3 adjustedScreenSpace;
    Vector3 adjustedWorldSpace;
    float recalScale = 50f;

    void Start()
    {
        SingletonCom.Instance.list_PinPoints.Add(this);
    }

    void Update()
    {
        //AdjustScale();
    }

    void AdjustScale()
    {
        if (camera_targetCam != null)
        {
            screenSpace = camera_targetCam.WorldToScreenPoint(object_head.transform.position);
            adjustedScreenSpace = new Vector3(screenSpace.x + recalScale, screenSpace.y, screenSpace.z);
            adjustedWorldSpace = camera_targetCam.ScreenToWorldPoint(adjustedScreenSpace);
            object_head.transform.localScale = Vector3.one * (object_head.transform.position - adjustedWorldSpace).magnitude;
        }
    }

    public void CreatePinPoint(Camera cam, Vector3 targetPoint, ConstString.PinType type)
    {
        object_point.transform.position = targetPoint;

        Vector3 setLinePoint = transform.TransformPoint(object_point.transform.localPosition); // local to world
        linerender_line.SetPosition(0, setLinePoint);
        Vector3 endPoint = setLinePoint.normalized * 3f;
        linerender_line.SetPosition(1, endPoint);

        object_head.transform.position = endPoint;
        thisType = type;

        switch (thisType)
        {
            case ConstString.PinType.type_color:
                render_head.material.SetColor("_Color", Color.magenta);
                break;
            case ConstString.PinType.type_position:
                render_head.material.SetColor("_Color", Color.yellow);
                break;
            case ConstString.PinType.type_rotation:
                render_head.material.SetColor("_Color", Color.green);
                break;
            case ConstString.PinType.type_size:
                render_head.material.SetColor("_Color", Color.cyan);
                break;
            default:
                render_head.material.color = Color.white;
                break;
        }

        camera_targetCam = cam;
    }

    public void SetShowHide(bool isShow)
    {
        // recal line points
        if (isShow)
        {
            // line points
            Vector3 setLinePoint = transform.TransformPoint(object_point.transform.localPosition); // local to world
            linerender_line.SetPosition(0, setLinePoint);
            Vector3 endPoint = setLinePoint.normalized * 3f;
            linerender_line.SetPosition(1, endPoint);
        }

        linerender_line.gameObject.SetActive(isShow);
        object_head.SetActive(isShow);
        object_point.SetActive(isShow);
    }

    public void DeleteThisPin()
    {
        SingletonCom.Instance.list_PinPoints.Remove(this);
        Destroy(this.gameObject);
    }
}
