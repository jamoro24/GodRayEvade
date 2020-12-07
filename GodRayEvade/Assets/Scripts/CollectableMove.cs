using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float RotSpeed = 50f;
    public float MovementY = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, Mathf.Sin(Time.time)/MovementY, 0f);
        transform.Rotate(0f, RotSpeed * Time.deltaTime, 0f);
    }
}