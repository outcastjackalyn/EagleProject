using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public List<Vector3> points;
    public List<float> pauses;
    public float cornerStart = 3f;
    public float speed = 0.1f;
   // public float cornerSpeed = 0.1f;
    public float pauseTimer = 0.0f;
    public bool move = true;
    private int number = 0;
    private Vector3 current;
    private Vector3 target;
    //private Vector3 nextTarget;
    // Start is called before the first frame update
    void Start()
    {
        number = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (move && pauseTimer <= 0.0f)
        {
            current = transform.position;
            target = points[number];
            float dist = Vector3.Distance(current, target);
            /*if (points.Count > number + 1)
            {
                nextTarget = points[number + 1];
                if (dist < cornerStart)
                {
                    target = Vector3.Lerp(target, nextTarget, cornerSpeed);
                }

            }*/
            if (dist < 0.1f)
            {
                pauseTimer = pauses[number];
                number++;
                if(number >= points.Count)
                {
                    move = false;
                }
            }
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(current, target, speed);
        }
        else
        {
            pauseTimer -= Time.deltaTime;
        }

        
    }
}
