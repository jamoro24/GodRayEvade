using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : NetworkedBehaviour
{
    private GameObject cameraWrapper;
    private GameObject camera1Pos;
    private GameObject camera2Pos;
    private GameObject player1Pos;
    private GameObject player2Pos;
    public GameObject spotlightPrefab;
    public GameObject weaponPrefab;
    public Material player1Material;
    public Material player2Material;

    private string PLAYER_NAME = "ME";
    private string OTHER_NAME = "THE OTHER";

    // Start is called before the first frame update
    void Start()
    {
        cameraWrapper = GameObject.Find("CameraWrapper");
        camera1Pos = GameObject.Find("Camera1Pos");
        camera2Pos = GameObject.Find("Camera2Pos");
        player1Pos = GameObject.Find("Player1Pos");
        player2Pos = GameObject.Find("Player2Pos");

        if (IsOwner)
        {
            if (IsServer)
            {
                transform.position = player1Pos.transform.position;
                cameraWrapper.transform.position = camera1Pos.transform.position;
                cameraWrapper.transform.rotation = camera1Pos.transform.rotation;
            }
            else
            {
                transform.position = player2Pos.transform.position;
                cameraWrapper.transform.position = camera2Pos.transform.position;
                cameraWrapper.transform.rotation = camera2Pos.transform.rotation;
            }
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
        }

        if (IsServer)
        {
            if (IsOwner)
            {
                GetComponentInChildren<Renderer>().material = player1Material;
                GetComponentInChildren<Text>().text = PLAYER_NAME;
            }
            else
            {
                GetComponentInChildren<Renderer>().material = player2Material;
                GetComponentInChildren<Text>().text = OTHER_NAME;
            }
                
        }
        else
        {
            if (IsOwner)
            {
                GetComponentInChildren<Renderer>().material = player2Material;
                GetComponentInChildren<Text>().text = OTHER_NAME;
            }
            else
            {
                GetComponentInChildren<Renderer>().material = player1Material;
                GetComponentInChildren<Text>().text = PLAYER_NAME;
            }  
        }

        if (IsServer)
        {
            GameObject spotLight = Instantiate(spotlightPrefab, Vector3.zero, Quaternion.identity);
            spotLight.GetComponent<NetworkedObject>().SpawnWithOwnership(GetComponent<NetworkedObject>().OwnerClientId);

            GameObject weapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.identity);
            weapon.GetComponent<NetworkedObject>().SpawnWithOwnership(GetComponent<NetworkedObject>().OwnerClientId);
        }
    }
}