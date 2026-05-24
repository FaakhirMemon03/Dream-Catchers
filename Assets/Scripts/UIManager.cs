using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI dustText;
    public TextMeshProUGUI starsText;
    public GameObject pauseMenu;

    void Start()
    {
        UpdateUI(GameManager.Instance.dreamDustCount);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onDustChanged.AddListener(UpdateUI);
        }
    }

    void UpdateUI(int dust)
    {
        if (dustText != null) dustText.text = dust.ToString();
        if (starsText != null) starsText.text = GameManager.Instance.starsCount.ToString();
    }

    public void TogglePause()
    {
        bool isPaused = !pauseMenu.activeSelf;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void GoToDreamRoom()
    {
        Time.timeScale = 1;
        GameManager.Instance.ChangeScene("DreamRoom");
    }

    public void GoToJungle()
    {
        Time.timeScale = 1;
        GameManager.Instance.ChangeScene("CandyJungle");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.ChangeScene("MainMenu");
    }
}
