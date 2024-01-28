using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement = gameObject.GetComponent<Movement>();
        //movement.Start();
    }

    // Update is called once per frame
    void Update()
    {
        movement.Move(gameObject.GetComponent<Rigidbody>());
    }
}
