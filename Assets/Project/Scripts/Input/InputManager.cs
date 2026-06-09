using UnityEngine;
using UnityEngine.InputSystem;

public enum InputMode
{
    Keyboard,
    Motion
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Input Mode")]
    [SerializeField] private InputMode inputMode = InputMode.Keyboard;

    private KeyboardGameInput keyboardInput;
    private MotionGameInput motionInput;
    private IGameInput gameInput;

    public IGameInput GameInput => gameInput;
    public InputMode CurrentInputMode => inputMode;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        keyboardInput = GetComponent<KeyboardGameInput>();
        motionInput = GetComponent<MotionGameInput>();

        if (keyboardInput == null)
        {
            Debug.LogError("KeyboardGameInput component is missing on InputManager object.");
            return;
        }

        if (motionInput == null)
        {
            Debug.LogError("MotionGameInput component is missing on InputManager object.");
            return;
        }

        SetInputMode(inputMode);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleInputMode();
        }
    }

    public void SetInputMode(InputMode mode)
    {
        inputMode = mode;

        if (inputMode == InputMode.Keyboard)
        {
            gameInput = keyboardInput;
            Debug.Log("Input mode changed to Keyboard");
        }
        else
        {
            gameInput = motionInput;
            Debug.Log("Input mode changed to Motion");
        }
    }

    public void SetKeyboardMode()
    {
        SetInputMode(InputMode.Keyboard);
    }

    public void SetMotionMode()
    {
        SetInputMode(InputMode.Motion);
    }

    public void ToggleInputMode()
    {
        if (inputMode == InputMode.Keyboard)
            SetMotionMode();
        else
            SetKeyboardMode();
    }
}