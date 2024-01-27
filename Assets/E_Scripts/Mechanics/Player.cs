using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : Character
{
    Movement myMovement;
    Attack myAttack;
    [SerializeField] InputActionReference myAction;

    // Start is called before the first frame update
    void Start()
    {
        myMovement = GetComponent<Movement>();
        myAttack = GetComponent<Attack>();

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

            myMovement.MyMove(dir);
        }
        else
            myMovement.MyMove(Vector2.zero);
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            myMovement.Jump();
        }
    }

    public void Attack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.canceled && callbackContext.startTime < 26)
            myAttack.Slash();
        else
            print(callbackContext.startTime);
    }

    public void HeavyAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            myAttack.HeavySlash();
        }
    }
    #endregion
}
