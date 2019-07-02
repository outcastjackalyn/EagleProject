using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Light light;


    float totalRotation = 360;
    [SerializeField] private float totalTime = 60;            // seconds to take to rotate totalRotation degrees
    [SerializeField] private float currentRotationTime = 0;   // current elapsed time for the rotation
    public bool dayTime = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        light = gameObject.GetComponentInChildren<Light>();

    }

    void Update()
    {
        if (currentRotationTime > totalTime / 2)
        {
            dayTime = false;
        }
        else
        {
            dayTime = true;
        }
        light.enabled = dayTime;
    }


        // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.transform.position;
        //transform.rotation =
        
        // what percentage are we toward the goal?
        float t = currentRotationTime / totalTime;

        // interpolate the number of degrees between 0 and totalRotation, using t
        float currentRotationAmount = Mathf.Lerp(0, totalRotation, t);


        // set the rotation to that number of degrees
        transform.rotation = Quaternion.Euler(currentRotationAmount, 0, 0);
        
        currentRotationTime += Time.deltaTime;
        if(currentRotationTime > totalTime)
        {
            currentRotationTime -= totalTime;
        }

    }
}
