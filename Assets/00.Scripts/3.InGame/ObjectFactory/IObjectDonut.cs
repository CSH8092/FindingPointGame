using System;
using UnityEngine;
using Random = System.Random;

public class IObjectDonut : MonoBehaviour, IObject
{
    public int[] array_diffPoint { get; set; }
    public GameObject Object_Target { get; set; }
    public ObjectHandler ObjectHandler { get; set; }

    public IObjectDonut(GameObject data)
    {
        SetMeshData(data);
        SetRandomPoint(10);
    }

    void SetRandomPoint(int percent)
    {
        array_diffPoint = new int[5] { 0, 0, 0, 0, 0 };

        Random rand = new Random();
        for (int i = 0; i < array_diffPoint.Length; i++)
        {
            // 0 ~ 99 사이의 무작위 숫자를 생성하고 10 미만일 경우 1로 설정
            if (rand.Next(100) < percent)
            {
                array_diffPoint[i] = 1;
                MakeDiffPoint(i);
            }
        }

        Debug.LogFormat("Set {0} {1} {2} {3} {4}", array_diffPoint[0], array_diffPoint[1], array_diffPoint[2], array_diffPoint[3], array_diffPoint[4]);
    }

    void MakeDiffPoint(int caseNum)
    {
        switch (caseNum)
        {
            case 0:
                // 도넛 크림 숨겨짐
                ObjectHandler.object_Parts[1].SetActive(false);
                break;
            case 1:
                // 도넛 크림 회전
                ObjectHandler.object_Parts[1].transform.localRotation = Quaternion.Euler(-90, 0, 180);
                break;
            case 2:
                // 빵 색 다름
                ObjectHandler.ChangeColorRandom(0);
                break;
            case 3:
                // 크림 색 다름
                ObjectHandler.ChangeColorRandom(1);
                break;
            case 4:
                // 도넛이 커짐
                ObjectHandler.gameObject.transform.localScale *= 1.3f;
                break;
            default:
                break;
        }
    }

    void SetMeshData(GameObject data)
    {
        // Set Stage Object
        GameObject model = Instantiate(data);
        model.name = data.name;

        // Set OutLine
        for (int i = 0; i < model.transform.childCount; i++)
        {
            model.transform.GetChild(i).gameObject.AddComponent<Outline>();
        }

        // Save Target Object
        Object_Target = model;

        try
        {
            ObjectHandler = Object_Target.GetComponent<ObjectHandler>();
        }
        catch
        {
            Debug.LogErrorFormat("{0} Data에 Object Handler가 존재하지 않습니다.", data.name);
        }
    }

    public void FadeInOut(bool fadeIn, Action event_end)
    {
        if (fadeIn)
        {
            ObjectHandler.DoFadeIn(event_end);
        }
        else
        {
            ObjectHandler.DoFadeOut(event_end);
        }
    }
}