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

    void Start()
    {
        if (IsServer)
        {
            agent = GetComponent<NavMeshAgent>();

            player1Weapon = GameObject.Find("Player1Weapon");
            player2Weapon = GameObject.Find("Player2Weapon");

            if (IsOwner)
            {
                transform.position = player1Weapon.transform.position;
            }
            else
            {
                transform.position = player2Weapon.transform.position;
            }
                
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
        if(IsServer)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}