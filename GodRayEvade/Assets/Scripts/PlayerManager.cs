﻿using MLAPI;
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
    public GameObject spotlightPrefab;
    public GameObject weaponPrefab;
    public Material player1Material;
    public Material player2Material;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        camera1Pos = GameObject.Find("Camera1Pos");
        camera2Pos = GameObject.Find("Camera2Pos");
        player1Pos = GameObject.Find("Player1Pos");
        player2Pos = GameObject.Find("Player2Pos");
        
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
            if (IsOwner)
                GetComponentInChildren<Renderer>().material = player1Material;
            else
                GetComponentInChildren<Renderer>().material = player2Material;
        }
        else
        {
            if (IsOwner)
                GetComponentInChildren<Renderer>().material = player2Material;
            else
                GetComponentInChildren<Renderer>().material = player1Material;
        }

        if (IsServer)
        {
            GameObject spotLight = Instantiate(spotlightPrefab, Vector3.zero, Quaternion.identity);
            spotLight.GetComponent<NetworkedObject>().SpawnWithOwnership(GetComponent<NetworkedObject>().OwnerClientId);

            GameObject weapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.identity);
            weapon.GetComponent<NetworkedObject>().Spawn();
            weapon.GetComponent<WeaponManager>().asignSpotLight(spotLight);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}