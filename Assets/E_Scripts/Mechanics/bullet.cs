using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    GameObject player;

    public Vector3 dir;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    public Vector3 GetDir()
    {
        dir = player.transform.position - transform.position;
        return dir;
    }

    void Update()
    {
        if (dir != null)
        {
            rb.velocity = (dir * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ObjectPooling.Instance.TurnOffObject(this.gameObject);
        }
    }
}
