using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player variables
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Animator PlayerAnimator;

    //Stats
    [SerializeField] private int MaxHealth = 100;
    int currentHealth;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDmg = 40;
    [SerializeField] private int jumps = 2;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;

    //States
    private bool grounded;
    private bool isDead;
    private bool isHitted;
    private bool canMove;

    //Misc
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Transform[] attackPoints;
    [SerializeField] private AudioSource runAudio;
    [SerializeField] private AudioSource attackAudio;
    private GameObject currentPlatformStanding;

    // Start is called before the first frame update
    void Start()
    {
        grounded = false;
        isDead = false;
        isHitted = false;
        canMove = true;

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy()
    {
        //EventSystem.current.OnPlayerLand -= onLand;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        attackAudio.mute = AudioManager.muted;
        attackAudio.volume = AudioManager.volume;
        AttackInput();
        MovementInput();
    }

    private void AttackInput()
    {
        // Attacks
        if (Input.GetKeyDown(KeyCode.J))
        {
            canMove = false;

            PlayerAnimator.SetBool("IsOrbAttack", true);
            if (!AudioManager.muted)
            {
                //attackAudio.Play();
            }
            //OrbAttack();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            canMove = false;
            PlayerAnimator.SetBool("IsShieldDown", true);
        }
        //if (Input.GetKeyUp(KeyCode.K))
        //{
        //    if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        //    {
        //        Utils.SetAllBoolFalse(PlayerAnimator);
        //        PlayerAnimator.SetBool("IsShieldUp", true);
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.L) && grounded)
        {
            canMove = false;
            PlayerAnimator.SetBool("IsShockwave", true);
            if (!AudioManager.muted)
            {
                //attackAudio.Play();
            }
            //Shockwave();
        }

        // Misc
        if (isDead)
        {
            Utils.SetAllBoolFalse(PlayerAnimator);
            PlayerAnimator.SetBool("IsDead", true);
        }
        if (isHitted)
        {
            Utils.SetAllBoolFalse(PlayerAnimator);
            PlayerAnimator.SetBool("IsHitted", true);
        }

        if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Orb attack") || PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("shield down") || PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shield up")
            || PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("shield hold") || PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shockwave"))
        {
            player.velocity = Vector2.zero;
            player.gravityScale = 0;
        }
        else
        {
            player.gravityScale = 1;
            canMove = true;
        }
    }


    private void MovementInput()
    {

        if (canMove)
        {
            player.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, player.velocity.y);

            if (player.velocity.normalized == Vector2.zero && grounded)
            {
                PlayerAnimator.SetBool("IsIdle", true);
            }

            if (player.velocity.normalized == Vector2.left)
            {
                Utils.SetAllBoolFalse(PlayerAnimator);
                PlayerAnimator.SetBool("IsRunning", true);
            }
            if (player.velocity.normalized == Vector2.right)
            {
                Utils.SetAllBoolFalse(PlayerAnimator);
                PlayerAnimator.SetBool("IsRunning", true);
            }
            if (player.velocity.x < 0)
            {
                playerSpriteRenderer.flipX = true;
            }
            if (player.velocity.x > 0)
            {
                playerSpriteRenderer.flipX = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
            {
                player.velocity = new Vector2(player.velocity.x, jumpHeight);
                jumps--;
                grounded = false;
                Utils.SetAllBoolFalse(PlayerAnimator);
                PlayerAnimator.SetBool("IsJumping", true);
            }

            if (player.velocity.normalized.y < -.1f)
            {
                Utils.SetAllBoolFalse(PlayerAnimator);
                PlayerAnimator.SetBool("IsFalling", true);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (currentPlatformStanding != null)
                {
                    StartCoroutine(DisableCollision());
                }
            }
        }
        Debug.Log(canMove + " aaa");
    }

    public void GetDmg(int playerAtkDmg)
    {
        Debug.Log("me muero");
        Debug.Log(currentHealth + "sss");

        currentHealth -= playerAtkDmg;

    }

    private void OrbAttack()
    {
        Collider2D[] enemiesHittedLeft = Physics2D.OverlapCircleAll(attackPoints[0].position, attackRange, enemyLayers);
        Collider2D[] enemiesHittedRight = Physics2D.OverlapCircleAll(attackPoints[1].position, attackRange, enemyLayers);

        foreach (Collider2D enemy in enemiesHittedLeft)
        {
            Debug.Log(enemy.name);
            enemy.GetComponent<HellBot>().GetDmg(attackDmg);
        }
        foreach (Collider2D enemy in enemiesHittedRight)
        {
            Debug.Log(enemy.name);
            enemy.GetComponent<HellBot>().GetDmg(attackDmg);
        }
    }

    private void Shockwave()
    {
        Collider2D[] enemiesHitted = Physics2D.OverlapCircleAll(attackPoints[2].position, attackRange, enemyLayers);

        foreach (Collider2D enemy in enemiesHitted)
        {
            Debug.Log(enemy.name);
            enemy.GetComponent<HellBot>().GetDmg(attackDmg);
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Running;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlat"))
        {
            currentPlatformStanding = collision.gameObject;
            grounded = true;
            jumps = 2;
            if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("fall"))
            {
                player.velocity = Vector2.zero;
                Utils.SetAllBoolFalse(PlayerAnimator);
            }
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (!grounded)
            {
                grounded = true;
                jumps = 2;
                Utils.SetAllBoolFalse(PlayerAnimator);
            }
            else
            {
                Utils.SetAllBoolFalse(PlayerAnimator);
                PlayerAnimator.SetBool("IsIdle", true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlat"))
        {
            currentPlatformStanding = null;
            grounded = false;
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            grounded = false;
        }
    }

    private IEnumerator DisableCollision()
    {
        grounded = false;
        Utils.SetAllBoolFalse(PlayerAnimator);
        PlayerAnimator.SetBool("IsFalling", true);
        BoxCollider2D platformCollider = currentPlatformStanding.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
