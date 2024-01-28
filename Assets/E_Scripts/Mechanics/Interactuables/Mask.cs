using UnityEngine;

public class Mask : Interactable
{
    public MaskType type;

    public override void Action()
    {
        base.Action();

        var player = FindObjectOfType<Player>();

        player.AddMask(type);
        gameObject.SetActive(false);
    }
}
