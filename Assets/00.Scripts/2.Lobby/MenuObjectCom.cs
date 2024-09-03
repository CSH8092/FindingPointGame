using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjectCom : MonoBehaviour
{
    // Mesh
    public MeshFilter mesh_this;

    // Set Animation Value
    [SerializeField]
    float time_playTime = 0.8f;
    [SerializeField]
    Vector3 scale_highlight = new Vector3(1.3f, 1.3f, 1.3f);

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void SetMeshData(Mesh newMesh)
    {
        mesh_this.sharedMesh = newMesh;
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
