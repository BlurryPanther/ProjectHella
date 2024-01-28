using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Character
{
    [SerializeField] private bool canAttack=false;

    [SerializeField] private int count = 0;
    [SerializeField] private int timer = 500;
    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            count++;

            if (count >= timer)
            {
                CallBullet();
                count = 0;
            }
        }
    }

    void CallBullet()
    {
        GameObject bullet = ObjectPooling.Instance.RequestBullet();
        bullet.transform.position = new Vector3
        {
            x = transform.position.x + 1,
            y = transform.position.y,
            z = 0
        };
        bullet.GetComponent<bullet>().GetDir();
        //attack.Shoot(new Vector3(transform.position.x + 1, transform.position.y, 0));
        //canAttack = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canAttack = true;
        }
    }
}
