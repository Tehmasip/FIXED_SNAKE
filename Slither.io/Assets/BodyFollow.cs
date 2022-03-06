using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFollow : MonoBehaviour
{
    public SnakeController snakeController;
    public Animator Anim;
    public Transform Target;
    public float smoothTime;
    private Vector3 movementVelocity;

    // Start is called before the first frame update
    Collider col;
    void Start()
    {
        
        col = this.gameObject.GetComponent<Collider>();

        if(snakeController.Imune == true)
        {
            col.enabled = false;
            Anim.enabled = true;
            Invoke("CheckHead", 5.5f);
        }
    }

    void CheckHead()
    {
        while(snakeController.Imune == true)
        {
           
        }
        col.enabled = true;
        Anim.enabled = false;
        Anim.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Target!=null)
        {
          /*  if( Vector3.Distance(this.transform.position , Target.transform.position) > 30 * smoothTime)
            {
                col.enabled = false;

                transform.position = Target.position;
            }
            else
            {*/
                //col.enabled = true;
                transform.position = Vector3.SmoothDamp(transform.position,
                         Target.position, ref movementVelocity, smoothTime);
           // }

        }
    }
}
