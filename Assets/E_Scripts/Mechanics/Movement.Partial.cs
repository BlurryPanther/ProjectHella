using System;
using UnityEngine;

public partial class Movement : MonoBehaviour
{
    [SerializeField] float minDistance = 1.5f;

    [Space(10), Header("Status")]
    [SerializeField] Vector3 curDirection = Vector3.right;
    [SerializeField] bool isGrounded = false;
    
    [SerializeField] float speedBoost = 10;
    Vector3? destiny = null;
    Vector3 curDir = Vector3.zero;
    bool canMove = true;
    bool canJump = true;
    Action callback = null;

    public bool IsGrounded
    {
        get => isGrounded;
        private set
        {
            if (value && canJump)
            {
                canJump = true;
                jumpsCount = 0;
            }

            isGrounded = value;
        }
    }
    public Vector3 CurDirection { get => curDirection; set => curDir = value; }

    void PartialStart()
    {
        curDirection = Vector3.right;
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
            curDirection = newDir.normalized;

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
                (rb ??= GetComponent<Rigidbody>()).velocity = new Vector3(curDir.x * speed * speedBoost, rb.velocity.y, 0);
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
            (rb ??= GetComponent<Rigidbody>()).velocity = new Vector3(curDir.x * speed, rb.velocity.y, 0);
    }

    public void MoveTo(Vector3 destiny, Action callBack, float speedBoost = 1)
    {
        if (!canMove || !canJump) return;

        this.destiny = destiny;
        this.callback = callBack;
        this.speedBoost = speedBoost;

        var dir = destiny - transform.position;
        MyMove(dir);
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
        destiny = null;
        curDir = Vector3.zero;
    }

    public void Jump()
    {
        if ((isGrounded && jumpsCount > 0) || (!IsGrounded && jumpsCount > 1) || !canJump) return;

        canJump = false;
        jumpsCount++;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        Invoke("EnableJump", .3f);
    }

    public void Jump(bool basicJump, float holdTime)
    {
        if ((isGrounded && jumpsCount > 0) || (!IsGrounded && jumpsCount > 1) || !canJump) return;

        var jumpForce = jumpsCount == 0 ? GetJumpVelocity(basicJump, holdTime) : this.jumpForce;
        canJump = false;
        jumpsCount++;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        Invoke("EnableJump", .3f);
    }

    private float GetJumpVelocity(in bool basicJump, in float holdTime)
    {
        var minH = basicJump ? minFirstJumpHeight : minSecJumpHeight;
        var maxH = basicJump ? maxFirstJumpHeight : maxSecJumpHeight;
        var gapHeight = maxH - minH;

        var extraHeigh = holdTime * gapHeight;

        return Mathf.Sqrt((minH + extraHeigh) * 2 * Physics.gravity.magnitude);
    }

    private void EnableJump() => canJump = true;

    private void DetectWall()
    {
        rb = GetComponent<Rigidbody>();
        Ray ray = new Ray(transform.position, curDirection * 3);
        var hits = Physics.RaycastAll(ray, 3);

        foreach (var hit in hits)
        {
            if (Vector3.Angle(hit.point, curDirection) > 175)
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
