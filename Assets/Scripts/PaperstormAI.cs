using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PaperstormAI : MonoBehaviour, IMonster
{
    public float flySpeed = 3f;
    public float flutterStrength = 2f;
    
    private Rigidbody2D rb;
    private bool isCaptured = false;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        direction = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        if (isCaptured) return;

        // Fluttering movement
        Vector2 flutter = new Vector2(Mathf.Sin(Time.time * 5), Mathf.Cos(Time.time * 5)) * flutterStrength;
        rb.linearVelocity = direction * flySpeed + flutter;

        // Bounce off bounds (simple logic)
        if (Mathf.Abs(transform.position.x) > 10 || Mathf.Abs(transform.position.y) > 10)
        {
            direction = -transform.position.normalized;
        }
    }

    public void OnCaptured()
    {
        isCaptured = true;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Paperstorm: *Crinkle* No more homework today!");
        GameManager.Instance?.AddDreamDust(20);
        Destroy(gameObject, 0.8f);
    }
}
