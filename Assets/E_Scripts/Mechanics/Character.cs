using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Movement movement;
    protected Attack attack;
    //Health
    public int maxHp = 10;
    public int hp = 5;

    //Damage
    public int dmg = 3;

    public event Action OnDead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int value)
    {
        if (hp <= 0)
            OnDead?.Invoke();

        //null coalesing opperator
    }

    public virtual void Revive()
    {
        hp = maxHp;
    }
}
