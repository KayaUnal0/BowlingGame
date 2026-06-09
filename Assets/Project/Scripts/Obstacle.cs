using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        BallController ball = collision.collider.GetComponentInParent<BallController>();

        if (ball == null)
            return;

        Debug.Log("Ball hit obstacle. Game over.");

        ball.DeactivateControl();
        Destroy(ball.gameObject);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeStage(GameStage.Spectator);
        }

        GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();

        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
    }
}