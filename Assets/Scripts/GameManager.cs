using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Stats")]
    public int dreamDustCount = 0;
    public int starsCount = 0;

    [Header("Events")]
    public UnityEvent<int> onDustChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddDreamDust(int amount)
    {
        dreamDustCount += amount;
        Debug.Log($"Dream Dust: {dreamDustCount}");
        onDustChanged?.Invoke(dreamDustCount);
    }

    public void AddStar()
    {
        starsCount++;
        Debug.Log($"Stars: {starsCount}");
    }

    public bool HasEnoughDust(int amount)
    {
        return dreamDustCount >= amount;
    }

    public void SpendDreamDust(int amount)
    {
        if (HasEnoughDust(amount))
        {
            dreamDustCount -= amount;
            onDustChanged?.Invoke(dreamDustCount);
        }
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
