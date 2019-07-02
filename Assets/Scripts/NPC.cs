using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType
{
    CAT = 0,
    DUCK = 1,
    PENGUIN = 2,
    SHEEP = 3
}

public enum NPCState
{
    IDLE,
    HUNGRY,
    SLEEPING,
    SCARED,

}


public class NPC : MonoBehaviour
{
    private NPCType type;
    private NPCState state = NPCState.IDLE;
    private List<GameObject> types;
    private GameObject render;
    private float age = 0.0f;
    private float hunger = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        type = (NPCType) Mathf.FloorToInt(Random.Range(0.0f, 3.99f));
        render = Instantiate(types[(int) type], this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void changeState(NPCState current)
    {

    }

}
