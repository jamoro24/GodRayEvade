using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightManager : NetworkedBehaviour
{
    private GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            mainCamera = GameObject.Find("MainCamera");
            transform.position = mainCamera.transform.position;

        }

        if (IsServer)
        {
            if (IsOwner)
                GetComponentInChildren<Light>().color = new Color(1f, 0f, 0f);
            else
                GetComponentInChildren<Light>().color = new Color(0f, 1f, 0f);
        }
        else
        {
            if (IsOwner)
                GetComponentInChildren<Light>().color = new Color(0f, 1f, 0f);
            else
                GetComponentInChildren<Light>().color = new Color(1f, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            transform.rotation = mainCamera.transform.rotation;
        }

    }
}