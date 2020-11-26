using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeaponManager : NetworkedBehaviour
{
    private NavMeshAgent agent;

    private GameObject player1Weapon;
    private GameObject player2Weapon;

    public Material player1Material;
    public Material player2Material;

    private GameObject spotLight;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player1Weapon = GameObject.Find("Player1Weapon");
        player2Weapon = GameObject.Find("Player2Weapon");

        if (IsOwner)
        {
            if (IsServer)
            {
                transform.position = player1Weapon.transform.position;
            }
            else
            {
                transform.position = player2Weapon.transform.position;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
        {
            if (spotLight != null)
            {
                RaycastHit hit;
                int layerMask = 1 << 10;
                if (Physics.Raycast(spotLight.transform.position, spotLight.transform.forward, out hit, Mathf.Infinity, layerMask))
                {
                    agent.SetDestination(hit.point);
                }
            }
            else
            {
                SpotLightManager[] spotlights = GameObject.FindObjectsOfType<SpotLightManager>();
                foreach(SpotLightManager tSpotLight in spotlights)
                {
                    if(tSpotLight.IsOwner)
                    {
                        spotLight = tSpotLight.gameObject;
                    }
                }
            }
        }
    }
}