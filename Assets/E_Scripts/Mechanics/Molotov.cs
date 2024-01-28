using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    public Vector3 dir = new Vector3
    {
        x = 1,
        y = 1,
        z = 0,
    }.normalized;

    bool doOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!doOnce)
        {
            rb.AddForce(dir * speed, ForceMode.Impulse);
            doOnce = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            Destroy(gameObject.GetComponent<Rigidbody>());
            GameObject molly = gameObject.transform.GetChild(0).gameObject;
            molly.SetActive(true);
        }
    }
}
