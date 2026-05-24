using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BroccolooAI : MonoBehaviour, IMonster
{
    public float moveSpeed = 1.5f;
    public float cryInterval = 4f;
    
    private Rigidbody2D rb;
    private bool isCaptured = false;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        if (isCaptured) return;

        timer += Time.deltaTime;
        if (timer >= cryInterval)
        {
            Cry();
            timer = 0;
        }

        // Broccoloo moves slowly and randomly
        if (rb.linearVelocity.magnitude < 0.1f)
        {
            rb.linearVelocity = Random.insideUnitCircle.normalized * moveSpeed;
        }
    }

    void Cry()
    {
        Debug.Log("Broccoloo: *Sniff Sniff* Why does nobody like veggies?");
        // Logic for crying effect (e.g., slowing player nearby)
    }

    public void OnCaptured()
    {
        isCaptured = true;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Broccoloo: I'm in a bubble... is it made of candy?");
        GameManager.Instance?.AddDreamDust(15);
        Destroy(gameObject, 1.2f);
    }
}
