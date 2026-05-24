using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum Type { DreamDust, Star, Sticker }
    public Type itemType;
    public int value = 1;

    public float rotationSpeed = 50f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Simple animation: Rotate and Float
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time * floatFrequency) * floatAmplitude, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        switch (itemType)
        {
            case Type.DreamDust:
                GameManager.Instance.AddDreamDust(value);
                break;
            case Type.Star:
                GameManager.Instance.AddStar();
                break;
        }

        // Play SFX/Particles (to be implemented)
        Debug.Log($"Collected {itemType}!");
        Destroy(gameObject);
    }
}
