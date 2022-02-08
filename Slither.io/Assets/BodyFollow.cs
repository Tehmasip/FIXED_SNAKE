using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFollow : MonoBehaviour
{
    public Transform Target;
    public float smoothTime = 0.2f;
    private Vector3 movementVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Target!=null)
        {

            transform.position = Vector3.SmoothDamp(transform.position,
                     Target.position, ref movementVelocity, smoothTime);
        }
    }
}
