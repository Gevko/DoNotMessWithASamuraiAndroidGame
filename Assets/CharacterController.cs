using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Transform groundCheck;

    private bool right;

    private Rigidbody2D myRigidbody;

    private Animator myAnimator;

    private bool jump = false;

    private Collider2D[] results = new Collider2D[1];

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (transform.right.x > 0 && horizontalInput < 0)
        {
            Flip();
        }

        if (transform.right.x < 0 && horizontalInput > 0)
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            Attack();
        }


        HandleMovement(horizontalInput);

    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");
    }

    private void HandleMovement(float horizontalInput)
    {
        myRigidbody.velocity =
new Vector2(horizontalInput * speed,
myRigidbody.velocity.y
);

        myAnimator.SetFloat("spd", Mathf.Abs(horizontalInput));
    }

    private void Flip()
    {
        Vector3 localRotation = transform.localEulerAngles;
        localRotation.y += 180f;
        transform.localEulerAngles = localRotation;
    }

    private void FixedUpdate()
    {
        if(jump && IsGrounded())
        {
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapPointNonAlloc(groundCheck.position, results, groundLayer) > 0;
    }

}
