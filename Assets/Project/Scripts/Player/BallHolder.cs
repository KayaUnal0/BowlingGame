using UnityEngine;

public class BallHolder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private BallController heldBall;

    [Header("Follow Settings")]
    [SerializeField] private float positionSmoothTime = 0.08f;
    [SerializeField] private float rotationFollowSpeed = 12f;

    private Rigidbody heldBallRigidbody;
    private Vector3 followVelocity;
    private bool isHoldingBall;

    public BallController HeldBall => heldBall;

    private void Awake()
    {
        if (heldBall != null)
        {
            heldBallRigidbody = heldBall.GetComponent<Rigidbody>();
        }
    }

    private void Start()
    {
        AttachBall();
    }

    private void LateUpdate()
    {
        if (!isHoldingBall)
            return;

        if (heldBall == null || holdPoint == null)
            return;

        SmoothFollowHoldPoint();
    }

    public void AttachBall()
    {
        if (heldBall == null || holdPoint == null)
            return;

        heldBallRigidbody = heldBall.GetComponent<Rigidbody>();

        if (heldBallRigidbody != null)
        {
            // First make sure the Rigidbody is not kinematic
            // so Unity allows us to clear velocity.
            heldBallRigidbody.isKinematic = false;

            heldBallRigidbody.linearVelocity = Vector3.zero;
            heldBallRigidbody.angularVelocity = Vector3.zero;

            // Now make it kinematic because the holder controls its position.
            heldBallRigidbody.isKinematic = true;
            heldBallRigidbody.useGravity = false;
        }

        heldBall.DeactivateControl();

        heldBall.transform.SetParent(null);

        heldBall.transform.position = holdPoint.position;
        heldBall.transform.rotation = holdPoint.rotation;

        followVelocity = Vector3.zero;
        isHoldingBall = true;
    }

    public void ReleaseBall()
    {
        if (heldBall == null)
            return;

        isHoldingBall = false;

        heldBall.transform.SetParent(null);

        if (heldBallRigidbody != null)
        {
            // When releasing, physics should control the ball.
            heldBallRigidbody.isKinematic = false;
            heldBallRigidbody.useGravity = true;

            heldBallRigidbody.linearVelocity = Vector3.zero;
            heldBallRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void SmoothFollowHoldPoint()
    {
        heldBall.transform.position = Vector3.SmoothDamp(
            heldBall.transform.position,
            holdPoint.position,
            ref followVelocity,
            positionSmoothTime
        );

        heldBall.transform.rotation = Quaternion.Slerp(
            heldBall.transform.rotation,
            holdPoint.rotation,
            rotationFollowSpeed * Time.deltaTime
        );
    }
}