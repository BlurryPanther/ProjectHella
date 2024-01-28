using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    List<Buff> buffs = new List<Buff>();
    public static BuffManager Instance;

    public class Buff
    {
        public bool IsAlive { get; private set; } = true;
        public float Lifespan { get; private set; } = 10;
        public float curTime = 0;

        public int Damage { get; private set; } = 3;
        public float Rate { get; private set; }
        public Character Target { get; set; }

        public event Action<Buff> OnDead;

        public Buff(Character target, int damage, Action<Buff> action)
        {
            Target = target;
            this.Damage = damage;
            OnDead += action;
            target.OnDead += () => OnDead?.Invoke(this);
        }

        public void IncreaseCurTime()
        {
            curTime += Rate;

            if (curTime >= Lifespan)
            {
                IsAlive = false;
                OnDead?.Invoke(this);
            }
        }
    }

    private void Start()
    {
        Instance ??= this;
    }

    public void SetBuff(Character target, int damage)
    {
        Buff buff = new Buff(target, damage, OnBuffDead);

        buffs.Add(buff);
        StartCoroutine(StartBuff(buff));
    }

    private void OnBuffDead(Buff buff)
    {
        buffs.Remove(buff);
    }

    IEnumerator StartBuff(Buff buff)
    {
        while (buff.IsAlive)
        {
            yield return new WaitForSeconds(buff.Rate);

            buff.Target.Damage(buff.Damage);

            buff.IncreaseCurTime();
        }
    }
}
