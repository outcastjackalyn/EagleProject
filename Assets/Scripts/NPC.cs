using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum NPCType
{
    CAT = 0,
    DUCK = 1,
    PENGUIN = 2,
    SHEEP = 3
}

[System.Serializable]
public enum NPCState
{
    IDLE,
    HUNGRY,
    SLEEPING,

}


public class NPC : MonoBehaviour
{
    [SerializeField] private NPCType type;
    [SerializeField] private NPCState state = NPCState.IDLE;
    [SerializeField] private List<GameObject> types;
    [SerializeField] private GameObject render;
    [SerializeField] private float age = 0.0f;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float hunger = 100.0f;
    [SerializeField] private float stuck = 0.0f;
    [SerializeField] private bool stopped = false;
    [SerializeField] private Vector3 target;
    [SerializeField] private bool foodTarget = false;
    [SerializeField] private float previousDist = 0.0f;
    [SerializeField] private Sun sun;

    // Start is called before the first frame update
    void Start()
    {
        sun = GameObject.Find("Sun").GetComponent<Sun>();
        type = (NPCType)Random.Range(0, 4);
        render = Instantiate(types[(int)type], this.transform);
        gameObject.GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, -0.4f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetReached() || stopped)
        {
            SetTarget();
            stopped = false;
        }
        if (state != NPCState.SLEEPING)
        {
            if (sun.dayTime)
            {
                stuck += Time.deltaTime;
                if (stuck > 3.0f)
                {
                    float change = Vector3.Distance(target, transform.position) - previousDist;
                    if (change > -0.5f)
                    {
                        stopped = true;
                    }
                    else
                    {
                        stopped = false;
                    }
                    stuck = 0.0f;
                    previousDist = Vector3.Distance(target, transform.position);
                }
                else
                {

                    if (hunger < 0.0f)
                    {
                        StartCoroutine(changeState(NPCState.HUNGRY));
                    }
                    else
                    {
                        hunger -= Time.deltaTime;
                    }
                }

            }
            else
            {
                StartCoroutine(changeState(NPCState.SLEEPING));
            }

        }
        else if (sun.dayTime)
        {
            StartCoroutine(changeState(NPCState.IDLE));
        }

    }

    void Fell()
    {
        if (transform.position.y < -3.0f)
        {
            transform.position = new Vector3(transform.position.x, 3.0f, transform.position.z);
        }
    }

    void FixedUpdate()
    {
        if (state == NPCState.IDLE || state == NPCState.HUNGRY)
        {
            Move();
        }
        Fell();
    }

    private IEnumerator changeState(NPCState newState)
    {
        if (newState == state)
        {
            yield break;
        }
        if(stopped && newState == NPCState.HUNGRY)
        {
            state = NPCState.IDLE;
            yield break;
        }
        if (state == NPCState.SLEEPING)
        {
            //stand up
        }
        if (newState == NPCState.SLEEPING)
        {
            //lay down
        }

        if (newState != NPCState.SLEEPING)
        {
            stopped = false;
            SetTarget();
        }
        state = newState;
        yield return new WaitForSeconds(0.2f);
    }

    private void SetTarget()
    {
        if(state == NPCState.HUNGRY && !foodTarget)
        {
            // find closest food transform.pos
            Debug.Log("Quest for lunch!!!");
            findFood(1f);
            foodTarget = true;
            //if stopped maybe try to idle for a while
        }
        else
        {
            //pick random nearby location 
            target = randomPos();
            foodTarget = false;
        }
    }

    Vector3 findFood(float radius)
    {
        Debug.Log("looking in radius: " + radius.ToString());
        LayerMask food = LayerMask.NameToLayer("Food");
        Collider[] found = Physics.OverlapSphere(transform.position, radius, food);
        if (found.Length > 0)
        {
            Debug.Log("found this many: " + found.Length.ToString());
            float dist = 1000f;
            Vector3 pos = found[0].gameObject.transform.position;

            foreach(Collider c in found)
            {
                if(Vector3.Distance(c.transform.position, transform.position) < dist)
                {
                    dist = Vector3.Distance(c.transform.position, transform.position);
                    pos = c.gameObject.transform.position;
                    Debug.Log(c.gameObject.name + " at " + pos);
                }
            }
            return pos;
        }
        else
        {
            Debug.Log("looked in radius: " + radius.ToString());
            return findFood(radius++);
        }
    }


    Vector3 randomPos()
    {
        LayerMask decor = LayerMask.NameToLayer("Decor");
        Vector3 pos;
        float minDist = 10.0f;
        float xDist = Random.Range(-20f, 20f);
        float zDist;
        if (Mathf.Abs(xDist) < minDist)
        {
            minDist -= Mathf.Abs(xDist);
            switch (Random.Range(0, 2))
            {
                case 0:
                    zDist = Random.Range(minDist, 20f);
                    break;
                case 1:
                    zDist = Random.Range(-20f, -minDist);
                    break;
                default:
                    zDist = 0.0f;
                    break;
            }
        }
        else
        {
            zDist = Random.Range(-20f, 20f);
        }
        pos = new Vector3(transform.position.x + xDist, transform.position.y + 0.1f, transform.position.z + zDist);
        Collider[] c = new Collider[] { };
        if (Physics.OverlapSphereNonAlloc(pos, 0.2f, c, decor) > 0)
        {
            return randomPos();
        }
        return pos;
    }

    void checkProgress()
    {

    }

    private bool targetReached()
    {
        Vector3 flatTarget = new Vector3(target.x, 0.0f, target.z);
        Vector3 flatPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        float range = state == NPCState.HUNGRY ? 0.5f : 2.0f;
        if (Vector3.Distance(flatTarget, flatPos) < range)
        {
            StartCoroutine(changeState(NPCState.IDLE));
            return true;
        }
        return false;
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Food")
        {
            hunger += collider.GetComponent<FoodGen>().quality;
            StartCoroutine(collider.GetComponent<FoodGen>().Eat());
        }

    }

    private void Move()
    {
        float step = speed * Time.deltaTime;
        Vector3 flatTarget = new Vector3(target.x, 0.0f, target.z);
        transform.LookAt(flatTarget);
        Vector3 flatishTarget = new Vector3(target.x, 0.4f, target.z);
        transform.position = Vector3.MoveTowards(transform.position, flatishTarget, step);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            transform.parent = collision.transform;
            //Debug.Log("hit floor boss");
            //do something like this for NPC chunk assigning
           // StartCoroutine(Assign(collision.gameObject));
        }
    }
}