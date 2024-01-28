using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masks : MonoBehaviour
{
    public enum Maskss
    {
        hpMask,
        fireMask,
        jumpMask,
    }
    public Maskss mask;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (mask)
            {
                case Maskss.hpMask:
                    other.GetComponent<Player>().AddMask(MaskType.HP);
                    Destroy(gameObject);
                    break;
                case Maskss.fireMask:
                    other.GetComponent<Player>().AddMask(MaskType.Fire);
                    Destroy(gameObject);
                    break;
                case Maskss.jumpMask:
                    other.GetComponent<Player>().AddMask(MaskType.Jump);
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
