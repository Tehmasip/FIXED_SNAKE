using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float size=10;
    public Camera cam;

    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }
    void Update()
    {
        if(target !=null)
        transform.position = target.position+offset;


    }
    private void FixedUpdate()
    {
       
       cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, size, 1 * Time.deltaTime);
    }
}
