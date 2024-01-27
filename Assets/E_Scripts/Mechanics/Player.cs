using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private static Player instance;
    public Player Instance { get { return instance; } }
    bool hpMask;
    bool fireMask;
    bool jumpMask;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();

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
        if (callbackContext.performed)
        {
            movement.Jump();
        }
    }

    public void Attack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.canceled && callbackContext.startTime < 26)
            attack.Slash(dmg, fireMask ? true : false);
        else
            print(callbackContext.startTime);
    }

    public void HeavyAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            attack.HeavySlash(dmg, fireMask ? true : false);
        }
    }

    public void AddMask(MaskType type)
    {
        if (type == MaskType.Fire)
            fireMask = true;
        else if (type == MaskType.Jump)
            jumpMask = true;
        else
            hpMask = true;
    }
    #endregion
}

public enum MaskType
{
    None,
    Jump,
    HP,
    Fire
}
