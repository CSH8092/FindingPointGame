using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjectCom : MonoBehaviour
{
    // Mesh
    Mesh mesh_this;

    // Set Animation Value
    [SerializeField]
    float time_playTime = 0.8f;
    [SerializeField]
    Vector3 scale_highlight = new Vector3(1.3f, 1.3f, 1.3f);

    void Start()
    {
        mesh_this = this.GetComponent<Mesh>();
    }

    void Update()
    {
        
    }

    public void SetMeshData(Mesh newMesh)
    {
        mesh_this = newMesh;
    }

    public void SetHighlight(bool isOn)
    {
        if (isOn)
        {
            transform.DOScale(scale_highlight, time_playTime).SetEase(Ease.OutElastic);
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
