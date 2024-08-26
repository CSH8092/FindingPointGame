using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStageMenu : MonoBehaviour
{
    [Header("Target Object")]
    public GameObject object_ParentMenu;
    public GameObject prefab_MenuButton;
    public int count_menu = 6;
    [SerializeField]
    float value_farDistance = 2f;

    // Values
    float angle;
    float x, y, z;

    void Start()
    {
        CreateMenu();
    }

    void Update()
    {
        
    }

    void CreateMenu()
    {
        for(int i = 0; i < count_menu; i++)
        {
            angle = i * Mathf.PI * 2 / count_menu;
            x = Mathf.Cos(angle) * value_farDistance;
            z = Mathf.Sin(angle) * value_farDistance;
            Vector3 new_position = new Vector3(x, 0, z);
            GameObject menuObject = Instantiate(prefab_MenuButton, object_ParentMenu.transform);
            menuObject.transform.position = new_position;
            menuObject.transform.LookAt(object_ParentMenu.transform);
            menuObject.name = string.Format("Menu Button {0}", i);
        }
    }

    void ReadNSetData()
    {
        // 1. menu button 마다 stage mesh 추가
        // 2. Json 등의 파일 읽어와서 이미 unlock한 stage 처리 필요 -> material 설정
    }
}
