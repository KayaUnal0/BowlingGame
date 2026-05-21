using UnityEngine;

public class PinManager : MonoBehaviour
{
    [SerializeField] private Pin[] pins;

    public int GetFallenPinCount()
    {
        int count = 0;

        foreach (Pin pin in pins)
        {
            if (pin != null && pin.IsFallen())
            {
                count++;
            }
        }

        return count;
    }
}