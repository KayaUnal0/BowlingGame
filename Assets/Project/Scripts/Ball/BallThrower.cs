using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [SerializeField] private BallHolder ballHolder;
    [SerializeField] private float throwForce = 12f;
    [SerializeField] private float upwardForce = 1f;

    public void ThrowBall()
    {
        if (ballHolder == null || ballHolder.HeldBall == null)
        {
            Debug.LogError("BallHolder or held ball is missing.");
            return;
        }

        BallController ball = ballHolder.HeldBall;

        ballHolder.ReleaseBall();

        Rigidbody rb = ball.GetComponent<Rigidbody>();

        Vector3 throwDirection = transform.forward;
        Vector3 force = throwDirection * throwForce + Vector3.up * upwardForce;

        rb.AddForce(force, ForceMode.Impulse);

        ball.ActivateControl();

        GameManager.Instance.ChangeStage(GameStage.BallControl);
    }
}