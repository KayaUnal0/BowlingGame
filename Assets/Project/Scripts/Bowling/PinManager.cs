using UnityEngine;

public class PinManager : MonoBehaviour
{
    [Header("Pins")]
    [SerializeField] private Transform pinsParent;

    [Header("Fallen Detection")]
    [SerializeField] private float fallenAngleThreshold = 45f;
    [SerializeField] private float heightDropThreshold = 0.15f;
    [SerializeField] private float movementThreshold = 0.35f;

    [Header("Movement Detection")]
    [SerializeField] private float velocityThreshold = 0.02f;
    [SerializeField] private float angularVelocityThreshold = 0.02f;
    [SerializeField] private float disturbedMovementThreshold = 0.05f;
    [SerializeField] private float disturbedAngleThreshold = 4f;

    private Rigidbody[] pinRigidbodies;
    private Vector3[] startingUpDirections;
    private Vector3[] startingPositions;

    private void Awake()
    {
        if (pinsParent == null)
        {
            Debug.LogError("Pins Parent is not assigned.");
            return;
        }

        pinRigidbodies = pinsParent.GetComponentsInChildren<Rigidbody>();

        startingUpDirections = new Vector3[pinRigidbodies.Length];
        startingPositions = new Vector3[pinRigidbodies.Length];

        for (int i = 0; i < pinRigidbodies.Length; i++)
        {
            Transform pinTransform = pinRigidbodies[i].transform;

            startingUpDirections[i] = pinTransform.up;
            startingPositions[i] = pinTransform.position;
        }
    }

    public int GetFallenPinCount()
    {
        if (pinRigidbodies == null)
            return 0;

        int fallenCount = 0;

        for (int i = 0; i < pinRigidbodies.Length; i++)
        {
            Rigidbody pinRb = pinRigidbodies[i];

            if (pinRb == null)
                continue;

            Transform pinTransform = pinRb.transform;

            float angleDifference = Vector3.Angle(
                pinTransform.up,
                startingUpDirections[i]
            );

            float heightDrop = startingPositions[i].y - pinTransform.position.y;

            Vector3 startFlat = new Vector3(
                startingPositions[i].x,
                0f,
                startingPositions[i].z
            );

            Vector3 currentFlat = new Vector3(
                pinTransform.position.x,
                0f,
                pinTransform.position.z
            );

            float horizontalMovement = Vector3.Distance(startFlat, currentFlat);

            bool isTilted = angleDifference > fallenAngleThreshold;
            bool hasDropped = heightDrop > heightDropThreshold;
            bool hasMovedAway = horizontalMovement > movementThreshold;

            if (isTilted || hasDropped || hasMovedAway)
            {
                fallenCount++;
            }
        }

        return fallenCount;
    }

    public bool AreAnyPinsMoving()
    {
        if (pinRigidbodies == null)
            return false;

        foreach (Rigidbody pinRb in pinRigidbodies)
        {
            if (pinRb == null)
                continue;

            float linearSpeed = pinRb.linearVelocity.magnitude;
            float angularSpeed = pinRb.angularVelocity.magnitude;

            if (linearSpeed > velocityThreshold)
                return true;

            if (angularSpeed > angularVelocityThreshold)
                return true;
        }

        return false;
    }

    public bool HasAnyPinBeenDisturbed()
    {
        if (pinRigidbodies == null)
            return false;

        for (int i = 0; i < pinRigidbodies.Length; i++)
        {
            Rigidbody pinRb = pinRigidbodies[i];

            if (pinRb == null)
                continue;

            Transform pinTransform = pinRb.transform;

            Vector3 startFlat = new Vector3(
                startingPositions[i].x,
                0f,
                startingPositions[i].z
            );

            Vector3 currentFlat = new Vector3(
                pinTransform.position.x,
                0f,
                pinTransform.position.z
            );

            float horizontalMovement = Vector3.Distance(startFlat, currentFlat);

            float angleDifference = Vector3.Angle(
                pinTransform.up,
                startingUpDirections[i]
            );

            if (horizontalMovement > disturbedMovementThreshold)
                return true;

            if (angleDifference > disturbedAngleThreshold)
                return true;
        }

        return false;
    }
}