using System;
using UnityEngine;
using Random = System.Random;

public class IObjectPudding : MonoBehaviour, IObject
{
    public int[] array_diffPoint { get; set; }
    public GameObject Object_Target { get; set; }
    public ObjectHandler ObjectHandler { get; set; }

    public IObjectPudding(GameObject data)
    {
        SetMeshData(data); 
        SetRandomPoint(20); // 20%
    }

    void SetRandomPoint(int percent)
    {
        array_diffPoint = new int[5] { 0, 0, 0, 0, 0 };

        Random rand = new Random();
        for (int i = 0; i < array_diffPoint.Length; i++)
        {
            // 0 ~ 99 사이의 무작위 숫자를 생성하고 percent 미만일 경우 1로 설정
            if (rand.Next(100) < percent)
            {
                MakeDiffPoint(i);
            }
        }

        //Debug.LogFormat("Set {0} {1} {2} {3} {4}", array_diffPoint[0], array_diffPoint[1], array_diffPoint[2], array_diffPoint[3], array_diffPoint[4]);
    }

    bool flag_cherry = false;
    void MakeDiffPoint(int caseNum)
    {
        switch (caseNum)
        {
            case 0:
                // 체리 숨겨짐
                ObjectHandler.object_Parts[0].SetActive(false);

                flag_cherry = true;
                Debug.LogError("1");
                break;
            case 1:
                // 체리 회전
                if (flag_cherry)
                {
                    Debug.LogError("2");
                    return;
                }

                ObjectHandler.object_Parts[0].transform.localRotation = Quaternion.Euler(-90, 0, 180);
                break;
            case 2:
                // 크림 색 다름
                ObjectHandler.ChangeColorRandom(1);
                break;
            case 3:
                // 푸딩 색 다름
                ObjectHandler.ChangeColorRandom(2);
                break;
            case 4:
                // 푸딩이 커짐
                ObjectHandler.gameObject.transform.localScale *= 1.3f;
                break;
            default:
                break;
        }

        array_diffPoint[caseNum] = 1;
    }

    void SetMeshData(GameObject data)
    {
        // Set Stage Object
        GameObject model = Instantiate(data);
        model.name = data.name;

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

