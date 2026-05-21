using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private IGameInput gameInput;

    public IGameInput GameInput => gameInput;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gameInput = GetComponent<IGameInput>();

        if (gameInput == null)
        {
            Debug.LogError("No IGameInput component found on InputManager.");
        }
    }
}