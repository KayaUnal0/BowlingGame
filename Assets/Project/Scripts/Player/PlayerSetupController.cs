using UnityEngine;

public class PlayerSetupController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform laneReference;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("Lane Limits")]
    [SerializeField] private float minSidePosition;
    [SerializeField] private float maxSidePosition;

    private float currentSidePosition;
    private BallThrower thrower;

    private void Awake()
    {
        thrower = GetComponent<BallThrower>();
    }

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
        if (GameManager.Instance == null)
            return;

        if (!GameManager.Instance.IsStage(GameStage.PlayerSetup))
            return;

        if (laneReference == null)
            return;

        if (InputManager.Instance == null || InputManager.Instance.GameInput == null)
            return;

        HandleSideMovement();
        HandleThrowInput();
    }

    private void HandleSideMovement()
    {
        Vector2 input = InputManager.Instance.GameInput.MoveInput;

        currentSidePosition += input.x * moveSpeed * Time.deltaTime;
        currentSidePosition = Mathf.Clamp(currentSidePosition, minSidePosition, maxSidePosition);

        Vector3 localPosition = laneReference.InverseTransformPoint(transform.position);
        localPosition.x = currentSidePosition;

        transform.position = laneReference.TransformPoint(localPosition);
    }

    private void HandleThrowInput()
    {
        if (!InputManager.Instance.GameInput.ThrowPressed)
            return;

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