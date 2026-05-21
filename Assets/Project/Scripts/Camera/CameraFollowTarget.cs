using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Transform ballTarget;

    [SerializeField] private Vector3 playerOffset = new Vector3(0f, 3f, -5f);
    [SerializeField] private Vector3 ballOffset = new Vector3(0f, 4f, -7f);

    [SerializeField] private float followSpeed = 6f;
    [SerializeField] private float rotationSpeed = 6f;

    private Transform currentTarget;
    private Vector3 currentOffset;

    private void Start()
    {
        GameManager.Instance.OnStageChanged += HandleStageChanged;
        HandleStageChanged(GameManager.Instance.CurrentStage);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStageChanged -= HandleStageChanged;
        }
    }

    private void LateUpdate()
    {
        if (currentTarget == null)
            return;

        Vector3 targetPosition = currentTarget.position + currentOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        Vector3 lookDirection = currentTarget.position - transform.position;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleStageChanged(GameStage stage)
    {
        if (stage == GameStage.PlayerSetup)
        {
            currentTarget = playerTarget;
            currentOffset = playerOffset;
        }
        else if (stage == GameStage.BallControl)
        {
            currentTarget = ballTarget;
            currentOffset = ballOffset;
        }
    }
}