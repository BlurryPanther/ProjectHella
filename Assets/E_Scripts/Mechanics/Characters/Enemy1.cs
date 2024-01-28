using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    Movement movement;
    GameObject player;
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        movement = gameObject.GetComponent<Movement>();
        player = GameObject.Find("Player");
        dir = player.transform.position - transform.position;
    }
}
