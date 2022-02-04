using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MultiPlayerController : MonoBehaviour
{
    public Transform spawnPos;
    public MiniMap cam;
    public GameObject cameraFollow;
    public GameObject[] foodGenerateTarget;
    //amount left of food on map
    private int curAmountOfFood;
    //Max Food appear on map
    private int maxAmountOfFood=100;
    void Start()
    {
        for(int i = 0; i < 100; i++) {
            GenerateFood();
        }
        SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GenerateFood() {
        //
        if (curAmountOfFood < maxAmountOfFood) {
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
            var newFood = Instantiate(foodGenerateTarget[Random.Range(0, foodGenerateTarget.Length)], foodPos,
                          Quaternion.identity);
            newFood.transform.parent = GameObject.Find("Foods").transform;
            curAmountOfFood++;
        }
    }
    private void SpawnPlayers() {
        GameObject player;
        if (PhotonNetwork.IsMasterClient) {
            player= PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SnakeHeadPhoton"),
                           spawnPos.position,
                           spawnPos.rotation
                        );
        } else {
            player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SnakeHeadPhoton"),
                          spawnPos.position+new Vector3(13,0,0),
                          spawnPos.rotation
                       );
        }
        cam.target = player.transform;
        cameraFollow.GetComponent<CameraFollow>().target = player.transform;
        cameraFollow.GetComponent<CameraFollow>().enabled = true;
        Debug.Log("active"+ cameraFollow.GetComponent<CameraFollow>().enabled);
    }
}
