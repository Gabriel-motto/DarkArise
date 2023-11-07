using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D player;
    public SpriteRenderer spriteRenderer;

    public float speed;
    public float jumpHeight;
    private bool grounded;
    private bool isDead;
    private bool isHitted;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        speed = 6f;
        jumpHeight = 6f;
        grounded = false;
        isDead = false;
        isHitted = false;
        canMove = true;

        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        EventSystem.current.PlayerLand += onLand;
    }

    // Update is called once per frame
    void Update()
    {
        animate();
        input();
    }

    private void animate()
    {
        if (animator != null)
        {
            // Attacks
            if (Input.GetKeyDown(KeyCode.J))
            {
                animator.SetTrigger("TrAtk1");
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                animator.SetTrigger("TrAtk2");
            }
            if (Input.GetKeyDown(KeyCode.L) && grounded)
            {
                animator.SetTrigger("TrCrossSlice");
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && grounded)
            {
                animator.SetTrigger("TrSliceAtk");
            }

            // Movement
            if (Input.GetAxisRaw("Horizontal") > 0 && canMove)
            {
                spriteRenderer.flipX = false;
                if (grounded)
                {
                    animator.SetTrigger("TrRun");
                }
            }
            if (Input.GetAxisRaw("Horizontal") < 0 && canMove)
            {
                spriteRenderer.flipX = true;
                if (grounded)
                {
                    animator.SetTrigger("TrRun");
                }
            }
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                animator.ResetTrigger("TrRun");
                animator.SetTrigger("TrStop");
            }
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                animator.SetTrigger("TrJump");
            }
            if (!grounded && player.velocity.y < 0)
            {
                animator.ResetTrigger("TrLand");
                animator.SetTrigger("TrFall");
            }

            // Misc
            if (isDead)
            {
                animator.SetTrigger("TrDeath");
            }
            if (isHitted)
            {
                animator.SetTrigger("TrHit");
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack 1") || animator.GetCurrentAnimatorStateInfo(0).IsName("attack 2") || animator.GetCurrentAnimatorStateInfo(0).IsName("cross slice")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("Slice attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("cross slice"))
            {
                player.velocity = Vector2.zero;
                player.gravityScale = 0;
                canMove = false;
            }
            else
            {
                player.gravityScale = 1;
                canMove = true;
            }
        }
    }

    private void input()
    {
        if (canMove)
        {
            player.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, player.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            player.velocity = new Vector2(player.velocity.x, jumpHeight);
            grounded = false;
        }
    }

    private void onLand()
    {
        animator.SetTrigger("TrLand");
        player.velocity = Vector2.zero;
        grounded = true;
    }
}
