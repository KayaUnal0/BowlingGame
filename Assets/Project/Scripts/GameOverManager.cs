using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverScreen;

    private void Awake()
    {
        Time.timeScale = 1f;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);
        else
            Debug.LogError("GameOverScreen is not assigned.");
    }

    public void Retry()
    {
        Debug.Log("Retry button clicked.");

        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}