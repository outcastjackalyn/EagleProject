using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType
{
    CAT,
    DUCK,
    PENGUIN,
    SHEEP
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

    private float age = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
