using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    LayerMask layer;
    [SerializeField] private GameObject groundCheck;

    //goomba

    [SerializeField] private bool changeDir;

    RaycastHit wc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Physics.Raycast(groundCheck.transform.position, speed * Vector2.left, out wc, .25f, layer);
        //Debug.DrawRay(GC.transform.position, wcd * Vector2.left * .25f, Color.red);
        
    }

    void ChangeDir()
    {
        groundCheck.transform.position = new Vector3(transform.position.x - 1, 0, 0);
    }

    void Move()
    {
        rb.velocity = new Vector3(rb.velocity.x + speed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            Debug.Log("Falling");
            speed *= -1;
            ChangeDir();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
