using SnakeScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(SpawnPlayerSnakeScript.isFail);
        if(!SpawnPlayerSnakeScript.isFail)
        transform.position = target.position + offset; 
    }
}
