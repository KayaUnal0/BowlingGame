using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TMP_Text pinsText;
    [SerializeField] private TMP_Text recordText;

    private const string RecordKey = "BestPins";

    private void Awake()
    {
        if (winScreen != null)
            winScreen.SetActive(false);
    }

    public void ShowWinScreen(int fallenPins)
    {
        int record = PlayerPrefs.GetInt(RecordKey, 0);

        if (fallenPins > record)
        {
            record = fallenPins;
            PlayerPrefs.SetInt(RecordKey, record);
            PlayerPrefs.Save();
        }

        if (pinsText != null)
            pinsText.text = fallenPins + " PINS";

        if (recordText != null)
            recordText.text = "Record: " + record + " pins";

        if (winScreen != null)
            winScreen.SetActive(true);
    }

    public void TryAgain()
    {
        Debug.Log("TRY AGAIN CLICKED");

        Time.timeScale = 1f;

        UnityEngine.SceneManagement.Scene currentScene =
            UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }
}