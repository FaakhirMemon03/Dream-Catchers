using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AlarmoAI : MonoBehaviour, IMonster
{
    public float moveSpeed = 2f;
    public float wanderRadius = 5f;
    public float changeTargetInterval = 3f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float timer;
    private bool isCaptured = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        startPosition = transform.position;
        SetNewTarget();
    }

    void Update()
    {
        if (isCaptured) return;

        timer += Time.deltaTime;
        if (timer >= changeTargetInterval)
        {
            SetNewTarget();
            timer = 0;
        }
    }

    void FixedUpdate()
    {
        if (isCaptured)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void SetNewTarget()
    {
        targetPosition = startPosition + Random.insideUnitCircle * wanderRadius;
    }

    public void OnCaptured()
    {
        isCaptured = true;
        
        Debug.Log("Alarmo: Oh no! I'm trapped in a bubble! *Ring Ring*");
        
        GameManager.Instance?.AddDreamDust(10);
        
        // Shrvel effect or animation would go here
        Destroy(gameObject, 1f);
    }
}
