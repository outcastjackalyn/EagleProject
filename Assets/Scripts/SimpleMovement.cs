using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public List<Vector3> points;
    public float cornerStart = 3f;
    public float speed = 5f;
    public float cornerSpeed = 5f;
    public bool move = true;
    private int number = 0;
    private Vector3 current;
    private Vector3 target;
    private Vector3 nextTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            current = transform.position;
            target = points[number];
            float dist = Vector3.Distance(current, target);
            if (dist < 0.1f)
            {
                number++;
            }
            if (points.Count >= number + 1)
            {
                nextTarget = points[number + 1];
                if (dist < cornerStart)
                {
                    target = Vector3.Lerp(target, nextTarget, cornerSpeed);
                }

            }
            transform.position = Vector3.Lerp(current, target, speed);
        }

        
    }
}
