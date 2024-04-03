using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Character
{
    GameObject player;
    Vector3 dir = Vector3.right;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        animController.SetBool("isMoving", true);
        player = FindObjectOfType<Player>(true).gameObject;
        col = GetComponent<BoxCollider>();
        attack.EnableDagameOnHit();

        dir = Vector3.right;
    }

    protected override void Update()
    {
        if (!DetectGround())
            ChangeDir();

        movement.MyMove(dir);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    ChangeDir();
    //}

    private void ChangeDir() => dir *= -1;

    private bool DetectGround()
    {
        float dis = col.bounds.extents.y + col.bounds.extents.y * .4f;
        Vector3 start = transform.position + Vector3.up * col.center.y * -col.size.y + dir * col.bounds.extents.x;

        var hits = Physics.RaycastAll(start, Vector3.down, dis);
        Debug.DrawRay(start, Vector3.down * dis, Color.red);

        foreach (var hit in hits) 
        {
            if (hit.collider.GetComponent<Character>())
                continue;

            if (Vector3.Angle(hit.normal, Vector3.up) < 10)
            {
                return true;
            }
        }

        return false;
    }
}
