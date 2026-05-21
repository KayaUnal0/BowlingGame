using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameStage CurrentStage { get; private set; }

    public event Action<GameStage> OnStageChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ChangeStage(GameStage.PlayerSetup);
    }

    public void ChangeStage(GameStage newStage)
    {
        CurrentStage = newStage;
        OnStageChanged?.Invoke(newStage);

        Debug.Log("Game stage changed to: " + newStage);
    }

    public bool IsStage(GameStage stage)
    {
        return CurrentStage == stage;
    }
}