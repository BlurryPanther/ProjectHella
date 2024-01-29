using System;
using UnityEngine;

public partial class Movement : MonoBehaviour
{
    [SerializeField] float minDistance = 1.5f;

    [Space(10), Header("Status")]
    [SerializeField] Vector3 curDirection = Vector3.right;
    
    [SerializeField] float speedBoost = 10;
    [SerializeField] int startDir = -1;
    Vector3? destiny = null;
    Vector3 curDir = Vector3.zero;
    bool canMove = true;
    Action callback = null;

    [Space(), Header("Camera"), SerializeField]
    GameObject cameraTarget;
    [SerializeField] float xOffset;

    public float minFirstJumpHeight = 4;
    public float maxFirstJumpHeight = 7;
    public float minSecJumpHeight = 2;
    public float maxSecJumpHeight = 4;
    protected SpriteRenderer charSrite;

    public Vector3 CurDirection { get => curDirection; set => curDir = value; }

    void PartialStart()
    {
        curDirection = Vector3.right;
        charSrite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void MyMove(Vector3 direction)
    {
        if (!canMove) return;

        var newDir = new Vector3()
        {
            x = direction.x switch { < 0 => -1, > 0 => 1, _ => 0},
            y = rb.velocity.y,
            z = 0
        };

        if (newDir.x != 0)
            curDirection = Vector3.right * newDir.x;

        curDir = newDir;
    }

    private void FixedUpdate()
    {
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

        charSrite.flipX = curDirection.x != startDir;

        SetCameraTarget();
    }

    private void SetCameraTarget()
    {
        if (!cameraTarget) return;

        cameraTarget.transform.position = transform.position + Vector3.right * curDirection.x * xOffset;
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
        destiny = null;
        curDir = Vector3.zero;
    }

    public void Jump(bool basicJump, float holdTime)
    {
        var jumpForce = GetJumpVelocity(basicJump, holdTime);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private float GetJumpVelocity(in bool basicJump, in float holdTime)
    {
        var minH = basicJump ? minFirstJumpHeight : minSecJumpHeight;
        var maxH = basicJump ? maxFirstJumpHeight : maxSecJumpHeight;
        var gapHeight = maxH - minH;

        var extraHeigh = holdTime * gapHeight;

        return Mathf.Sqrt((minH + extraHeigh) * 2 * Physics.gravity.magnitude);
    }

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

    
}
