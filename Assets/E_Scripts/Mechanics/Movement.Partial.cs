using System;
using UnityEngine;

public partial class Movement : MonoBehaviour
{
    Rigidbody rb;
    public float mySpeed = 4;
    public Vector3 myDir = Vector3.right;
    Vector3 curDir = Vector3.zero;
    Vector3? destiny = null;
    bool canMove = true;
    Action callback = null;
    [SerializeField] float minDistance = 1.5f;
    float speedBoost = 10;

    void PartialStart()
    {
        myDir = Vector3.right;
    }

    public void MyMove(Vector3 direction)
    {
        if (!canMove) return;

        var newDir = new Vector3()
        {
            x = direction.x,
            y = rb.velocity.y,
            z = 0
        };

        if (newDir.normalized.magnitude == 1)
            myDir = newDir.normalized;

        curDir = newDir.normalized;
    }

    private void FixedUpdate()
    {
        DetectWall();

        if (destiny.HasValue)
        {
            canMove = false;

            if (Vector3.Distance(transform.position, destiny.Value) > minDistance)
            {
                (rb ??= GetComponent<Rigidbody>()).velocity = new Vector3(curDir.x * mySpeed * speedBoost, rb.velocity.y, 0);
            }
            else
            {
                canMove = true;
                destiny = null;
                speedBoost = 1;
                curDir = Vector3.zero;
                print("Finished");
                callback?.Invoke();
                //callback = null;
            }
        }
        else
            (rb ??= GetComponent<Rigidbody>()).velocity = new Vector3(curDir.x * mySpeed, rb.velocity.y, 0);
    }

    public void MoveTo(Vector3 destiny, Action callBack, float speedBoost = 1)
    {
        if (!canMove) return;

        this.destiny = destiny;
        this.callback = callBack;
        this.speedBoost = speedBoost;

        var dir = destiny - transform.position;
        MyMove(dir);
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }

    private void DetectWall()
    {
        rb = GetComponent<Rigidbody>();
        Ray ray = new Ray(transform.position, myDir * 3);
        var hits = Physics.RaycastAll(ray, 3);

        foreach (var hit in hits)
        {
            if (Vector3.Angle(hit.point, myDir) > 175)
            {
                print("Wall detected");
            }
        }
    }
}
