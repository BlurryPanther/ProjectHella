using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float immortalityTime = 1;
    [SerializeField] bool knockBack = false;
    [SerializeField] float knockBackDis = .5f;
    protected Movement movement;
    protected Attack attack;
    private Rigidbody rb;
    protected BoxCollider col;

    protected Animator animController;
    protected SpriteRenderer charSrite;

    //Health
    public int maxHp = 8;
    [Space()]
    public int hp = 5;

    //Damage
    public int dmg = 3;

    public bool inknockback;
    myAction caImmortality;
    [SerializeField] protected bool isGrounded = false;
    protected bool canJump = true;
    public int jumpsCount = 0;

    public event Action OnDead;

    public bool IsGrounded
    {
        get => isGrounded;
        private set
        {
            if (value && canJump)
            {
                canJump = true;
                if (jumpsCount > 1)
                {
                    animController.SetBool("isJumping", false);
                    animController.SetBool("isSecondJumping", false);
                }
                if (jumpsCount == 1)
                    animController.SetBool("isJumping", false);
                jumpsCount = 0;
            }

            isGrounded = value;
        }
    }

    private void Awake()
    {
        caImmortality = new myAction(immortalityTime);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        animController = gameObject.GetComponent<Animator>();
        charSrite = gameObject.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        DetectGround();
    }

    public void Damage(int value, Vector3? pos = null)
    {
        if (hp <= 0 || !caImmortality.canMove) return;

        //animController.SetBool("gotHit", true);
        Invoke("StartAnimC", 1.5f);
        hp -= value;


        Invoke("EndAnimC", 2f);
        StartCoroutine(caImmortality.CoolDown());
        if (pos != null)
            KnockBack(pos.Value);
    }

    private void KnockBack(Vector3 enemyPos)
    {
        if (!knockBack || !canJump) return;

        inknockback = true;
        var dir1 = (transform.position - enemyPos);
        var dir = new Vector3(dir1.x, 0, 0);
        dir += transform.position;

        rb.AddForce(dir.normalized * knockBackDis, ForceMode.Impulse);
        Invoke("FinishKnockBack", 1);
        //movement.MoveTo(dir, FinishKnockBack);
    }

    private void EnableJump() => canJump = true;

    public void FinishKnockBack()
    {
        inknockback = false;
    }

    public virtual void Revive()
    {
        hp = maxHp;
    }
    public void KillCharacter()
    {
        if (hp <= 0)
        {
            OnDead?.Invoke();
        }
    }

    private bool DetectGround()
    {
        Vector3 size = new(.5f, .5f, .5f);
        var dis = (col ??= GetComponent<BoxCollider>()).bounds.extents.y + .5f;

        var hits = Physics.BoxCastAll(transform.position, Vector3.one, Vector3.down, Quaternion.identity, dis, 1 << 0);

        foreach (var hit in hits)
        {
            if (hit.point == default) continue;

            if (Vector3.Angle(hit.normal, Vector3.up) < 10)
            {
                Debug.DrawRay(hit.point, hit.normal * 4, Color.yellow);
                IsGrounded = true;
                return true;
            }
        }

        IsGrounded = false;
        return false;
    }

    void StartAnimC()
    {
        animController.SetBool("gotHit", true);
    }

    void EndAnimC()
    {
        animController.SetBool("gotHit", false);
    }
}
