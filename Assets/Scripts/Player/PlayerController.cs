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

    //Working on progress
    private GameObject blasterGhost;
    private Animator blasterAnimator;
    private GameObject currentPlatformStanding;
    private GameObject blasterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        grounded = false;
        isDead = false;
        isHitted = false;
        canMove = true;

        blasterPrefab = Instantiate(Resources.Load("BlasterPrefab")) as GameObject;
        blasterAnimator = blasterPrefab.GetComponent<Animator>();

        //EventSystem.current.OnPlayerLand += onLand;
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
        animate();
        input();
    }

    private void animate()
    {
        //if (assassinAnimator != null)
        //{
        //    //if (player.velocity.normalized.y == 0)
        //    //{
        //    //    grounded = true;
        //    //    jumps = 2;
        //    //    if (assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("fall"))
        //    //    {
        //    //        assassinAnimator.SetTrigger("TrLand");
        //    //    }
        //    //}
        //    // Attacks
        //    if (Input.GetKeyDown(KeyCode.J))
        //    {
        //        assassinAnimator.CrossFade("attack 1", 0, 0);
        //        if (!AudioManager.muted)
        //        {
        //            attackAudio.Play();
        //        }
        //        Attack();
        //    }
        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        assassinAnimator.SetTrigger("TrAtk2");
        //        if (!AudioManager.muted)
        //        {
        //            attackAudio.Play();
        //        }
        //        Attack();
        //    }
        //    if (Input.GetKeyDown(KeyCode.L) && grounded)
        //    {
        //        assassinAnimator.SetTrigger("TrCrossSlice");
        //        if (!AudioManager.muted)
        //        {
        //            attackAudio.Play();
        //        }
        //        Attack();
        //    }
        //    if (Input.GetKeyDown(KeyCode.LeftShift) && grounded)
        //    {
        //        assassinAnimator.SetTrigger("TrSliceAtk");
        //        if (!AudioManager.muted)
        //        {
        //            attackAudio.Play();
        //        }
        //        Attack();
        //    }

        //    // Movement
        //    if (Input.GetAxisRaw("Horizontal") > 0 && canMove)
        //    {
        //        spriteRenderer.flipX = false;
        //        if (grounded)
        //        {
        //            assassinAnimator.SetTrigger("TrRun");
        //            if (!runAudio.isPlaying)
        //            {
        //                runAudio.Play();
        //            }
        //        }
        //    }
        //    if (Input.GetAxisRaw("Horizontal") < 0 && canMove)
        //    {
        //        spriteRenderer.flipX = true;
        //        if (grounded)
        //        {
        //            assassinAnimator.SetTrigger("TrRun");
        //            if (!runAudio.isPlaying)
        //            {
        //                runAudio.Play();
        //            }
        //        }
        //    }
        //    if (Input.GetAxisRaw("Horizontal") == 0)
        //    {
        //        assassinAnimator.ResetTrigger("TrRun");
        //        assassinAnimator.SetTrigger("TrStop");
        //    }
        //    if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        //    {
        //        assassinAnimator.SetTrigger("TrJump");
        //    }
        //    if (player.velocity.normalized.y < 0)
        //    {
        //        assassinAnimator.ResetTrigger("TrLand");
        //        assassinAnimator.SetTrigger("TrFall");
        //    }

        //    // Misc
        //    if (isDead)
        //    {
        //        assassinAnimator.SetTrigger("TrDeath");
        //    }
        //    if (isHitted)
        //    {
        //        assassinAnimator.SetTrigger("TrHit");
        //    }

        //    if (assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack 1") || assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack 2") || assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("cross slice")
        //        || assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slice attack") || assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("cross slice"))
        //    {
        //        player.velocity = Vector2.zero;
        //        player.gravityScale = 0;
        //        canMove = false;
        //    }
        //    else
        //    {
        //        player.gravityScale = 1;
        //        canMove = true;
        //    }

        //    // Second character animations

        //    if (Input.GetKeyDown(KeyCode.U))
        //    {
        //        blasterGhost = Instantiate(Resources.Load("BlasterPrefab"), transform.position, Quaternion.identity) as GameObject;
        //        blasterGhost.transform.localScale = GetComponent<Transform>().localScale;
        //        blasterAnimator = blasterGhost.GetComponent<Animator>();

        //        if (blasterAnimator != null && blasterAnimator.runtimeAnimatorController != null)
        //        {
        //            blasterGhost.SetActive(true);
        //            blasterAnimator.CrossFade("attack", 0, 0);
        //            Attack();
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.I))
        //    {
        //        blasterGhost = Instantiate(Resources.Load("BlasterPrefab"), transform.position, Quaternion.identity) as GameObject;
        //        blasterGhost.transform.localScale = GetComponent<Transform>().localScale;
        //        blasterAnimator = blasterGhost.GetComponent<Animator>();

        //        if (blasterAnimator != null && blasterAnimator.runtimeAnimatorController != null)
        //        {
        //            blasterGhost.SetActive(true);
        //            blasterAnimator.SetTrigger("TrSweep");
        //            Attack();
        //        }
        //    }
        //}
    }


    private void input()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    playerSpriteRenderer = blasterPrefab.GetComponent<SpriteRenderer>();
        //    PlayerAnimator = blasterAnimator;
        //}

        if (player.velocity.normalized == Vector2.zero && grounded)
        {
            Utils.SetAllBoolFalse(PlayerAnimator);
            PlayerAnimator.SetBool("IsIdle", true);

            //if (!PlayerAnimator.GetBool("IsLanding"))
            //{
            //    Utils.SetAllBoolFalse(PlayerAnimator);
            //}
            //if (!grounded)
            //{
            //    grounded = true;
            //    jumps = 2;
            //    Utils.SetAllBoolFalse(PlayerAnimator);
            //    PlayerAnimator.SetBool("IsLanding", true);
            //}
            //else
            //{
            //    Utils.SetAllBoolFalse(PlayerAnimator);
            //    PlayerAnimator.SetBool("IsIdle", true);
            //}
        }

        if (canMove)
        {
            player.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, player.velocity.y);

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
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            player.velocity = new Vector2(player.velocity.x, jumpHeight);
            jumps--;
            grounded = false;
            Utils.SetAllBoolFalse(PlayerAnimator);
            PlayerAnimator.SetBool("IsJumping", true);
        }
        if (player.velocity.normalized.y < 0)
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

        Debug.Log(grounded);
    }

    private void Attack()
    {
        Collider2D[] enemiesHitted = Physics2D.OverlapCircleAll(attackPoints[0].position, attackRange, enemyLayers);
        Collider2D[] enemiesHitted2 = Physics2D.OverlapCircleAll(attackPoints[1].position, attackRange, enemyLayers);

        foreach (Collider2D enemy in enemiesHitted)
        {
            Debug.Log(enemy.name);
            enemy.GetComponent<HellBot>().GetDmg(attackDmg);
        }
        foreach (Collider2D enemy in enemiesHitted2)
        {
            Debug.Log(enemy.name);
            enemy.GetComponent<HellBot>().GetDmg(attackDmg);
        }
    }
    //private void onLand()
    //{
    //    assassinAnimator.SetTrigger("TrLand");
    //    player.velocity = Vector2.zero;
    //    grounded = true;
    //}

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Running;
    }

    void OnDrawGizmosSelected()
    {
        foreach(Transform attackPoint in attackPoints)
        {
            if (attackPoint == null)
            {
                return;
            }

            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
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
                PlayerAnimator.ResetTrigger("TrFall");
                PlayerAnimator.SetTrigger("TrLand");
            }
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (!grounded)
            {
                grounded = true;
                jumps = 2;
                Utils.SetAllBoolFalse(PlayerAnimator);
                //PlayerAnimator.SetBool("IsLanding", true);
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
        PlayerAnimator.SetTrigger("TrFall");
        BoxCollider2D platformCollider = currentPlatformStanding.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
