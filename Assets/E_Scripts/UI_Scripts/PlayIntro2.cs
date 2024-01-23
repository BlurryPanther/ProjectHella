using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIntro2 : MonoBehaviour
{
    public Animator Animator;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke ("PlayAnimation",2.0f);
    }

    private void PlayAnimation()
    {
        Animator.Play("Intro2");
    }

}
