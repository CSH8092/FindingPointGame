using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinPoint : MonoBehaviour
{
    public GameObject object_head;
    public LineRenderer linerender_line;
    public GameObject object_point;

    void Start()
    {
        //linerender_line.SetPosition(0, Vector3.zero);
        //linerender_line.SetPosition(1, object_head.transform.position);
    }

    void Update()
    {
        
    }

    public void SetPinPosition(Vector3 targetPoint)
    {
        object_point.transform.position = targetPoint;

        Vector3 setLinePoint = transform.TransformPoint(object_point.transform.localPosition); // local to world
        linerender_line.SetPosition(0, setLinePoint);
        Vector3 endPoint = setLinePoint.normalized * 3f;
        linerender_line.SetPosition(1, endPoint);

        object_head.transform.position = endPoint;
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
}
