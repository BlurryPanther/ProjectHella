using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Movement : MonoBehaviour
{

    Player player;

    private Rigidbody rb;
    [Header("Settings"), Space(10)]
    [SerializeField] private float speed;
    [SerializeField] float jumpForce;

    [SerializeField] private GameObject groundCheck;

    //goomba

    [SerializeField] private bool changeDir;

    RaycastHit wc;

    public int Jumps { get; set; } = 1;

    // Start is called before the first frame update
    public void Start()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        PartialStart();
    }

    // Update is called once per frame
    void Update()
    {
        //Move();

        //Physics.Raycast(groundCheck.transform.position, speed * Vector2.left, out wc, .25f, layer);
        //Debug.DrawRay(GC.transform.position, wcd * Vector2.left * .25f, Color.red);
        
    }

    void ChangeDir()
    {
        groundCheck.transform.position = new Vector3(transform.position.x - 1, 0, 0);
        
    }

    public void Move(Rigidbody rb)
    {
        print("FuCK");
        rb.velocity = new Vector3(rb.velocity.x * speed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Wall")
        {
            speed *= -1;
        }
    }
}
