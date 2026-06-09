using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Transform ballTarget;
    [SerializeField] private Transform spectatorTarget;

    [Header("Offsets")]
    [SerializeField] private Vector3 playerOffset = new Vector3(0f, 3f, -5f);
    [SerializeField] private Vector3 ballOffset = new Vector3(0f, 4f, -7f);
    [SerializeField] private Vector3 spectatorOffset = new Vector3(0f, 7f, -12f);

    [Header("Follow Settings")]
    [SerializeField] private float followSpeed = 6f;
    [SerializeField] private float rotationSpeed = 6f;

    [Header("Spectator Settings")]
    [SerializeField] private float spectatorMoveSpeed = 1.5f;
    [SerializeField] private float spectatorLookHeight = 1.5f;

    private Transform currentTarget;
    private Vector3 currentOffset;
    private bool isSpectatorMode;
    private bool isFrozen;

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
        if (isFrozen)
            return;

        if (isSpectatorMode)
        {
            HandleSpectatorCamera();
            return;
        }

        HandleFollowCamera();
    }

    private void HandleFollowCamera()
    {
        if (currentTarget == null)
            return;

        Vector3 targetPosition = currentTarget.position + currentOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

        LookAtTarget(currentTarget.position);
    }

    private void HandleSpectatorCamera()
    {
        if (spectatorTarget == null)
            return;

        Vector3 targetPosition = spectatorTarget.position + spectatorOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            spectatorMoveSpeed * Time.deltaTime
        );

        Vector3 lookPosition = spectatorTarget.position + Vector3.up * spectatorLookHeight;
        LookAtTarget(lookPosition);
    }

    private void LookAtTarget(Vector3 targetPosition)
    {
        Vector3 lookDirection = targetPosition - transform.position;

        if (lookDirection == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void HandleStageChanged(GameStage stage)
    {
        isSpectatorMode = false;
        isFrozen = false;

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
        else if (stage == GameStage.Spectator)
        {
            isSpectatorMode = true;
        }
        else if (stage == GameStage.RoundFinished)
        {
            isFrozen = true;
        }
    }
}