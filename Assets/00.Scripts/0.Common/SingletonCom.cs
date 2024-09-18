using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonCom
{
    public List<PinPoint> list_PinPoints = new List<PinPoint>();

    public bool isMenuArrange = false;
    public bool isObservationMode = false;

    public int curr_StageNum = 0;

    private SingletonCom() { }

    private static SingletonCom instance = null;
    public static SingletonCom Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new SingletonCom();
            }
            return instance;
        }
    }
}
