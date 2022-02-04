using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Vector3 pointInWorld, mousePosition, direction, pointInWorldForeignLagCompensation = new Vector3();
    private PhotonView photonView;
    private int snakeWalkSpeed=2;
    public GameObject _multiPlayerCamera;
  //  public GameObject ControlFreak2;
    private readonly float radius = 20.0f;
    private readonly float snakeRunSpeed = 7.0f; // Called in SnakeRun()
    // Start is called before the first frame update
    void Start()
    {
        _multiPlayerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //_multiPlayerCamera = transform.GetChild(3).gameObject;
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseControlSnake();
        SnakeMove();
       // MovePlayerInputs();
    }
    private void MouseControlSnake() {
       // if (isPhotonPlayer == false) {
            //var ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>()
            //    .ScreenPointToRay(c);
            //RaycastHit hit; // Store the first obj touched by ray
            //Physics.Raycast(ray, out hit, 50.0f); // The third parameter is the max distance
            //mousePosition = new Vector3(hit.point.x, hit.point.y, 0);
            //direction = Vector3.Slerp(direction, mousePosition - transform.position, Time.deltaTime * 2.5f);
            //direction.z = 0;
            ////pointInWorld = direction.normalized * radius + transform.position;
            //transform.LookAt(pointInWorld);
         if (photonView.IsMine) {
            Debug.Log("mine");
            var ray = _multiPlayerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; // Store the first obj touched by ray
            Physics.Raycast(ray, out hit, 50.0f); // The third parameter is the max distance
            mousePosition = new Vector3(hit.point.x, hit.point.y, 0);
            direction = Vector3.Slerp(direction, mousePosition - transform.position, Time.deltaTime * 2.5f);
            direction.z = 0;
            pointInWorld = direction.normalized * radius + transform.position;
            transform.LookAt(pointInWorld);
       }
    }
    private void SnakeMove() {
        transform.position += transform.forward * snakeWalkSpeed * Time.deltaTime;
    }
    private void MovePlayerInputs() {
        Debug.Log("here");
        float moveHorizontal = ControlFreak2.CF2Input.GetAxisRaw("Horizontal");
        float moveVertical = ControlFreak2.CF2Input.GetAxisRaw("Vertical");
        Vector3 v3 = new Vector3(moveHorizontal, moveVertical, 1.0f);
        Quaternion qTo = Quaternion.LookRotation(v3);
        gameObject.transform.LookAt(v3);
        //gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, qTo, 50 * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, qTo, 50 * Time.deltaTime);
    }
}
