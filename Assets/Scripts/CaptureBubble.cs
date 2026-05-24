using UnityEngine;

public class CaptureBubble : MonoBehaviour
{
    public float expandSpeed = 2f;
    public float maxRadius = 3f;
    public float lifeTime = 2f;

    void Start()
    {
        transform.localScale = Vector3.zero;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (transform.localScale.x < maxRadius)
        {
            transform.localScale += Vector3.one * expandSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            IMonster monster = other.GetComponent<IMonster>();
            if (monster != null)
            {
                monster.OnCaptured();
                Debug.Log("Monster captured in bubble!");
            }
        }
    }
}
