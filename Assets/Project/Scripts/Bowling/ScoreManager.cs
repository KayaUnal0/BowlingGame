using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private PinManager pinManager;
    [SerializeField] private float scoreDelay = 6f;

    private void Start()
    {
        GameManager.Instance.OnStageChanged += HandleStageChanged;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStageChanged -= HandleStageChanged;
        }
    }

    private void HandleStageChanged(GameStage stage)
    {
        if (stage == GameStage.BallControl)
        {
            StartCoroutine(CalculateScoreAfterDelay());
        }
    }

    private IEnumerator CalculateScoreAfterDelay()
    {
        yield return new WaitForSeconds(scoreDelay);

        int fallenPins = pinManager.GetFallenPinCount();

        Debug.Log("Pins knocked down: " + fallenPins);

        GameManager.Instance.ChangeStage(GameStage.RoundFinished);
    }
}