using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Attack : MonoBehaviour
{
    Movement movement;
    bool damageOnHit = false;
    int collitionDamage = 0;
    [SerializeField] int slashDamage;
    [SerializeField] float slashDis;
    [SerializeField] int heavySlashDmg;
    [SerializeField] float heavySlashDis;
    [SerializeField] int blowDmg;
    [SerializeField] int hitDmg;
    [SerializeField] int thornsDmg = 2;

    private void FixedUpdate()
    {
        if (damageOnHit)
        {
            RaycastHit hit;
            if (Physics.BoxCast(transform.position, new Vector3(.5f, .5f, .5f), Vector3.left, out hit, Quaternion.identity, .1f, 1 << 9))
            {
                if (hit.collider.GetComponent<Player>() is var p && p)
                    p.Damage(thornsDmg, transform.position);
            }
        }
    }

    public void EnableDagameOnHit(bool shouldEnable = true)
    {
        damageOnHit = shouldEnable;
        collitionDamage = hitDmg;
        movement = GetComponent<Movement>();  
        print("Push!!");
    }

    public void Slash(bool buff = false)
    {
        print("Slash!!");
        var target = DetectPlayer(slashDis);

        if (target)
            target.Damage(slashDamage, transform.position);
    }

    public void HeavySlash(bool buff = false)
    {
        print("Heavy Slash!!");
        var target = DetectPlayer(heavySlashDis);

        target?.Damage(heavySlashDmg, transform.position);
    }

    public void Blow(float radious, Character target)
    {
        print("Blow");

        if (Vector3.Distance(target.transform.position, transform.position) < radious)
            target.Damage(blowDmg, transform.position);
    }

    public void Trhow_Thorns(Character target)
    {
        print("Thorns!!");
    }

    private Character DetectPlayer(float dis)
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, new Vector3(.5f, .5f, .5f), movement.CurDirection, out hit, Quaternion.identity, dis, 1 << 9))
        {
            if (hit.collider.gameObject.GetComponent<Character>() is var c && c)
            {
                if (!hit.collider.TryGetComponent<Player>(out _))
                {

                    return c;
                }
            }
        }

        return null;
    }
}
