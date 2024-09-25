using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class MenuObjectCom : MonoBehaviour
{
    // Mesh
    public MeshFilter mesh_this;
    //public MeshRenderer renderer_this;
    //public MeshCollider collider_this;  

    // Components
    public ObjectHandler objectHandler { get; set; }

    // Values
    [SerializeField]
    float float_rotationSpeed = 50f;

    // Set Animation Value
    float time_playTime = 1f;
    [SerializeField]
    Vector3 scale_highlight = new Vector3(1.3f, 1.3f, 1.3f);


    void Start()
    {
        this.transform.localScale = scale_highlight;
    }

    void Update()
    {

    }

    public void SetMaterial(Material newMaterial)
    {
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            if (g.TryGetComponent<MeshRenderer>(out MeshRenderer material))
            {
                material.material = newMaterial;
            }
        }
    }

    public void SetHighlight(bool isOn)
    {
        if (isOn)
        {
            this.transform.localScale = scale_highlight;
            transform.DOShakeScale(time_playTime).SetEase(Ease.OutElastic);
            objectHandler?.DoFadeIn();

            //Action action = () => transform.DOScale(scale_highlight, time_playTime).SetEase(Ease.OutElastic);
            //objectHandler?.DoFadeIn(action);
        }
        else
        {
            if (DOTween.IsTweening(transform))
            {
                transform.DOKill();
            }
            transform.DOScale(Vector3.one, time_playTime / 2).SetEase(Ease.OutExpo);
        }
    }
}
