using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float speed = 6f;

    [SerializeField]
    private float jumpForce = 10f;

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
    private LayerMask armourBonusLayerMask;

    [SerializeField]
    private int damage = 25;

    [SerializeField]
    private GameObject rDmgPoints;

    [SerializeField]
    private GameObject gDmgPoints;
    // estes 2 sao para sair
   /* [SerializeField]
    private GameObject dialogueText;

    [SerializeField]
    private GameObject dialogueBox;
    */
    private float attackRange = 0.5f;

    private int healthPoints = 100;

    private int armourPoints = 100;

    private bool right;

    private Rigidbody2D myRigidbody;

    private Animator myAnimator;

    private bool jump = false;

    public bool isAlive = true;

    private Collider2D[] results = new Collider2D[1];

    private Camera mainCamera;

    private int cooldownCounter = 0;

    private int cooldownInMs = 700;

    // Start is called before the first frame update
    private void Start()
    {
        armourPoints = GameManager.Instance.playerAP;
        healthPoints = GameManager.Instance.playerHP;
        updateBars();
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
        // isto é para sair
        /*if (dialogueText != null)
        {
            Renderer r = dialogueText.GetComponent<Renderer>();

            TextMesh mr = dialogueText.GetComponent<TextMesh>();

            r.sortingLayerName = "PlayerLayer";

            mr.text = "batatas fritas com arroz\nbatatatatattatatata\nagagagaggagagagaga\nbsbsjsjsjsbjs";
        }*/


        if (isAlive)
        {
            if(GameManager.Instance.allowMoving) {
            
                
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

            } else {
                // se não me posso mexer - o fire1 = proxima mensagem
                if (Input.GetButtonDown("Fire1")) {
                                UIManager.Instance.HandleNextMessage();
                }
            }
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
                bool isAlive = false;
                if(enemiesToDamage[i].GetComponent<Enemy>() != null && enemiesToDamage[i].GetComponent<Enemy>().isAlive) {
                  enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(GameManager.Instance.firstBossDead ? damage* 2 : damage);
                   isAlive = true;
                }

                if(enemiesToDamage[i].GetComponent<Boss>() != null && enemiesToDamage[i].GetComponent<Boss>().isAlive) {
                  enemiesToDamage[i].GetComponent<Boss>().TakeDamage(GameManager.Instance.firstBossDead ? damage * 2 : damage);
                    isAlive = true;
                }

                if(enemiesToDamage[i].GetComponent<FinalBoss>() != null && enemiesToDamage[i].GetComponent<FinalBoss>().isAlive) {
                  enemiesToDamage[i].GetComponent<FinalBoss>().TakeDamage(GameManager.Instance.firstBossDead ? damage * 2 : damage);
                    isAlive = true;
                }

                if(isAlive) { AppearDmgPoints(GameManager.Instance.firstBossDead ? damage * 2 : damage, false, false); }
            }
        }
    }

    private void StopAttackAnimation()
    {
        myAnimator.ResetTrigger("Attack");
    }


    private void HandleMovement(float horizontalInput)
    {
        DetectArmourBonus();

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
                if(enemiesToDamage[i].GetComponent<Enemy>() != null) {
                    enemiesToDamage[i].GetComponent<Enemy>().StopMovement();
                }
                if(enemiesToDamage[i].GetComponent<Boss>() != null) {
                    enemiesToDamage[i].GetComponent<Boss>().StopMovement();
                }
                if(enemiesToDamage[i].GetComponent<FinalBoss>() != null) {
                    enemiesToDamage[i].GetComponent<FinalBoss>().StopMovement();
                }
            }
        }

    }


    private void Flip()
    {
        Vector3 localRotation = transform.localEulerAngles;
        localRotation.y += 180f;
        transform.localEulerAngles = localRotation;
        healthBarCanvas.transform.forward = mainCamera.transform.forward;
        // isto é para sair
        /*if (dialogueBox != null && dialogueText != null)
        {
            dialogueBox.transform.forward = mainCamera.transform.forward;
            dialogueText.transform.forward = mainCamera.transform.forward;
        }*/
    }

    private void FixedUpdate()
    {
        if (jump && IsGrounded() && GameManager.Instance.allowMoving)
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

        if (!r)
        {
           r = Physics2D.OverlapPointNonAlloc(groundCheck.position, results, armourBonusLayerMask) > 0;
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

            AppearDmgPoints(dmg, true, false);

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

            GameManager.Instance.playerHP = healthPoints;
            GameManager.Instance.playerAP = armourPoints;

            updateBars();

        }
    }

    private void AddCharacterLife(int life)
    {
        if(isAlive)
        {
            AppearDmgPoints(life, false, true);
            if (healthPoints < 100)
            {
                int h = life - healthPoints;

                healthPoints += h;

                life -= h;
            }

            if(life > 0)
            {
                armourPoints += life;

                if(armourPoints > 100)
                {
                    armourPoints = 100;
                }
            }

            GameManager.Instance.playerAP = armourPoints;
            GameManager.Instance.playerHP = healthPoints;

            updateBars();
        }
    }

    public void AddCharacterHpAp(int hp, int ap)
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

            GameManager.Instance.playerAP = armourPoints;
            GameManager.Instance.playerHP = armourPoints;

            updateBars();
            AppearDmgPoints(hp+ap,false, true);
        }
    }

    private void Defeat()
    {
        isAlive = false;
        GameManager.Instance.allowMoving = false;
        myAnimator.SetTrigger("Defeat");
    }

    private void updateBars()
    {
        lifebarImage.fillAmount = healthPoints / 100f;
        armourbarImage.fillAmount = armourPoints / 100f;
    }

    private void DetectArmourBonus()
    {
        Collider2D[] shieldsToDetect = Physics2D.OverlapCircleAll(attackPos.position, 0.1f, armourBonusLayerMask);

        if(shieldsToDetect.Length > 0)
        {
            for(int i = 0; i < shieldsToDetect.Length; i++)
            {
                // destroys shield
                shieldsToDetect[i].GetComponent<ArmourBonusController>().Destroy();

  
                AddCharacterLife(150);
            }
        }

    }

    private void AppearDmgPoints(int dmg, bool isTaken, bool isHealth) {
        GameObject dmgPoints = Instantiate(isTaken ? rDmgPoints : gDmgPoints, transform.position, Quaternion.identity);

        Renderer r = dmgPoints.GetComponent<Renderer>();

        TextMesh mr = dmgPoints.GetComponent<TextMesh>();

        r.sortingLayerName = "PlayerLayer";

        if(!isHealth) {

          mr.text = isTaken ? "-" + " " + dmg.ToString() : "+" + " " + dmg.ToString();

        } else {

          mr.text = dmg.ToString() + " Life";

        }

        StartCoroutine(WaitAndDestroy(dmgPoints));

    }
   private IEnumerator WaitAndDestroy(GameObject g)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(g);
    }
}
