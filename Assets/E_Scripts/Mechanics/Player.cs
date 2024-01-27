using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private static Player instance;
    public Player Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
