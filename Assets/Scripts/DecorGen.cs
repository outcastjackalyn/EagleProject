using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorGen : MonoBehaviour
{


    [SerializeField] public List<GameObject> forestDecor;
    [SerializeField] private List<GameObject> parts;
    [SerializeField] private GameObject selected;
    [SerializeField] private GameObject render;


    // Start is called before the first frame update
    void Start()
    {
        selected = forestDecor[Mathf.FloorToInt(Random.Range(0.0f, 20.99f))];
        render = Instantiate(selected, transform);
        Parts(render);
        CreateCollider();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void CreateCollider()
    {
        foreach (GameObject part in parts)
        {
            GameObject collide = new GameObject();
            collide.name = "DecorCollision";
            collide.transform.localPosition = part.transform.position;
            collide.transform.localRotation = part.transform.rotation;
            collide.transform.parent = transform;
            MeshCollider collider = collide.AddComponent<MeshCollider>() as MeshCollider;
            collider.sharedMesh = part.GetComponent<MeshFilter>().sharedMesh;
            collider.convex = true;


        }
    }

    void Parts(GameObject obj)
    {
        if(obj.GetComponent<MeshFilter>() != null)
        {
            parts.Add(obj);
        }
        else
        {
            foreach(Transform child in obj.transform)
            {
                Parts(child.gameObject);
            }
        }
        
    }

}
