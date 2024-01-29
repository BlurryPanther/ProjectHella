using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private int count = 0;
    [SerializeField] private int timer = 3000;

    GameObject player;
    BuffManager buffManager;

    public Vector3 dir;

    private void Awake()
    {
        player = GameObject.Find("Player");
        buffManager = FindObjectOfType<BuffManager>();
    }

    private void OnEnable()
    {
        count = 0;
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

        count++;
        if (count >= timer)
        {
            ObjectPooling.Instance.TurnOffObject(this.gameObject);
            count = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Character>().Damage(1, transform.position);
            //buffManager.SetBuff(other.GetComponent<Character>(), 1);
            ObjectPooling.Instance.TurnOffObject(this.gameObject);
        }
    }
}
