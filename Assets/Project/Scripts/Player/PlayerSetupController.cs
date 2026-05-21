using UnityEngine;

public class PlayerSetupController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform laneReference;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Lane Limits")]
    [SerializeField] private float minSidePosition;
    [SerializeField] private float maxSidePosition;

    private float currentSidePosition;

    private void Start()
    {
        if (laneReference == null)
        {
            Debug.LogError("PlayerSetupController needs a Lane Reference assigned.");
            return;
        }

        Vector3 localPosition = laneReference.InverseTransformPoint(transform.position);
        currentSidePosition = localPosition.x;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsStage(GameStage.PlayerSetup))
            return;

        if (laneReference == null)
            return;

        HandleMovement();
        HandleAiming();
        HandleThrowInput();
    }

    private void HandleMovement()
    {
        Vector2 input = InputManager.Instance.GameInput.MoveInput;

        currentSidePosition += input.x * moveSpeed * Time.deltaTime;
        currentSidePosition = Mathf.Clamp(currentSidePosition, minSidePosition, maxSidePosition);

        Vector3 localPosition = laneReference.InverseTransformPoint(transform.position);
        localPosition.x = currentSidePosition;

        transform.position = laneReference.TransformPoint(localPosition);
    }

    private void HandleAiming()
    {
        Vector2 aimInput = InputManager.Instance.GameInput.AimInput;

        float rotationAmount = aimInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationAmount, Space.World);
    }

    private void HandleThrowInput()
    {
        if (InputManager.Instance.GameInput.ThrowPressed)
        {
            BallThrower thrower = GetComponent<BallThrower>();

            if (thrower != null)
            {
                thrower.ThrowBall();
            }
            else
            {
                Debug.LogError("No BallThrower found on player.");
            }
        }
    }
}