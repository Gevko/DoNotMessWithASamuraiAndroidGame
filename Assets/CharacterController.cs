using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float jumpForce = 7f;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Image lifebarImage;

    [SerializeField]
    private Image armourbarImage;

    [SerializeField]
    private Canvas healthBarCanvas;

    [SerializeField]
    private Transform attackPos;

    [SerializeField]
    private LayerMask enemyLayerMask;

    [SerializeField]
    private int damage = 25;

    private float attackRange = 0.2f;

    private int healthPoints = 100;

    private int armourPoints = 100;

    private bool right;

    private Rigidbody2D myRigidbody;

    private Animator myAnimator;

    private bool jump = false;

    private bool isAlive = true;

    private Collider2D[] results = new Collider2D[1];

    private Camera mainCamera;

    private int cooldownCounter = 0;

    private int cooldownInMs = 700;

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mainCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isAlive)
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
                StopAttackAnimation();
                jump = true;
            }

            if (Input.GetButtonDown("Fire1"))
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
            }

            HandleMovement(horizontalInput);

        }
    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayerMask);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i] != null)
            {
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    private void StopAttackAnimation()
    {
        myAnimator.ResetTrigger("Attack");
    }


    private void HandleMovement(float horizontalInput)
    {

        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRigidbody.velocity = new Vector2(horizontalInput * 0, myRigidbody.velocity.y);

            StopAttackAnimation();

        }
        else
        {
            myRigidbody.velocity = new Vector2(horizontalInput * speed, myRigidbody.velocity.y);

            myAnimator.SetFloat("Spd", Mathf.Abs(horizontalInput));

            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, 0.1f, enemyLayerMask);

            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<Enemy>().StopMovement();
            }
        }

    }


    private void Flip()
    {
        Vector3 localRotation = transform.localEulerAngles;
        localRotation.y += 180f;
        transform.localEulerAngles = localRotation;
        healthBarCanvas.transform.forward = mainCamera.transform.forward;

    }

    private void FixedUpdate()
    {
        if (jump && IsGrounded())
        {
            Jump();
        }

        HandleLayers();
    }

    private void Jump()
    {
        myRigidbody.velocity = Vector2.zero;
        myRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jump = false;
    }

    private bool IsGrounded()
    {
        bool r = Physics2D.OverlapPointNonAlloc(groundCheck.position, results, groundLayer) > 0;

        if (!r)
        {
            r = Physics2D.OverlapPointNonAlloc(groundCheck.position, results, enemyLayerMask) > 0;
        }

        if (r)
        {
            myAnimator.SetTrigger("Jmp");
        }

        return r;
    }

    private void HandleLayers()
    {
        if (!IsGrounded())
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isAlive)
        {

            if (armourPoints > 0)
            {
                // vou remover armor 1º
                if (armourPoints < dmg)
                {
                    dmg -= armourPoints;
                    armourPoints = 0;
                }
                else
                {
                    armourPoints -= dmg;
                    dmg = 0;
                }
            }

            if (dmg > 0)
            {
                healthPoints -= dmg;

                if (healthPoints < 0)
                {
                    healthPoints = 0;
                }
            }

            if (healthPoints == 0)
            {
                Defeat();
            }

            updateBars();

        }
    }

    public void LifeSteal(int hp, int ap)
    {
        if (isAlive)
        {
            healthPoints += hp;
            armourPoints += ap;

            if (healthPoints > 100)
            {
                healthPoints = 100;
            }

            if (armourPoints > 100)
            {
                armourPoints = 100;
            }

            updateBars();
        }
    }

    private void Defeat()
    {
        isAlive = false;
        myAnimator.SetTrigger("Defeat");
    }

    private void updateBars()
    {
        lifebarImage.fillAmount = healthPoints / 100f;
        armourbarImage.fillAmount = armourPoints / 100f;
    }

}
