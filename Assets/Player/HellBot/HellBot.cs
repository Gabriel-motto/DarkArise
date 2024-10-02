using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBot : MonoBehaviour
{

    [SerializeField] private Animator animator;
    GameObject gameObject;
    Rigidbody2D rb;


    [SerializeField] private Transform player;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform[] attackPoints;
    [SerializeField] private float attackRange = 0.7f;
    [SerializeField] private int attackDmg = 40;

    [SerializeField] private SpriteRenderer SpriteRenderer;

    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float activationRadius = 8f;

    private bool canMove = true;

    public int MaxHealth = 100;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject = GetComponent<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = MaxHealth;
    }

    private void Update()
    {

        Attack();

        Ai();

        if (currentHealth < 0)
        {
            EnemyDie();
        }
    }

    void OnDrawGizmosSelected()
    {
        foreach (Transform attackPoint in attackPoints)
        {
            if (attackPoint == null)
            {
                return;
            }

            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    private void Attack()
    {

        Collider2D[] playerHitted1 = Physics2D.OverlapCircleAll(attackPoints[0].position, attackRange, playerLayer);
        Collider2D[] playerHitted2 = Physics2D.OverlapCircleAll(attackPoints[1].position, attackRange, playerLayer);

        
        canMove = true;
        rb.bodyType = RigidbodyType2D.Dynamic;

        foreach (Collider2D player in playerHitted1)
        {
            player.GetComponent<PlayerController>().GetDmg(attackDmg);


            if (!animator.GetBool("isAttack"))
            {
                Utils.SetAllBoolFalse(animator);
            }
            canMove = false;
            animator.SetBool("isAttack", true);
            rb.bodyType = RigidbodyType2D.Static;

        }
        foreach (Collider2D player in playerHitted2)
        {
            Debug.Log(player.name);
            player.GetComponent<PlayerController>().GetDmg(attackDmg);

            if (!animator.GetBool("isAttack"))
            {
                Utils.SetAllBoolFalse(animator);

            }
            canMove = false;

            animator.SetBool("isAttack", true);
            rb.bodyType = RigidbodyType2D.Static;

        }

   

    }
    private void Ai()
    {
        if (canMove)
        {
            float distanceToPlayer = Vector3.Distance(rb.transform.position, player.position);

            if (distanceToPlayer < activationRadius)
            {
                rb.velocity = new Vector2(player.position.x - rb.transform.position.x, 0).normalized * chaseSpeed;
                if (rb.velocity.x > 0)
                {
                    SpriteRenderer.flipX = false;
                }
                else
                {
                    SpriteRenderer.flipX = true;
                }

                if (!animator.GetBool("isRuning"))
                {
                    Utils.SetAllBoolFalse(animator);
                }


                animator.SetBool("isRuning", true);
            }
            else
            {

                if (!animator.GetBool("isIdel"))
                {
                    Utils.SetAllBoolFalse(animator);

                }

                animator.SetBool("isIdel", true);
            }
        }
    }

    public void GetDmg(int playerAtkDmg)
    {
        Debug.Log("enemy hitted");
        currentHealth -= playerAtkDmg;
        animator.CrossFade("hit", 0, 0);
        rb.velocity = Vector3.zero;
    }

    public void EnemyDie()
    {
        animator.CrossFade("death", 0, 0);
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }
}
