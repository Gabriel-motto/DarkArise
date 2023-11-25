using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBot : MonoBehaviour
{
    Animator animator;
    GameObject gameObject;
    Rigidbody2D rb;

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
        if (currentHealth < 0)
        {
            EnemyDie();
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
