using System;
using UnityEngine;
using Random = System.Random;

public interface IObject 
{
    int[] array_diffPoint { get; set; }
    GameObject Object_Target { get; set; }
    ObjectHandler ObjectHandler { get; set; }

    public void SetRandomPoint(int percent)
    {

    }

    void MakeDiffPoint(int caseNum)
    {

    }

    void SetMeshData(GameObject data)
    {

    }

    void FadeInOut(bool fadeIn, Action event_end)
    {

    }
}
