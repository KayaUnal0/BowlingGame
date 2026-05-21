using UnityEngine;

public class KeyboardGameInput : MonoBehaviour, IGameInput
{
    public Vector2 MoveInput
    {
        get
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            return new Vector2(horizontal, vertical).normalized;
        }
    }

    public Vector2 AimInput
    {
        get
        {
            float horizontal = 0f;

            if (Input.GetKey(KeyCode.LeftArrow))
                horizontal = -1f;
            else if (Input.GetKey(KeyCode.RightArrow))
                horizontal = 1f;

            return new Vector2(horizontal, 0f);
        }
    }

    public bool ThrowPressed => Input.GetKeyDown(KeyCode.Space);

    public bool ResetPressed => Input.GetKeyDown(KeyCode.R);
}