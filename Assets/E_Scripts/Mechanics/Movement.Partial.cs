using System;
using UnityEngine;

public partial class Movement : MonoBehaviour
{
    [SerializeField] float minDistance = 1.5f;
    [SerializeField] float jumpForce;
    public float mySpeed = 4;
    Rigidbody rb;

    float speedBoost = 10;

    public Vector3 myDir = Vector3.right;
    Vector3? destiny = null;
    Vector3 curDir = Vector3.zero;
    bool canMove = true;
    bool inJump = false;
    [SerializeField] bool isGrounded = false;
    Action callback = null;

    public bool IsGrounded
    {
        get => isGrounded;
        private set
        {
            if (value)
            {
                inJump = false;
            }

            isGrounded = value;
        }
    }

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
        DetectGround();

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
        if (!canMove || inJump) return;

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

    public void Jump()
    {
        if (!isGrounded) return;

        inJump = true;

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

    private bool DetectGround()
    {
        rb = GetComponent<Rigidbody>();
        //Ray ray = new Ray(transform.position, Vector3.down);
        //var hits = Physics.RaycastAll(ray, 3, 1 << 1);
        Vector3 size = new(.5f, .5f, .5f);
        var dir = (transform.position + Vector3.down) - transform.position;

        var hits = Physics.BoxCastAll(transform.position, Vector3.one, Vector3.down, Quaternion.identity, .6f, 1 << 0);

        foreach (var hit in hits)
        {
            if (Vector3.Angle(hit.point, Vector3.up) < 10)
            {
                IsGrounded = true;
                return true;
            }
        }

        IsGrounded= false;
        return false;
    }
}
