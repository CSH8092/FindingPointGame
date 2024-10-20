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
            // 0 ~ 99 ������ ������ ���ڸ� �����ϰ� percent �̸��� ��� 1�� ����
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
                // ü�� ������
                ObjectHandler.object_Parts[0].SetActive(false);

                flag_cherry = true;
                Debug.LogError("1");
                break;
            case 1:
                // ü�� ȸ��
                if (flag_cherry)
                {
                    Debug.LogError("2");
                    return;
                }

                ObjectHandler.object_Parts[0].transform.localRotation = Quaternion.Euler(-90, 0, 180);
                break;
            case 2:
                // ũ�� �� �ٸ�
                ObjectHandler.ChangeColorRandom(1);
                break;
            case 3:
                // Ǫ�� �� �ٸ�
                ObjectHandler.ChangeColorRandom(2);
                break;
            case 4:
                // Ǫ���� Ŀ��
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
            Debug.LogErrorFormat("{0} Data�� Object Handler�� �������� �ʽ��ϴ�.", data.name);
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

