using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody2D myRigidbody;

    private Animator myAnimator;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform playerCheck;

    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private LayerMask damageLayerMask;

    private Collider2D[] results = new Collider2D[1];

    private Collider2D[] playerResults = new Collider2D[1];

    private Collider2D[] damageResults = new Collider2D[1];

    [SerializeField]
    private float speed = 1f;

    private float life = 100f;

    private bool isAlive = true;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isAlive)
        {
            myRigidbody.velocity =
                new Vector2(speed * transform.right.x,
                myRigidbody.velocity.y
                );

            //myAnimator.SetFloat("HorizontalSpeed", Mathf.Abs(myRigidbody.velocity.x));
        }
    }

    private void FixedUpdate()
    {
        if (Physics2D.OverlapPointNonAlloc(
            groundCheck.position,
            results,
            groundLayerMask) == 0)
        {
            Flip();
        }
        if (Physics2D.OverlapPointNonAlloc(
            playerCheck.position,
            playerResults,
            playerLayerMask) != 0)
        {
            Attack();
        }
        if (Physics2D.OverlapPointNonAlloc(
            playerCheck.position,
            damageResults,
            damageLayerMask) != 0)
        {
            TakeDamage(25);
        }
    }

    private void Flip()
    {
        Vector3 localRotation = transform.localEulerAngles;
        localRotation.y += 180f;
        transform.localEulerAngles = localRotation;
        //myCanvas.transform.forward = mainCamera.transform.forward;
    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");
    }

    private void TakeDamage(float damage)
    {
        if (isAlive)
        {
            myAnimator.SetTrigger("TookDamage");
            life -= damage;
            if (life < 0f)
            {
                life = 0f;
            }

            if (life == 0f)
            {
                Die();
            }
        }
    }
    private void Die()
    {
        isAlive = false;
        myAnimator.SetTrigger("Die");
    }

}
