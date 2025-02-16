using UnityEngine;

public class Freezer : MonoBehaviour
{
    public static Freezer Instance { get; private set; }
    public bool IsGameFrozen { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void FreezeGame()
    {
        IsGameFrozen = true;
    }
}