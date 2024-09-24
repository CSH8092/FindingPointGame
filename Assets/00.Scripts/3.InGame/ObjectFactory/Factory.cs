using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public enum ObjectType
    {
        pudding,
        donut,
    }

    public static IObject CreateObject(ObjectType type, GameObject data)
    {
        switch (type)
        {
            case ObjectType.pudding:
                return new IObjectPudding(data);
            case ObjectType.donut:
                return new IObjectPudding(data);
            default:
                return new IObjectPudding(data);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
