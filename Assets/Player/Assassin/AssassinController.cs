using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SamuraiController : MonoBehaviour
{
    private Animator assassinAnimator;
    private GameObject blasterGhost;
    private Animator blasterAnimator;
    private BlasterAnimationEventHandler blasterAnimationEventHandler;
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
        grounded = false;
        isDead = false;
        isHitted = false;
        canMove = true;

        GameObject blasterPrefab = Instantiate(Resources.Load("BlasterPrefab")) as GameObject;
        blasterAnimator = blasterPrefab.GetComponent<Animator>();

        assassinAnimator = GetComponent<Animator>();

        EventSystem.current.OnAnimationEnd += DesactivarBlasterGhost;
        EventSystem.current.OnPlayerLand += onLand;
    }

    // Update is called once per frame
    void Update()
    {
        animate();
        input();
    }

    private void animate()
    {
        if (assassinAnimator != null)
        {
            // Attacks
            if (Input.GetKeyDown(KeyCode.J))
            {
                assassinAnimator.SetTrigger("TrAtk1");
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                assassinAnimator.SetTrigger("TrAtk2");
            }
            if (Input.GetKeyDown(KeyCode.L) && grounded)
            {
                assassinAnimator.SetTrigger("TrCrossSlice");
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && grounded)
            {
                assassinAnimator.SetTrigger("TrSliceAtk");
            }

            // Movement
            if (Input.GetAxisRaw("Horizontal") > 0 && canMove)
            {
                spriteRenderer.flipX = false;
                if (grounded)
                {
                    assassinAnimator.SetTrigger("TrRun");
                }
            }
            if (Input.GetAxisRaw("Horizontal") < 0 && canMove)
            {
                spriteRenderer.flipX = true;
                if (grounded)
                {
                    assassinAnimator.SetTrigger("TrRun");
                }
            }
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                assassinAnimator.ResetTrigger("TrRun");
                assassinAnimator.SetTrigger("TrStop");
            }
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                assassinAnimator.SetTrigger("TrJump");
            }
            if (!grounded && player.velocity.y < 0)
            {
                assassinAnimator.ResetTrigger("TrLand");
                assassinAnimator.SetTrigger("TrFall");
            }

            // Misc
            if (isDead)
            {
                assassinAnimator.SetTrigger("TrDeath");
            }
            if (isHitted)
            {
                assassinAnimator.SetTrigger("TrHit");
            }

            if (assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack 1") || assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack 2") || assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("cross slice")
                || assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slice attack") || assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("cross slice"))
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

            // Second character animations

            if (Input.GetKeyDown(KeyCode.U))
            {
                blasterGhost = Instantiate(Resources.Load("BlasterPrefab"), transform.position, Quaternion.identity) as GameObject;
                blasterAnimator = blasterGhost.GetComponent<Animator>();

                // Asegurar que el Animator del Blaster es válido
                if (blasterAnimator != null && blasterAnimator.runtimeAnimatorController != null)
                {
                    // Activar el "fantasma" del Blaster y reproducir la animación
                    blasterGhost.SetActive(true);
                    blasterAnimator.SetTrigger("TrAttack");
                }
                else
                {
                    Debug.LogError("Animator del Blaster no válido.");
                }
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
        assassinAnimator.SetTrigger("TrLand");
        player.velocity = Vector2.zero;
        grounded = true;
    }

    void DesactivarBlasterGhost()
    {
        Console.WriteLine("Hola");

        if (blasterGhost != null)
        {
            blasterGhost.SetActive(false);
            Destroy(blasterGhost);
            Console.WriteLine("Hola");
        }
    }
}
