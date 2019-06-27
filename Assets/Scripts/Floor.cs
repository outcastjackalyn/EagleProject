using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{

    public float stayLoaded = 5.0f;
    public void Reload()
    {
        stayLoaded = 5.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stayLoaded -= Time.deltaTime;
    }
}
