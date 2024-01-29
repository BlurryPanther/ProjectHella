using UnityEngine;

public class Enemy3 : Character
{
    [SerializeField] protected GameObject edgeL;
    [SerializeField] protected GameObject edgeR;
    [SerializeField] GameObject droppable;
    bool canAttack = true;
    [SerializeField] float minDis;
    [SerializeField] float rateAttack;
    [SerializeField] myAction caPush;
    [SerializeField] Character target;

    private void Awake()
    {
        target = FindObjectOfType<Player>();
    }

    protected override void Start()
    {
        base.Start();

        OnDead += DropMask;
        caPush = new myAction(rateAttack);
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < minDis)
        {
            if (caPush.canMove && canJump)
            {
                StartCoroutine(caPush.CoolDown());
                Push();
            }
        }
    }

    private void DropMask()
    {
        Instantiate(droppable, transform.position, Quaternion.identity);
    }

    private void Push()
    {
        canAttack = false;
        caPush.canMove = false;

        Vector3 oppositeEdge = default;
        var edgePos = GetNearestEdge(out oppositeEdge);
        Vector3 dir1 = edgePos - transform.position;

        attack.EnableDagameOnHit();
        animController.SetBool("isMoving", true);

        movement.MoveTo(edgePos, () =>
            movement.MoveTo(oppositeEdge, StopPush, 2.6f));

        Vector3 GetNearestEdge(out Vector3 opposite)
        {
            if (Vector3.Distance(transform.position, edgeL.transform.position) <
                Vector3.Distance(transform.position, edgeR.transform.position))
            {
                opposite = edgeR.transform.position;
                return edgeL.transform.position;
            }
            else
            {
                opposite = edgeL.transform.position;
                return edgeR.transform.position;
            }
        }
    }

    private void StopPush()
    {
        attack.EnableDagameOnHit(false);
        caPush.canMove = true;

        caPush.Restart();
        canAttack = true;
        animController.SetBool("isMoving", false);
    }
}
