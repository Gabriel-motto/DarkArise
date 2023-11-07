using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlasterController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D player;
    public SpriteRenderer spriteRenderer;

    public float speed;
    private bool isDead;
    private bool isHitted;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        speed = 6f;
        isDead = false;
        isHitted = false;
        canMove = true;

        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                animator.SetTrigger("TrAttack");
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                animator.SetTrigger("TrSweep");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                animator.SetTrigger("TrHeal");
            }

            // Movement
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                spriteRenderer.flipX = false;
                animator.SetTrigger("TrMove");
            }
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                spriteRenderer.flipX = true;
                animator.SetTrigger("TrMove");
            }
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                animator.ResetTrigger("TrMove");
                animator.SetTrigger("TrStop");
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

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("sweep") || animator.GetCurrentAnimatorStateInfo(0).IsName("heal"))
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
    }
}
