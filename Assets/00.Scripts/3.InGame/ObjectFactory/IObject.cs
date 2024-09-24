using UnityEngine;
using Random = System.Random;

public interface IObject 
{
    int[] array_diffPoint { get; set; }
    GameObject Object_Target { get; set; }

    public void SetRandomPoint(int percent)
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
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    void SetMeshData(GameObject data)
    {

    }
}
