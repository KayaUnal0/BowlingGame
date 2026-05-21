using UnityEngine;

public interface IGameInput
{
    Vector2 MoveInput { get; }
    Vector2 AimInput { get; }
    bool ThrowPressed { get; }
    bool ResetPressed { get; }
}