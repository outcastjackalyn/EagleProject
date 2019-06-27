using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorGen : MonoBehaviour
{


    [SerializeField] public List<GameObject> forestDecor;
    [SerializeField] private List<GameObject> parts;
    [SerializeField] private GameObject selected;
    [SerializeField] private GameObject render;
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private Chunk chunk;
    [SerializeField] private bool rocky = false;
    [SerializeField] private bool tree = false;
    [SerializeField] private bool frozenY = true;
    [SerializeField] private bool hit = false;


    [SerializeField] private float lifetime = 2.0f;

    public void SetChunk(Chunk c)
    {
        this.chunk = c;
    }


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        int i = Mathf.FloorToInt(Random.Range(0.0f, 20.99f));
        if (i < 9)
        {
            rocky = true;
            this.transform.Translate(0f, 0.5f, 0f);

        }
        if(i > 17)
        {
            tree = true;
        }
        SetConstraints(rocky, frozenY);
        selected = forestDecor[i];
        render = Instantiate(selected, transform);
        render.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);
        Parts(render);
        CreateCollider();
        render.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(lifetime < 0.0f && transform.parent.name == "DecorHolder")
        {
            Destroy(gameObject);
        }
        if (rigidbody != null)
        {
            if(rigidbody.velocity.y > 0.1)
            {
                rigidbody.velocity = new Vector3(0, -0.1f, 0);
            } 
            if(this.frozenY)
            {
                if(chunk.terrain.activeInHierarchy)
                {
                    this.frozenY = false;
                    SetConstraints(rocky, frozenY);
                }
            }
            else
            {
                lifetime -= Time.deltaTime;
            }
        }
    }


    void SetConstraints(bool rock, bool y)
    {
        if (rock)
        {
            rigidbody.constraints = y ? RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY
                : RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX ;
        }
        else
        {
            rigidbody.constraints = y ? RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY
                | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY 
                : RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX 
                | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            //do something like this for NPC chunk assigning
            StartCoroutine(Assign(collision.gameObject));
        }
        if (collision.gameObject.tag == "Decor")
        {
            hit = true;
            if (collision.transform.parent.name != "DecorHolder")
            {
                StartCoroutine(Assign(collision.transform.parent.gameObject));
            }
        }
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
            collider.convex = true;
            collider.sharedMesh = part.GetComponent<MeshFilter>().sharedMesh;


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

    IEnumerator Assign(GameObject obj)
    {
        //Debug.Log("assign to chunk " + obj.name);
        yield return new WaitForSeconds(0.5f);
        if (rigidbody != null)
        {
            if (rigidbody.angularVelocity.magnitude > 1)
            {
                yield return new WaitForSeconds(0.5f);
            }
            render.SetActive(true);
            Destroy(rigidbody);
            if(tree)
            {
                this.transform.Translate(0f, -0.5f, 0f);
            }
            this.transform.parent = obj.transform;
        }
        yield return true;
    }

}
