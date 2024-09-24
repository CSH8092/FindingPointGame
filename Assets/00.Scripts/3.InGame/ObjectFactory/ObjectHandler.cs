using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    [SerializeField]
    public GameObject[] object_Parts;

    public int count_Parts { get; set; }

    private void Start()
    {
        count_Parts = object_Parts.Length;
    }

}
