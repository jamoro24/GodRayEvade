using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : NetworkedBehaviour
{
    public float speed = 2f;

    Vector3 move;

    // Update is called once per frame
    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        if(IsOwner)
        {
            if(IsServer)
            {
                transform.Translate(-move * Time.fixedDeltaTime * speed);
            }
            else
            {
                transform.Translate(move * Time.fixedDeltaTime * speed);
            }
        }
    }
}