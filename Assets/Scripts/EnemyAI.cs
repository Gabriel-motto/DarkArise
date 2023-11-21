using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 5f;
    public float activationRadius = 8f;
    public Animator animator;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(rb.transform.position, player.position);

        if (distanceToPlayer < activationRadius)
        {
            rb.velocity = new Vector2(player.position.x - rb.transform.position.x, 0).normalized * chaseSpeed;
            if (rb.velocity.x > 0)
            {
                spriteRenderer.flipX = false;
            } else
            {
                spriteRenderer.flipX = true;
            }
            anim.SetTrigger("TrRun");
        } else
        {
            anim.SetTrigger("TrStop");
        }
    }
}
