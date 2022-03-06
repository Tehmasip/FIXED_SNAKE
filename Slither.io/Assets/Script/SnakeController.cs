using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Photon_Multiplayer_Scripts.Photon.Game_Controllers;
using Photon_Multiplayer_Scripts.Photon.Gameplay_Scripts;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using SnakeScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Runtime.InteropServices;


public class SnakeController : MonoBehaviourPunCallbacks
{
    public GameObject EATEFFECT;
    [DllImport("__Internal")]
    private static extern bool IsMobile();

    public bool isMobile()
    {
    #if !UNITY_EDITOR && UNITY_WEBGL
                 return IsMobile();
    #endif
        return false;
    }
    
    public GameObject CF2Controls;
    public GameObject Food;
    public GameObject BodyPrefeb;
    private Vector3 pointInWorld, mousePosition, direction, pointInWorldForeignLagCompensation = new Vector3();
    public PhotonView photonView;
    //public int snakeWalkSpeed=6;
    public GameObject _multiPlayerCamera;
  //  public GameObject ControlFreak2;
    private readonly float radius = 20.0f;
    public float snakeRunSpeed = 4f; // Called in SnakeRun()

    bool StartEat;
    // Start is called before the first frame update

    public bool StopMove;
    public bool Imune = true;
    public List<Transform> Bodies;
    public Animator Anim;

    public Sprite MyHead;
    public Sprite EnemyHead;
    public SpriteRenderer spriteRenderer;

    public Sprite MyBody;
    public Sprite EnemyBody;

    void Start()
    {
        Imune = true;
        _multiPlayerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //_multiPlayerCamera = transform.GetChild(3).gameObject;
        photonView = GetComponent<PhotonView>(); 

        if (photonView.IsMine)
        {
            spriteRenderer.sprite = MyHead;

            this.photonView.RPC("AddBodyElement", RpcTarget.AllBuffered);


            if (isMobile())
            {
                CF2Controls = GameObject.FindGameObjectWithTag("CF2Canvas");
                CF2Controls.SetActive(true);
            }
            else
            {
                CF2Controls = GameObject.FindGameObjectWithTag("CF2Canvas");
                CF2Controls.SetActive(false);
            }
        }
        else
        {

            spriteRenderer.sprite = EnemyHead;
        }


        Invoke("StartEatF", 0.5f);

        Invoke("ReadyToBattle", 5f);

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }

    void StartEatF()
    {
        StartEat = true;
        Debug.Log("KHANAAAA SHURUUUUUU");
    }

    void ReadyToBattle()
    {

        Debug.Log("BATTLE SHURUUUUUUUU");
        Imune = false;
        Anim.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!StopMove)
        {
            
            if (isMobile())
            {

                MovePlayerInputs();
            }
            else
            {

                MouseControlSnake();
            }

            SnakeMove();
        }
          
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

        transform.position += transform.forward * snakeRunSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
    private void MovePlayerInputs() {
        if (photonView.IsMine)
        {
        float moveHorizontal = ControlFreak2.CF2Input.GetAxisRaw("Horizontal");
        float moveVertical = ControlFreak2.CF2Input.GetAxisRaw("Vertical");
            // direction = transform.position + new Vector3(moveHorizontal, moveVertical, 0);
            mousePosition = new Vector3(transform.position.x + moveHorizontal, transform.position.y + moveVertical, 0);
            direction = Vector3.Slerp(direction, mousePosition - transform.position, Time.deltaTime * 2.5f);
            direction.z = 0;

            pointInWorld = direction.normalized * radius + transform.position;
            transform.LookAt(pointInWorld);
        }
}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Food")
        {
            collision.gameObject.SetActive(false);

            if(collision.gameObject.GetComponent<FoodInfo>().Fnum<100)
            MultiPlayerController.Instance.FoodBool[collision.gameObject.GetComponent<FoodInfo>().Fnum] = true;

            photonView = GetComponent<PhotonView>();
            if (this.photonView.IsMine && StartEat)
            {
                MultiPlayerController.Instance.ScoreI = MultiPlayerController.Instance.ScoreI+5;
                MultiPlayerController.Instance.LengthI++;
                MultiPlayerController.Instance.Score.text = "SCORE : " + MultiPlayerController.Instance.ScoreI;
                if (MultiPlayerController.Instance.ScoreI > PlayerPrefs.GetInt("BestScore")) 
                {
                PlayerPrefs.SetInt("BestScore",  MultiPlayerController.Instance.ScoreI);
                }
                MultiPlayerController.Instance.Length.text = "LENGTH : " + MultiPlayerController.Instance.LengthI;

                if(MultiPlayerController.Instance.LengthI<200)
                this.photonView.RPC("AddBodyElement", RpcTarget.AllBuffered);
            }
            Instantiate(EATEFFECT, collision.transform.position, Quaternion.identity);
            AudioManager.instance.Play("coin");
        }
        else if(collision.gameObject.tag == "Body" && Imune == false)
        {
            if(StartEat == true)
            if(this.photonView.IsMine)
            {
                if (this.photonView.ViewID + "body" != collision.gameObject.name)
                this.photonView.RPC("DestroySnake", RpcTarget.AllBuffered);
            }
        }
        else if (collision.gameObject.tag == "Boundary")
        {
            if (StartEat == true)
                if (this.photonView.IsMine)
                {
                    if (this.photonView.ViewID + "body" != collision.gameObject.name)
                        this.photonView.RPC("DestroySnake", RpcTarget.AllBuffered);
                }
        }
        else if (collision.gameObject.tag == "item")
        {
            collision.gameObject.SetActive(false);
            if (this.photonView.IsMine)
            {
                this.photonView.RPC("BoostSpeed", RpcTarget.All);
            }
        }
    }
    public float size;

    [PunRPC]
    public void AddBodyElement()
    {
        photonView = GetComponent<PhotonView>();
        //  BodyFollow body;

        if(foodcounter>0)
        {
            foodcounter = 0;
            if (Bodies.Count == 0)
            {
                
                /*BodyFollow body = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SnakeBody0900"),
                                    this.gameObject.transform.position + new Vector3(0, 0, 0),
                                    Quaternion.identity).GetComponent<BodyFollow>();*/

                BodyFollow body = Instantiate(BodyPrefeb, this.gameObject.transform.position + new Vector3(0, 0, 0),
                                    Quaternion.identity).GetComponent<BodyFollow>();
                body.snakeController = this;

                body.Target = this.gameObject.transform;

                Bodies.Add(body.gameObject.transform);

                body.gameObject.name = this.photonView.ViewID + "body";

                body.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = Bodies.Count;

                if (photonView.IsMine)
                    body.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = MyBody;
                else
                    body.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = EnemyBody;
            }
            else
            {
                //if (Bodies[0].GetComponent<BodyFollow>().smoothTime < 0.19f)
                if (size < 2.1f)
                    MultiPlayerController.Instance.cameraFollow.GetComponent<CameraFollow>().size = 10f + (float)((float)Bodies.Count/1.2f);
                
                if (size < 2.1f)
                {

                    size = 1f + (float)(Bodies.Count / 10f);
                }

                BodyFollow body = Instantiate(BodyPrefeb, Bodies[Bodies.Count - 1].position,
                                    Quaternion.identity).GetComponent<BodyFollow>();

                body.snakeController = this;

                body.Target = Bodies[Bodies.Count - 1];

                Bodies.Add(body.gameObject.transform);

                body.gameObject.name = this.photonView.ViewID + "body";
                body.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = Bodies.Count;
                this.gameObject.transform.localScale = new Vector3(size, size, size);
                
                //   if (Bodies[0].GetComponent<BodyFollow>().smoothTime < 0.19f)
                if (size < 2.1f)
                {
                    for (int i = 0; i < Bodies.Count; i++)
                    {
                        if (i == 0)
                        {

                            Bodies[i].localScale = new Vector3(size, size, size);
                            Bodies[i].GetComponent<BodyFollow>().smoothTime = 0.1f + ((float)Bodies.Count / 300);
                            Bodies[i].GetComponent<BodyFollow>().smoothTime += (Bodies[i].GetComponent<BodyFollow>().smoothTime / 3f);
                        }
                        else
                        {

                            Bodies[i].localScale = new Vector3(size, size, size);
                            Bodies[i].GetComponent<BodyFollow>().smoothTime = 0.1f + ((float)Bodies.Count / 300);
                        }
                    }

                    snakeRunSpeed = snakeRunSpeed + (float)(((float)Bodies.Count - 1f) / 10f);
                }
                else
                {
                    body.gameObject.transform.localScale = new Vector3(size, size, size);
                    body.GetComponent<BodyFollow>().smoothTime = Bodies[0].GetComponent<BodyFollow>().smoothTime;
                }


                if (photonView.IsMine)
                    body.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = MyBody;
                else
                    body.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = EnemyBody;

                // snakeRunSpeed = snakeRunSpeed + (float)(((float)Bodies.Count - 1f)/10f);
            }
        }
        else
        {
            foodcounter++;
        }
    }
    int foodcounter=1;
    [PunRPC]
    public void DestroySnake()
    {
        StopMove = true;
        if(this.photonView.IsMine)
        _multiPlayerCamera.GetComponent<CameraFollow>().enabled = false;
        int c = Bodies.Count-1;

        for(int i = c; i>=0;i--)
        {
            FoodInfo foodInfo = Instantiate(Food, Bodies[i].position, Quaternion.identity).GetComponent<FoodInfo>();
            foodInfo.Fnum = 200;
            Destroy(Bodies[i].gameObject);
        }

        Bodies.Clear();
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        int c = Bodies.Count - 1;
        for(int i = 0; i <= c; i++)
        {
            Destroy(Bodies[i].gameObject);
        } 
    }

    [PunRPC]
    public void BoostSpeed()
    {
        //snakeWalkSpeed = 12;
        StartCoroutine(BoostTime());
    }
    IEnumerator BoostTime()
    {
        yield return new WaitForSeconds(3);
       // snakeWalkSpeed = 8;
    }
    [PunRPC]
    public void SetWholeBodySpots( Vector3[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            Bodies[i].position = pos[i];
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if (PhotonNetwork.IsMasterClient)
        {
            List<Vector3> BodySpots = new List<Vector3>();

            for(int i = 0;i<Bodies.Count;i++)
            {
                BodySpots.Add(Bodies[i].position);
            }
            this.photonView.RPC("SetWholeBodySpots", RpcTarget.All, BodySpots.ToArray());
        }
    }
}
