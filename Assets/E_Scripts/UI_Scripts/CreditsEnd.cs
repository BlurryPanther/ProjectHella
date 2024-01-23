using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsEnd : MonoBehaviour
{
    public GameObject MainCanvas;
    public GameObject CreditCanvas;

    public void StartCredits()
    {
        Invoke("WaitToEnd", 22 );
    }

    public void WaitToEnd ()
    {
        MainCanvas.SetActive(true);
        CreditCanvas.SetActive(false);
        
    }
}
