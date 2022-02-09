using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFollow : MonoBehaviour
{
    public Transform Target;
    public float smoothTime = 0.2f;
    private Vector3 movementVelocity;
    // Start is called before the first frame update
    Collider col;
    void Start()
    {
        col = this.gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Target!=null)
        {
            if( Vector3.Distance(this.transform.position , Target.transform.position) > 1.5 )
            {
                col.enabled = false;

                transform.position = Target.position;
            }
            else
            {
                col.enabled = true;
                transform.position = Vector3.SmoothDamp(transform.position,
                         Target.position, ref movementVelocity, smoothTime);
            }

        }
    }
}
