using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{

    [SerializeField] bool hpMask;
    [SerializeField] bool fireMask;
    [SerializeField] bool jumpMask;
    ConditialAction caAttack;
    ConditialAction caJump;
    float curHeavySlashTime = 0;
    bool slashPerformed = false;
    [SerializeField] double slashHold = .2;
    [SerializeField] double jumpHold = .2;
    [SerializeField] double maxJumpHold = .5;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        caAttack = new ConditialAction(.1f);
        caJump = new ConditialAction(.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Inputs
    public void Move(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed)
        {
            var dir = new Vector3()
            {
                x = callbackContext.ReadValue<float>(),
                y = 0,
                z = 0,
            };

            movement.MyMove(dir);
        }
        else
            movement.MyMove(Vector2.zero);
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (!caJump.canMove) return;

        if (callbackContext.canceled)
        {
            double timePercent = callbackContext.duration < jumpHold ? 0 : callbackContext.duration / maxJumpHold;
            movement.Jump(jumpMask, (float)timePercent);
            StartCoroutine(caJump.CoolDown());
        }
        if (callbackContext.performed)
        {
            movement.Jump(jumpMask, 1);
            StartCoroutine(caJump.CoolDown());
        }
    }

    public void Attack(InputAction.CallbackContext callbackContext)
    {
        //if (!caAttack.canMove) return;

        //if (callbackContext.canceled)
        //{
        //    StartCoroutine(AttackCoolDown());
        //    attack.Slash(dmg, fireMask ? true : false);
        //}
    }

    public void HeavyAttack(InputAction.CallbackContext callbackContext)
    {
        if (!caAttack.canMove) return;

        if (callbackContext.canceled && callbackContext.duration < slashHold)
        {
            print("Slash in " + callbackContext.duration);
            attack.Slash(dmg, fireMask);
            StartCoroutine(caAttack.CoolDown());
        }
        if (callbackContext.performed)
        {
            print("Heavy Slash");
            attack.HeavySlash(dmg, fireMask);
            StartCoroutine(caAttack.CoolDown());
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
}

public enum MaskType
{
    None,
    Jump,
    HP,
    Fire
}

