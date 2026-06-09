using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private PinManager pinManager;
    [SerializeField] private WinManager winManager;

    [Header("Automatic Scoring")]
    [SerializeField] private float minimumWaitAfterThrow = 1.5f;
    [SerializeField] private float minimumWaitAfterFirstHit = 3f;
    [SerializeField] private float requiredStillTime = 2.5f;
    [SerializeField] private float maxWaitTime = 25f;

    private bool scoreCalculated = false;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStageChanged += HandleStageChanged;
        }
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
        if (stage == GameStage.BallControl && !scoreCalculated)
        {
            StartCoroutine(CalculateScoreWhenAllPinsStop());
        }
    }

    private IEnumerator CalculateScoreWhenAllPinsStop()
    {
        scoreCalculated = true;

        float totalTimer = 0f;
        float stillTimer = 0f;
        float timeSinceFirstHit = 0f;

        bool pinsHaveBeenHit = false;

        while (totalTimer < maxWaitTime)
        {
            yield return new WaitForFixedUpdate();

            totalTimer += Time.fixedDeltaTime;

            if (totalTimer < minimumWaitAfterThrow)
                continue;

            if (!pinsHaveBeenHit)
            {
                if (pinManager != null && pinManager.HasAnyPinBeenDisturbed())
                {
                    pinsHaveBeenHit = true;
                    timeSinceFirstHit = 0f;
                    stillTimer = 0f;
                }

                continue;
            }

            timeSinceFirstHit += Time.fixedDeltaTime;

            if (timeSinceFirstHit < minimumWaitAfterFirstHit)
            {
                stillTimer = 0f;
                continue;
            }

            bool anyPinMoving = pinManager != null && pinManager.AreAnyPinsMoving();

            if (anyPinMoving)
            {
                stillTimer = 0f;
            }
            else
            {
                stillTimer += Time.fixedDeltaTime;

                if (stillTimer >= requiredStillTime)
                {
                    break;
                }
            }
        }

        int fallenPins = 0;

        if (pinManager != null)
        {
            fallenPins = pinManager.GetFallenPinCount();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeStage(GameStage.RoundFinished);
        }

        if (winManager != null)
        {
            winManager.ShowWinScreen(fallenPins);
        }
        else
        {
            Debug.LogError("WinManager is not assigned in ScoreManager.");
        }
    }
}