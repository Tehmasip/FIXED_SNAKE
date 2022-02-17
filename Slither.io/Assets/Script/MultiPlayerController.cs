using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MultiPlayerController : MonoBehaviourPunCallbacks
{
    public GameObject[] FoodList;
    public int Time = 10;
    public Text Score;
    public Text Length;
    public Text PlayerName;

    public int ScoreI;
    public int LengthI;
    

    public PhotonView photonView;
    public Transform spawnPos;
    public MiniMap cam;
    public GameObject cameraFollow;
    public MiniMap MiniCam;
    public GameObject[] foodGenerateTarget;
    //amount left of food on map
    private int curAmountOfFood;

    //Max Food appear on map
    private int maxAmountOfFood=100;
    public List<Vector3> FoodSpots;
    public bool[] FoodBool;
    public bool FoodEnter;

    public static MultiPlayerController Instance;


    private void Awake()
    {
        if(Instance == null)
           Instance = this;
    }

    void Start()
    {
        if (AudioManager.instance.CheckPlay("MenuBG"))
        {
            AudioManager.instance.Stop("MenuBG");
            AudioManager.instance.Play("GamePlayBG");
        }
        else
        {
            AudioManager.instance.Play("GamePlayBG");
        }

        photonView = this.GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            GenerateFood();
        }

        PlayerName.text = PhotonNetwork.NickName;
        Score.text = "SCORE : " + ScoreI;
        Length.text = "LENGTH : " + LengthI;
        SpawnPlayers();


        InvokeRepeating("ResetFood", 20, 20);
    }


    [PunRPC]
    public void ResetFoodCall()
    {
        for (int i = 0; i < FoodSpots.Count; i++)
        {
            FoodBool[i] = false;
            FoodList[i].SetActive(true);
        }
    }

    //[PunRPC]
    public void ResetFood()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            photonView.RPC("ResetFoodCall", RpcTarget.All);
        }
    }
    private void GenerateFood()
    {
        for(int i = 0; i < 100; i++)
        {
            if(curAmountOfFood < maxAmountOfFood) 
            {
                var r = Random.Range(0, 4);

                Vector3 foodPos;

                if (r == 0)
                    foodPos = new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), 0);
                else if (r <= 1)
                    foodPos = new Vector3(Random.Range(-60, 60), Random.Range(-60, 60), 0);
                else if (r <= 2)
                    foodPos = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), 0);
                else
                    foodPos = new Vector3(Random.Range(-120, 120), Random.Range(-120, 120), 0);

                    FoodSpots[i] = foodPos;
            }
        }
        this.photonView.RPC("FoodSpawner", RpcTarget.All ,FoodSpots.ToArray());
    }

    [PunRPC]
    public void FoodSpawner (Vector3[] arr)
    {
        if (FoodEnter == false)
        {
            FoodEnter = true;
            for (int i = 0; i < arr.Length; i++)
            {
                var newFood = Instantiate(foodGenerateTarget[Random.Range(1, 4)], arr[i], Quaternion.identity);
                newFood.transform.parent = GameObject.Find("Foods").transform;
                FoodList[i] = newFood.gameObject;
                newFood.GetComponent<FoodInfo>().Fnum = i;
                curAmountOfFood++;
                FoodSpots [i] = arr[i];
                FoodBool [i] = false;

            }
        }
    }

    [PunRPC]
    public void AgainSpawnFood(Vector3[] arr, bool[] Fbools)
    {
        if (FoodEnter == false)
        {
            FoodEnter = true;

            Debug.Log("PAPA KI PAR AGAIN");
            for (int i = 0; i < arr.Length; i++)
            {
                var newFood = Instantiate(foodGenerateTarget[Random.Range(1, 4)], arr[i],
                              Quaternion.identity);
                newFood.transform.parent = GameObject.Find("Foods").transform;

                newFood.GetComponent<FoodInfo>().Fnum = i;
                curAmountOfFood++;
                FoodSpots[i] = arr[i];
                FoodList[i] = newFood.gameObject;
                FoodBool[i] = Fbools[i];

                if (Fbools[i] == true)
                {
                    newFood.SetActive(false);
                }
            }
        }
    }

    public GameObject player;
    private void SpawnPlayers() 
    {
        if (PhotonNetwork.IsMasterClient) {
            player= PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SnakeHeadPhoton"),
                           spawnPos.position,
                           spawnPos.rotation);
        } else {
            player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SnakeHeadPhoton"),
                          spawnPos.position+new Vector3(10,0,0),
                          spawnPos.rotation);
        }
        cam.target = player.transform;
        cameraFollow.GetComponent<CameraFollow>().target = player.transform;
        MiniCam.target = player.transform;
        MiniCam.enabled = true;
        cameraFollow.GetComponent<CameraFollow>().enabled = true;
        Debug.Log("active"+ cameraFollow.GetComponent<CameraFollow>().enabled);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.IsMasterClient)
        {
            this.photonView.RPC("AgainSpawnFood", RpcTarget.All, FoodSpots.ToArray(),FoodBool);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

    }
}
