using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;
    void LateUpdate()
    {
        cam = GameObject.Find("MainCamera").transform;
        transform.LookAt(transform.position + cam.forward);
    }
}
