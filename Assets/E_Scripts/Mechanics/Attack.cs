using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        animController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector3 pos)
    {
        /*
        GameObject bullet = ObjectPooling.Instance.RequestBullet();
        bullet.transform.position = pos;
        bullet.GetComponent<bullet>().GetDir();*/
    }
}
