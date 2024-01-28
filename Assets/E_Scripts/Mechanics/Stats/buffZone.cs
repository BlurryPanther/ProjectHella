using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffZone : MonoBehaviour
{
    BuffManager buffManager;

    private void Start()
    {
        buffManager = FindObjectOfType<BuffManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            buffManager.SetBuff(other.GetComponent<Character>(), 1);
        }
    }
}
