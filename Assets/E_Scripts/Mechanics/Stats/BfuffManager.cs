using System;
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
        Character target;
        float lifespan = 10;
        float damage = 3;
        float curTime = 0;
        float tick = 0;

        Buff()
        {
            target.OnDead += OnDead;
        }

        public event Action OnDead;
    }

    private void Start()
    {
        Instance ??= this;
    }
}
