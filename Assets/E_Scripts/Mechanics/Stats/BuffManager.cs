using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    [SerializeField] List<Buff> buffs = new List<Buff>();
    public static BuffManager Instance;

    [Serializable] public class Buff
    {
        //Hacemos que las variables solo puedan ser leidas más no modificadas
        public bool IsAlive { get => isAlive;}
        [SerializeField] bool isAlive = true;
        public float Lifespan { get => lifespan;}
        [SerializeField] float lifespan = 10;

        [SerializeField] float curTime = 0;

        public int Damage { get => damage;}
        [SerializeField] int damage = 3;
        public float Rate { get => rate;}
        [SerializeField] float rate = 1;
        public Character Target { get; set; }

        //Declaramos el evento OnDead que recibe un parametro de tipo Buff
        public event Action<Buff> OnDead;

        //Cuando una función tiene el mismo nombre que la clase se convierte en un constructor,
        //por lo que se se ejecuta primero y solo una sola vez
        public Buff(Character target, int damage, Action<Buff> action)
        {
            //Estamos declarando nuestras variables
            Target = target;
            this.damage = damage;
            //La acción que recibimos como parametro la estamos subscribiendo al Ondead (un evento)
            OnDead += action;
            //estamos subscribiendo nuestro evento Ondead al evento del target Ondead, para que los 2 se mueran al mismo
            //tiempo, pero OnDead del buff necesita una variable del tipo Buff y OnDead del target no por lo que
            //subscribismo OnDead a una funcion anonima y ahora ya nos deja subscribir OnDead Buff con su variable a esa
            //funcion
            target.OnDead += () => OnDead?.Invoke(this);

        }

        public void RestartCurTime() => curTime = 0;

        //Creamos una función que se encargará de revisar el tiempo de vida del buff
        public void IncreaseCurTime()
        {
            curTime += Rate;

            if (curTime >= Lifespan)
            {
                isAlive = false;
                OnDead?.Invoke(this);
            }
        }

        //Corritina que crea un buff y mientras que esta este viva, ira aplicando el daño periodicamente
        public IEnumerator StartBuff()
        {
            while (IsAlive)
            {
                yield return new WaitForSeconds(Rate);

                Target.Damage(Damage);
                print("BurnBitch");
                IncreaseCurTime();
            }
        }
    }

    private void Start()
    {
        Instance ??= this;
    }

    //La función que será llamada por el tipo de ataque que ocupe el buff
    public void SetBuff(Character target, int damage)
    {
        if (CheckTarjet(target))
            return;
        Buff buff = new Buff(target, damage, OnBuffDead);

        buffs.Add(buff);
        StartCoroutine(buff.StartBuff());
    }

    //Revisa si el targer ya esta en la lista
    bool CheckTarjet(Character target)
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            if(buffs[i].Target == target)
            {
                buffs[i].RestartCurTime();
                return true;
            }
        }
        return false;
    }

    //La función que se encarga de eliminar el buff
    private void OnBuffDead(Buff buff)
    {
        buffs.Remove(buff);
        StopCoroutine(buff.StartBuff());
    }
}
