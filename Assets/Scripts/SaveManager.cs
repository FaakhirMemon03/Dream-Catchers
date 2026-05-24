using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static void SaveGame()
    {
        if (GameManager.Instance != null)
        {
            PlayerPrefs.SetInt("DreamDust", GameManager.Instance.dreamDustCount);
            PlayerPrefs.SetInt("Stars", GameManager.Instance.starsCount);
            PlayerPrefs.Save();
            Debug.Log("Game Saved!");
        }
    }

    public static void LoadGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.dreamDustCount = PlayerPrefs.GetInt("DreamDust", 0);
            GameManager.Instance.starsCount = PlayerPrefs.GetInt("Stars", 0);
            Debug.Log("Game Loaded!");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveGame();
    }
}
