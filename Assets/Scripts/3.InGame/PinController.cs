using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{
    SingletonCom sc;

    void Start()
    {
        sc = SingletonCom.Instance;
    }

    void Update()
    {
        KeyEvent();
    }

    void KeyEvent()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteAllPinObjects();
        }
    }

    void DeleteAllPinObjects()
    {
        int count = sc.list_PinPoints.Count;
        for (int i = 0; i < count; i++)
        {
            sc.list_PinPoints[i].DeleteThisPin();
        }
    }

    public void ShowHidePinPoints(bool isShow)
    {
        int count = sc.list_PinPoints.Count;
        for (int i = 0; i < count; i++)
        {
            sc.list_PinPoints[i].SetShowHide(isShow);
        }
    }
}
