using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwMolotov : MonoBehaviour
{
    [SerializeField] private GameObject molotovPrefab;

    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = new Vector3
        {
            x = transform.position.x + 1,
            y = transform.position.y + 1,
            z = 0
        };
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(molotovPrefab, dir, Quaternion.identity);
        }
    }
}
