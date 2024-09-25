using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ObjectHandler : MonoBehaviour
{
    [SerializeField]
    float fadeTime = 1.5f;

    [SerializeField]
    public GameObject[] object_Parts;

    [SerializeField]
    List<Material> list_material = new List<Material>();

    public int count_Parts { get; set; }

    private void Start()
    {
        count_Parts = object_Parts.Length;

        for(int i = 0; i < count_Parts; i++)
        {
            list_material.Add(object_Parts[i].GetComponent<MeshRenderer>().material);
        }

        DoFadeIn();
    }

    public void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Test Code!");
            DoFadeIn();
        }
#endif
    }

    public void DoFadeIn(Action endEvent = null)
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < list_material.Count; i++)
        {
            int index = i;

            // value init
            list_material[index].SetFloat("_Split_Value", 0f);

            // animation start
            sequence.Join(
                DOTween.To(
                    () => list_material[index].GetFloat("_Split_Value"),
                    x => list_material[index].SetFloat("_Split_Value", x),
                    5f,
                    fadeTime
                ).SetEase(Ease.InCubic)
            );
        }

        // for loop end
        sequence.OnComplete(() => endEvent?.Invoke());
    }

    public void DoFadeOut(Action endEvent = null)
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < list_material.Count; i++)
        {
            int index = i;

            // value init
            list_material[index].SetFloat("_Split_Value", 5f);

            // animation start
            sequence.Join(
                DOTween.To(
                    () => list_material[index].GetFloat("_Split_Value"),
                    x => list_material[index].SetFloat("_Split_Value", x),
                    0f,
                    fadeTime
                ).SetEase(Ease.InCubic)
            );
        }

        // for loop end
        sequence.OnComplete(() => endEvent?.Invoke());
    }
}
