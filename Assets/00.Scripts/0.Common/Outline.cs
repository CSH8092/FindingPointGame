using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Outline : MonoBehaviour
{
    Material material_outline;

    GameObject object_Target;
    List<Renderer> list_render = new List<Renderer>();

    private void Awake()
    {
        material_outline = Instantiate(Resources.Load<Material>(@"Materials/Outline"));

        object_Target = this.gameObject;
        if (this.transform.childCount != 0)
        {
            object_Target = this.transform.GetChild(0).gameObject;
        }
    }

    void Start()
    {
        foreach (Renderer r in object_Target.transform.GetComponentsInChildren<Renderer>())
        {
            list_render.Add(r);
        }
    }

    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (!SingletonCom.Instance.isObservationMode)
            {
                for (int i = 0; i < list_render.Count; i++)
                {
                    var materials = list_render[i].sharedMaterials.ToList();
                    materials.Add(material_outline);
                    list_render[i].materials = materials.ToArray();
                }
            }
        }
    }

    private void OnMouseExit()
    {
        for (int i = 0; i < list_render.Count; i++)
        {
            var materials = list_render[i].sharedMaterials.ToList();
            materials.Remove(material_outline);
            list_render[i].materials = materials.ToArray();
        }
    }
}
