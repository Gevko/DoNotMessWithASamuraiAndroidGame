using System.Collections;
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
    public Transform playerCheck;

    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private LayerMask damageLayerMask;

    [SerializeField]
    private int damage = 25;

    private Collider2D[] results = new Collider2D[1];

    private Collider2D[] playerResults = new Collider2D[1];

    private Collider2D[] damageResults = new Collider2D[1];

    [SerializeField]
    private float speed = 1f;

    private float life = 250f;

    public bool isAlive = true;

    [SerializeField]
    private Image lifebarImage;

    [SerializeField]
    private Canvas myCanvas;

    [SerializeField]
    private float moveSpeed = 5f;

    private Camera mainCamera;

    private Transform player;

    private bool lookingRight = true;

    private float attackRange = 0.2f;

    private Collider2D playerCollider;

    private int cooldownCounter = 0;

    private int cooldownInMs = 1500;

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
        //GameManager.Instance.allowMoving = false;
        UpdateLifebarImage();
    }

    private void Update()
    {
        if (isAlive && GameManager.Instance.allowMoving)
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

       if(isAlive && GameManager.Instance.allowMoving) {
         Collider2D[] playerColliders = Physics2D.OverlapCircleAll(
            playerCheck.position,
            attackRange,
            playerLayerMask);


           if (cooldownCounter == 0)
            {
               if(playerColliders.Length > 0) { Attack(); } else { AttackLongRange(); }
               cooldownCounter = Environment.TickCount;
            }
            else
            {
             if (Environment.TickCount - cooldownCounter > cooldownInMs)
             {
               if(playerColliders.Length > 0) { Attack(); } else { AttackLongRange(); }
              cooldownCounter = Environment.TickCount;
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
            Vector3 direction = player.transform.position - playerCheck.transform.position;
            direction.Normalize();
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            myAnimator.SetTrigger("AttackLongRange");
            GameObject kunai =
            Instantiate(kunaiPrefab, playerCheck.position, Quaternion.Euler(0, 0, rotation - 90));
         /*   if(kunai != null) {
                        kunai.GetComponent<Rigidbody2D>().velocity = (player.transform.position - playerCheck.transform.position).normalized * moveSpeed;
            }*/
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
            playerCollider.GetComponent<CharacterController>().AddCharacterHpAp(5, 5);
        }

        isAlive = false;

        GameManager.Instance.secondBossDead = true;

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
