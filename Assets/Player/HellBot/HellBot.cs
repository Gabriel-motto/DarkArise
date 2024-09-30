using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBot : MonoBehaviour
{
    Animator animator;
    GameObject gameObject;
    Rigidbody2D rb;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform[] attackPoints;
    [SerializeField] private float attackRange = 0.7f;
    [SerializeField] private int attackDmg = 40;

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

        Debug.Log(attackPoints + "atackpoints");
        Debug.Log(attackPoints.Length + "atackpoints leng");


        Collider2D[] playerHitted1 = Physics2D.OverlapCircleAll(attackPoints[0].position, attackRange, playerLayer);
        Collider2D[] playerHitted2 = Physics2D.OverlapCircleAll(attackPoints[1].position, attackRange, playerLayer);

        foreach (Collider2D player in playerHitted1)
        {
            Debug.Log(player.name+ "aa");
            player.GetComponent<PlayerController>().GetDmg(attackDmg);
        }
        foreach (Collider2D player in playerHitted2)
        {
            Debug.Log(player.name);
            player.GetComponent<PlayerController>().GetDmg(attackDmg);
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
