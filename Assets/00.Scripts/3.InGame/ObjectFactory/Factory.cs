using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public enum ObjectType
    {
        pudding,
        donut,
        pencil,
        berry,

    }

    public static IObject CreateObject(ObjectType type, GameObject data)
    {
        switch (type)
        {
            case ObjectType.pudding:
                return new IObjectPudding(data);
            case ObjectType.donut:
                return new IObjectDonut(data);
            case ObjectType.pencil:
                return new IObjectPencil(data);
            case ObjectType.berry:
                return new IObjectBerry(data);
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
