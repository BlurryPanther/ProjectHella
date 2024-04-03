using UnityEngine;

public class Enemy1 : Character
{
    [SerializeField] float attackTime = 2;
    [SerializeField] float attackAnimation = .25f;

    GameObject player;
    Vector3 dir = Vector3.right;
    myAction caAttack;
    bool canMove = true;

    protected override void Start()
    {
        base.Start();

        animController.SetBool("isMoving", true);
        player = FindObjectOfType<Player>(true).gameObject;
        col = GetComponent<BoxCollider>();
        attack.EnableDagameOnHit();

        caAttack = new myAction(1);

        dir = Vector3.right;
    }

    protected override void Update()
    {
        if (!caAttack.canMove) return;

        if (!DetectGround())
            ChangeDir();

        if (DetectPlayer() && caAttack.canMove)
            FrontAttack();
        else if (canMove)
        {
            animController.SetBool("isMoving", true);
            movement.MyMove(dir);
        }
    }

    private void FrontAttack()
    {
        if (caAttack.canMove)
            StartCoroutine(caAttack.CoolDown());

        animController.SetBool("isMoving", false);
        canMove = false;
        animController.SetBool("isAttacking", true);
        attack.Slash();

        Invoke("StopAnimation", attackAnimation);
    }

    private void StopAnimation()
    {
        animController.SetBool("isAttacking", false);
        canMove = true;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    ChangeDir();
    //}

    private void ChangeDir() => dir *= -1;

    private bool DetectGround()
    {
        float dis = col.bounds.extents.y + col.bounds.extents.y * .4f;
        Vector3 start = transform.TransformPoint(col.center) + dir * col.bounds.extents.x;

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

    private bool DetectPlayer()
    {
        RaycastHit hit;

        if (Physics.BoxCast(transform.position, new Vector3(.5f, .5f, .5f), movement.CurDirection, out hit, Quaternion.identity, 1, 1 << 9))
        {
            if (hit.collider.TryGetComponent<Player>(out _))
            {
                return true;
            }
        }

        return false;
    }
}
