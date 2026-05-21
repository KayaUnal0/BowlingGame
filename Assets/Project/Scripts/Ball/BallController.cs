using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Ball Control")]
    [SerializeField] private float steeringForce = 8f;
    [SerializeField] private float forwardAssistForce = 2f;
    [SerializeField] private float maxSpeed = 18f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.6f;

    private Rigidbody rb;
    private bool canControl;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!canControl)
            return;

        if (!GameManager.Instance.IsStage(GameStage.BallControl))
            return;

        HandleBallControl();
        LimitSpeed();
    }

    public void ActivateControl()
    {
        canControl = true;
    }

    public void DeactivateControl()
    {
        canControl = false;
    }

    private void HandleBallControl()
    {
        Vector2 input = InputManager.Instance.GameInput.MoveInput;

        Vector3 steering = new Vector3(input.x, 0f, 0f) * steeringForce;
        Vector3 forwardAssist = Vector3.forward * input.y * forwardAssistForce;

        rb.AddForce(steering, ForceMode.Force);
        rb.AddForce(forwardAssist, ForceMode.Force);
    }

    private void LimitSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
}