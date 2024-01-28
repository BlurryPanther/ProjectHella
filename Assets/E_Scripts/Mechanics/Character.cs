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

    protected Animator animController;
    protected SpriteRenderer charSrite;

    //Health
    public int maxHp = 8;
    [Space()]
    public int hp = 5;

    //Damage
    public int dmg = 3;

    public event Action OnDead;

    myAction caImmortality;

    private void Awake()
    {
        caImmortality = new myAction(immortalityTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int value, Vector3? pos = null)
    {
        if (hp <= 0 || !caImmortality.canMove) return;

        hp -= value;
        if (hp <= 0)
        {
            OnDead?.Invoke();
        }

        StartCoroutine(caImmortality.CoolDown());

        //if (pos != null)
        //    KnockBack(pos.Value);
        //null coalesing opperator
    }

    private void KnockBack(Vector3 enemyPos)
    {
        if (!knockBack) return;
        {
            var dir1 = (transform.position - enemyPos).normalized * knockBackDis;
            var dir = new Vector3(dir1.x, 0, 0);
            dir += transform.position;

            movement.MoveTo(dir, FinishKnockBack);
        }
    }

    public void FinishKnockBack()
    {

    }

    public virtual void Revive()
    {
        hp = maxHp;
    }
    public void KillCharacter()
    {
    }
}
