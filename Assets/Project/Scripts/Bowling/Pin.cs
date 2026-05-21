using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private float fallenAngleThreshold = 45f;

    public bool IsFallen()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up);
        return angle > fallenAngleThreshold;
    }
}