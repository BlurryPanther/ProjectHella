using UnityEngine;

public class Thorns : Interactable
{
    public int dmg = 2;

    public override void Action()
    {
        base.Action();

        var target = FindObjectOfType<Player>();

        target.Damage(dmg, transform.position);
    }
}
