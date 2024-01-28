using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements;

public class Player : Character
{

    [SerializeField] bool hpMask;
    [SerializeField] bool fireMask;
    [SerializeField] bool jumpMask;
    myAction caAttack;
    myAction caJump;
    float curHeavySlashTime = 0;
    bool slashPerformed = false;
    [SerializeField] double slashHold = .2;
    [SerializeField] double jumpHold = .2;
    [SerializeField] double maxJumpHold = .5;

    // Start is called before the first frame update
    void Start()
    {
        animController = gameObject.GetComponent<Animator>();
        charSrite = gameObject.GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        caAttack = new myAction(.1f);
        caJump = new myAction(.1f);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckPoint.activePoint.RestartPoint();
        }

        if (hp <= 0)
        {
            KillCharacter();
            animController.SetBool("isDead", true);
        }
    }

    #region Inputs
    public void Move(InputAction.CallbackContext callbackContext)
    {
        animController.SetBool("isRunning", true);
        if(callbackContext.performed)
        {
            var dir = new Vector3()
            {
                x = callbackContext.ReadValue<float>(),
                y = 0,
                z = 0,
            };
            if (callbackContext.ReadValue<float>() < 0)
                charSrite.flipX = true;
            if(callbackContext.ReadValue<float>() > 0)
                charSrite.flipX = false;
            movement.MyMove(dir);
        }
        else
        {
            movement.MyMove(Vector2.zero);
            animController.SetBool("isRunning", false);
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (!caJump.canMove) return;
        if ((isGrounded && jumpsCount > 0) || (!IsGrounded && jumpsCount > 1) || !canJump) return;

        if (jumpsCount > 0 && !jumpMask) return;

        bool basicJump = jumpsCount <= 0;

        if (callbackContext.canceled)
        {
            double timePercent = callbackContext.duration < jumpHold ? 0 : callbackContext.duration / maxJumpHold;

            movement.Jump(basicJump, (float)timePercent);
            StartCoroutine(caJump.CoolDown());

            DisableJump();
        }
        if (callbackContext.performed)
        {
            movement.Jump(basicJump, 1);
            StartCoroutine(caJump.CoolDown());
            
            DisableJump();
        }
    }

    private void DisableJump()
    {
        canJump = false;
        jumpsCount++;
        Invoke("EnableJump", .3f);
    }

    public void HeavyAttack(InputAction.CallbackContext callbackContext)
    {
        if (!caAttack.canMove) return;

        if (callbackContext.canceled && callbackContext.duration < slashHold)
        {
            animController.SetBool("isLightAttacking", true);
            attack.Slash(fireMask);
            StartCoroutine(caAttack.CoolDown());
            Invoke("EndAnim", 1);
        }
        if (callbackContext.performed)
        {
            animController.SetBool("isHeavyAttacking", true);
            attack.HeavySlash(fireMask);
            StartCoroutine(caAttack.CoolDown());
            Invoke("EndAnim", 1);
        }
    }

    public void AddMask(MaskType type)
    {
        if (type == MaskType.Fire)
            fireMask = true;
        else if (type == MaskType.Jump)
            jumpMask = true;
        else
        {
            hpMask = true;
            hp = maxHp;
        }
    }
    #endregion

    public void Deconstruct(out bool fireMask, out bool jumpMask, out bool hpMask, 
        out Vector3 position, out Vector3 localScale, out Vector3 curDir, out int hp,
        out int shield)
    {
        fireMask = this.fireMask;
        jumpMask = this.jumpMask;
        hpMask = this.hpMask;
        position = transform.position; 
        localScale = transform.localScale;
        curDir = movement.CurDirection;
        hp = this.hp;
        shield = 0;
    }

    public void LoadData(object data)
    {
        (this.fireMask,
        this.jumpMask,
        this.hpMask,
        transform.position,
        transform.localScale,
        movement.CurDirection,
        this.hp,
        hp) = ((bool, bool, bool, Vector3, Vector3, Vector3, int, int)) data;

        movement.Stop();
    }

    void EndAnim()
    {
        animController.SetBool("isLightAttacking", false);
        animController.SetBool("isHeavyAttacking", false);
    }
}

public enum MaskType
{
    None,
    Jump,
    HP,
    Fire
}

