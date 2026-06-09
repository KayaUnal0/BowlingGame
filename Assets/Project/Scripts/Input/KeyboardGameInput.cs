using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardGameInput : MonoBehaviour, IGameInput
{
    public Vector2 MoveInput
    {
        get
        {
            if (Keyboard.current == null)
                return Vector2.zero;

            Vector2 input = Vector2.zero;

            if (Keyboard.current.aKey.isPressed)
                input.x = -1f;
            else if (Keyboard.current.dKey.isPressed)
                input.x = 1f;

            if (Keyboard.current.wKey.isPressed)
                input.y = 1f;
            else if (Keyboard.current.sKey.isPressed)
                input.y = -1f;

            return input.normalized;
        }
    }

    public Vector2 AimInput
    {
        get
        {
            if (Keyboard.current == null)
                return Vector2.zero;

            float horizontal = 0f;

            if (Keyboard.current.leftArrowKey.isPressed)
                horizontal = -1f;
            else if (Keyboard.current.rightArrowKey.isPressed)
                horizontal = 1f;

            return new Vector2(horizontal, 0f);
        }
    }

    public bool ThrowPressed
    {
        get
        {
            if (Keyboard.current == null)
                return false;

            return Keyboard.current.spaceKey.wasPressedThisFrame;
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