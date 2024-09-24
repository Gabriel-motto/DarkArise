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
    private GameObject currentPlatformStanding;

    [SerializeField]
    Rigidbody2D player;

    public BoxCollider2D playerCollider;
    public SpriteRenderer spriteRenderer;
    public LayerMask enemyLayers;
    public Transform[] attackPoints;
    public AudioSource runAudio;
    public AudioSource attackAudio;

    public float attackRange = 0.5f;
    public int attackDmg = 40;
    public int jumps = 2;
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
        if (assassinAnimator != null)
        {
            //if (player.velocity.normalized.y == 0)
            //{
            //    grounded = true;
            //    jumps = 2;
            //    if (assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("fall"))
            //    {
            //        assassinAnimator.SetTrigger("TrLand");
            //    }
            //}
            // Attacks
            if (Input.GetKeyDown(KeyCode.J))
            {
                assassinAnimator.CrossFade("attack 1", 0, 0);
                if (!AudioManager.muted)
                {
                    attackAudio.Play();
                }
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                assassinAnimator.SetTrigger("TrAtk2");
                if (!AudioManager.muted)
                {
                    attackAudio.Play();
                }
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.L) && grounded)
            {
                assassinAnimator.SetTrigger("TrCrossSlice");
                if (!AudioManager.muted)
                {
                    attackAudio.Play();
                }
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && grounded)
            {
                assassinAnimator.SetTrigger("TrSliceAtk");
                if (!AudioManager.muted)
                {
                    attackAudio.Play();
                }
                Attack();
            }

            // Movement
            if (Input.GetAxisRaw("Horizontal") > 0 && canMove)
            {
                spriteRenderer.flipX = false;
                if (grounded)
                {
                    assassinAnimator.SetTrigger("TrRun");
                    if (!runAudio.isPlaying)
                    {
                        runAudio.Play();
                    }
                }
            }
            if (Input.GetAxisRaw("Horizontal") < 0 && canMove)
            {
                spriteRenderer.flipX = true;
                if (grounded)
                {
                    assassinAnimator.SetTrigger("TrRun");
                    if (!runAudio.isPlaying)
                    {
                        runAudio.Play();
                    }
                }
            }
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                assassinAnimator.ResetTrigger("TrRun");
                assassinAnimator.SetTrigger("TrStop");
            }
            if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
            {
                assassinAnimator.SetTrigger("TrJump");
            }
            if (player.velocity.normalized.y < 0)
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
                blasterGhost.transform.localScale = GetComponent<Transform>().localScale;
                blasterAnimator = blasterGhost.GetComponent<Animator>();

                if (blasterAnimator != null && blasterAnimator.runtimeAnimatorController != null)
                {
                    blasterGhost.SetActive(true);
                    blasterAnimator.CrossFade("attack", 0, 0);
                    Attack();
                }
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                blasterGhost = Instantiate(Resources.Load("BlasterPrefab"), transform.position, Quaternion.identity) as GameObject;
                blasterGhost.transform.localScale = GetComponent<Transform>().localScale;
                blasterAnimator = blasterGhost.GetComponent<Animator>();

                if (blasterAnimator != null && blasterAnimator.runtimeAnimatorController != null)
                {
                    blasterGhost.SetActive(true);
                    blasterAnimator.SetTrigger("TrSweep");
                    Attack();
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

        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            player.velocity = new Vector2(player.velocity.x, jumpHeight);
            jumps--;
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentPlatformStanding != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
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
            if (assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("fall"))
            {
                player.velocity = Vector2.zero;
                assassinAnimator.ResetTrigger("TrFall");
                assassinAnimator.SetTrigger("TrLand");
            }
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            grounded = true;
            jumps = 2;
            if (assassinAnimator.GetCurrentAnimatorStateInfo(0).IsName("fall"))
            {
                player.velocity = Vector2.zero;
                assassinAnimator.ResetTrigger("TrFall");
                assassinAnimator.SetTrigger("TrLand");
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
        assassinAnimator.SetTrigger("TrFall");
        BoxCollider2D platformCollider = currentPlatformStanding.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
