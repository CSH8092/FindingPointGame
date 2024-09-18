using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MenuObjectCom : MonoBehaviour
{
    // Mesh
    public MeshFilter mesh_this;
    public MeshRenderer renderer_this;
    public MeshCollider collider_this;  

    // Set Animation Value
    [SerializeField]
    float time_playTime = 0.8f;
    [SerializeField]
    float float_rotationSpeed = 50f;
    [SerializeField]
    Vector3 scale_highlight = new Vector3(1.3f, 1.3f, 1.3f);

    [Header("Random Values")]
    int[] array_diffPoint = new int[5] { 0, 0, 0, 0, 0 };

    void Start()
    {

    }

    void Update()
    {
        //RotateObject(); // -> 해당 옵션 키면 MenuController.cs - MenuArrange() 다시 계산해줘야함 일단 보류
    }

    void RotateObject()
    {
        transform.Rotate(0f, float_rotationSpeed * Time.deltaTime, 0f);  // Y축 기준으로 회전
    }

    public void SetMeshData(Mesh newMesh)
    {
        mesh_this.sharedMesh = newMesh;
        collider_this.sharedMesh = newMesh;
    }

    public void SetMaterial(Material newMaterial)
    {
        renderer_this.material = newMaterial;
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

    public void SetRandomPoint()
    {
        Random rand = new Random();
        for (int i = 0; i < array_diffPoint.Length; i++)
        {
            // 0 ~ 99 사이의 무작위 숫자를 생성하고 10 미만일 경우 1로 설정
            if (rand.Next(100) < 10)
            {
                array_diffPoint[i] = 1;
            }
        }

        Debug.LogFormat("Set {0} {1} {2} {3} {4}", array_diffPoint[0], array_diffPoint[1], array_diffPoint[2], array_diffPoint[3], array_diffPoint[4]);
    }
}
