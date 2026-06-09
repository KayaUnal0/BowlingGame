using UnityEngine;
using UnityEngine.InputSystem;

public class MotionGameInput : MonoBehaviour, IGameInput
{
    [Header("Axis Settings")]
    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;
    [SerializeField] private float deadzone = 0.08f;

    private Gamepad gamepad;
    private Joystick joystick;

    private Vector2 currentInput;
    private bool throwPressedThisFrame;

    private void Update()
    {
        FindMotionController();
        ReadControllerInput();
        ReadThrowButton();
    }

    private void FindMotionController()
    {
        if (gamepad != null || joystick != null)
            return;

        if (Gamepad.all.Count > 0)
        {
            gamepad = Gamepad.all[0];
            return;
        }

        if (Joystick.all.Count > 0)
        {
            joystick = Joystick.all[0];
            return;
        }
    }

    private void ReadControllerInput()
    {
        currentInput = Vector2.zero;

        if (gamepad != null)
        {
            currentInput = gamepad.leftStick.ReadValue();
        }
        else if (joystick != null)
        {
            currentInput = joystick.stick.ReadValue();
        }

        if (invertX)
            currentInput.x *= -1f;

        if (invertY)
            currentInput.y *= -1f;

        if (Mathf.Abs(currentInput.x) < deadzone)
            currentInput.x = 0f;

        if (Mathf.Abs(currentInput.y) < deadzone)
            currentInput.y = 0f;
    }

    private void ReadThrowButton()
    {
        throwPressedThisFrame = false;

        if (gamepad != null)
        {
            throwPressedThisFrame = gamepad.buttonSouth.wasPressedThisFrame;
        }
        else if (joystick != null)
        {
            throwPressedThisFrame = joystick.trigger.wasPressedThisFrame;
        }
    }

    public Vector2 MoveInput
    {
        get
        {
            return currentInput;
        }
    }

    public Vector2 AimInput
    {
        get
        {
            return Vector2.zero;
        }
    }

    public bool ThrowPressed
    {
        get
        {
            return throwPressedThisFrame;
        }
    }

    public bool ResetPressed
    {
        get
        {
            if (Keyboard.current == null)
                return false;

            return Keyboard.current.rKey.wasPressedThisFrame;
        }
    }
}