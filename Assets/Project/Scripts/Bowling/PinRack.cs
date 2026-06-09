using UnityEngine;

public class PinRack : MonoBehaviour
{
    [Header("Pin References")]
    [SerializeField] private Transform[] pins = new Transform[10];

    [Header("Rack Settings")]
    [SerializeField] private float centerSpacing = 1.2f;
    [SerializeField] private float pinYPosition = 0f;

    [Header("Options")]
    [SerializeField] private bool arrangeOnStart = true;

    private void Start()
    {
        if (arrangeOnStart)
        {
            ArrangePins();
        }
    }

    [ContextMenu("Arrange Pins")]
    public void ArrangePins()
    {
        if (pins == null || pins.Length < 10)
        {
            Debug.LogError("PinRack needs exactly 10 pin references.");
            return;
        }

        float rowSpacing = centerSpacing * 0.8660254f;

        Vector3[] localPositions =
        {
            new Vector3(0f, pinYPosition, 0f),

            new Vector3(-centerSpacing / 2f, pinYPosition, rowSpacing),
            new Vector3( centerSpacing / 2f, pinYPosition, rowSpacing),

            new Vector3(-centerSpacing, pinYPosition, rowSpacing * 2f),
            new Vector3(0f, pinYPosition, rowSpacing * 2f),
            new Vector3(centerSpacing, pinYPosition, rowSpacing * 2f),

            new Vector3(-centerSpacing * 1.5f, pinYPosition, rowSpacing * 3f),
            new Vector3(-centerSpacing * 0.5f, pinYPosition, rowSpacing * 3f),
            new Vector3( centerSpacing * 0.5f, pinYPosition, rowSpacing * 3f),
            new Vector3( centerSpacing * 1.5f, pinYPosition, rowSpacing * 3f),
        };

        for (int i = 0; i < 10; i++)
        {
            if (pins[i] == null)
                continue;

            pins[i].SetParent(transform);
            pins[i].localPosition = localPositions[i];
            pins[i].localRotation = Quaternion.identity;
        }
    }
}