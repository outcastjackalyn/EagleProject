using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGen : MonoBehaviour
{
    [SerializeField] public List<GameObject> fruits;
    [SerializeField] private GameObject selected;
    [SerializeField] private GameObject render;
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private Chunk chunk;
    [SerializeField] private bool frozen = true;
    [SerializeField] private bool hit = false;
    private float scale = 3f;
    public float quality;

    [SerializeField] private float lifetime = 5.0f;

    public void SetChunk(Chunk c)
    {
        this.chunk = c;
    }


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        int i = Random.Range(0,9);
        SetConstraints(frozen);
        selected = fruits[i];
        quality = Random.Range(30.0f, 60.0f);
        render = Instantiate(selected, transform);
        render.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);
        //Parts(render);
        render.transform.localScale += new Vector3(scale, scale, scale);
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
            if(this.frozen)
            {
                if(chunk.terrain.activeInHierarchy)
                {
                    this.frozen = false;
                    SetConstraints(frozen);
                }
            }
            else
            {
                lifetime -= Time.deltaTime;
            }
        }
    }


    void SetConstraints(bool f)
    {
        if (f)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            //Debug.Log("hit floor boss");
            //do something like this for NPC chunk assigning
            StartCoroutine(Assign(collision.gameObject));
        }

        /*if (collision.gameObject.tag == "Animal")
        {
            StartCoroutine(Eat(collision.gameObject));
        }*/
        if (collision.gameObject.tag == "Decor")
        {
            hit = true;
            if (collision.transform.parent.name != "DecorHolder")
            {
                Debug.Log("hit something boss");
                //StartCoroutine(Assign(collision.transform.parent.gameObject));
            }
        }
    }


    void CreateCollider()
    {
        GameObject collide = new GameObject();
        collide.name = "FoodCollision";
        collide.layer = 17;
        collide.transform.localPosition = render.transform.position;
        collide.transform.localRotation = render.transform.rotation;
        collide.transform.parent = transform;
        MeshCollider collider = collide.AddComponent<MeshCollider>() as MeshCollider;
        collider.convex = true;
        collider.sharedMesh = render.GetComponent<MeshFilter>().sharedMesh;
        collider.transform.localScale += new Vector3(scale, scale, scale);
    }

   /* void Parts(GameObject obj)
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
        
    }*/

    IEnumerator Assign(GameObject obj)
    {
        //Debug.Log("assign to chunk " + obj.name);
        yield return new WaitForSeconds(0.5f);
        if (rigidbody != null)
        {
            if (rigidbody.angularVelocity.magnitude > 0.2)
            {
                yield return new WaitForSeconds(0.5f);
            }
            render.SetActive(true);
            Destroy(rigidbody);
            this.transform.parent = obj.transform;
            gameObject.GetComponentInChildren<MeshCollider>().isTrigger = true;
        }
        yield return true;
    }




    //do something else 
    public IEnumerator Eat()
    {
        //Debug.Log("assign to chunk " + obj.name);
        yield return new WaitForSeconds(0.5f);
        // get eatsed
        yield return true;
    }
}
