﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FinalBoss : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    private Animator myAnimator;

    [SerializeField]
    private GameObject kunaiPrefab;

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

    [SerializeField]
    private int damage = 10;

    private Collider2D[] results = new Collider2D[1];

    private Collider2D[] playerResults = new Collider2D[1];

    private Collider2D[] damageResults = new Collider2D[1];

    [SerializeField]
    private float speed = 1f;

    private float life = 100f;

    private bool isAlive = true;

    [SerializeField]
    private Image lifebarImage;

    [SerializeField]
    private Canvas myCanvas;

    private Camera mainCamera;

    private Transform player;

    private bool lookingRight = true;

    private float attackRange = 0.2f;

    private Collider2D playerCollider;

    private int cooldownCounter = 0;

    private int cooldownInMs = 1000;

    private float shootVelocity = 5f;


    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mainCamera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        UpdateLifebarImage();
    }

    private void Update()
    {
        if (isAlive)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (player.position.x > transform.position.x && !lookingRight)
            {
                Flip();
            }
            if (player.position.x < transform.position.x && lookingRight)
            {
                Flip();
            }
            /* myRigidbody.velocity =
                new Vector2(speed * transform.right.x,
                myRigidbody.velocity.y
                ); */

            //myAnimator.SetFloat("HorizontalSpeed", Mathf.Abs(myRigidbody.velocity.x));
        }
    }

    private void FixedUpdate()
    {
        /*if (Physics2D.OverlapPointNonAlloc(
            groundCheck.position,
            results,
            groundLayerMask) == 0)
        {
            Flip();
        }*/

        Collider2D[] playerColliders = Physics2D.OverlapCircleAll(
            playerCheck.position,
            attackRange,
            playerLayerMask);


        if (playerColliders.Length != 0)
        {
            if (cooldownCounter == 0)
            {
                Attack();
                cooldownCounter = Environment.TickCount;
            }
            else
            {
                if (Environment.TickCount - cooldownCounter > cooldownInMs)
                {
                    Attack();
                    cooldownCounter = Environment.TickCount;
                }
            }
        } else
        {
            if (cooldownCounter == 0)
            {
                AttackLongRange();
                cooldownCounter = Environment.TickCount;
            }
            else
            {
                if (Environment.TickCount - cooldownCounter > cooldownInMs)
                {
                    AttackLongRange();
                    cooldownCounter = Environment.TickCount;
                }
            }
        }
        //if (Physics2D.OverlapPointNonAlloc(
        //    playerCheck.position,
        //    damageResults,
        //    damageLayerMask) != 0)
        //{
        //    TakeDamage(25);
        //}
    }

    private void Flip()
    {
        Vector3 localRotation = transform.localEulerAngles;
        localRotation.y += 180f;
        lookingRight = !lookingRight;
        transform.localEulerAngles = localRotation;
        myCanvas.transform.forward = mainCamera.transform.forward;
    }

    private void Attack()
    {
        if (isAlive)
        {
            myAnimator.SetTrigger("Attack");
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().TakeDamage(damage);
        }
    }

    private void AttackLongRange()
    {
        if (isAlive)
        {
            myAnimator.SetTrigger("AttackLongRange");
            GameObject star =
            Instantiate(kunaiPrefab, playerCheck.position, Quaternion.Euler(0, 0, 90));
            star.GetComponent<Rigidbody2D>().velocity = playerCheck.right * shootVelocity;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isAlive)
        {
            //myAnimator.SetTrigger("TookDamage");
            life -= damage;
            if (life < 0f)
            {
                life = 0f;
            }

            UpdateLifebarImage();

            if (life == 0f)
            {
                Die();
            }
        }
    }
    private void Die()
    {
        if (playerCollider != null)
        {
            playerCollider.GetComponent<CharacterController>().LifeSteal(5, 5);
        }

        isAlive = false;

        myAnimator.SetTrigger("Die");

        //GameManager.Instance.EnemyKilled();

    }

    private void UpdateLifebarImage()
    {
        lifebarImage.fillAmount = life / 100f;
    }

    private void DestroyEnemy() //called by animation event
    {
        Destroy(gameObject);
    }

    public void StopMovement()
    {
        myRigidbody.velocity = Vector3.zero;
    }


}