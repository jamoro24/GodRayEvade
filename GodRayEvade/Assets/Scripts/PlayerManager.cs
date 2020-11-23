using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : NetworkedBehaviour
{
    private GameObject mainCamera;
    private GameObject camera1Pos;
    private GameObject camera2Pos;
    private GameObject player1Pos;
    private GameObject player2Pos;
    private GameObject player1Weapon;
    private GameObject player2Weapon;
    public GameObject spotlightPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        camera1Pos = GameObject.Find("Camera1Pos");
        camera2Pos = GameObject.Find("Camera2Pos");
        player1Pos = GameObject.Find("Player1Pos");
        player2Pos = GameObject.Find("Player2Pos");
        player1Weapon = GameObject.Find("Player1Weapon");
        player2Weapon = GameObject.Find("Player2Weapon");
        if (IsServer)
        {
            transform.position = player1Pos.transform.position;
            mainCamera.transform.position = camera1Pos.transform.position;
            mainCamera.transform.rotation = camera1Pos.transform.rotation;
        }
        else
        {
            transform.position = player2Pos.transform.position;
            mainCamera.transform.position = camera2Pos.transform.position;
            mainCamera.transform.rotation = camera2Pos.transform.rotation;
        }

        if (IsServer)
        {
            GameObject go = Instantiate(spotlightPrefab, Vector3.zero, Quaternion.identity);
            go.GetComponent<NetworkedObject>().SpawnWithOwnership(GetComponent<NetworkedObject>().OwnerClientId);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}