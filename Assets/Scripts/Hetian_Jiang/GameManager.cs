using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score Thresholds for Unlocking Doors")]
    public DoorController[] doorsToUnlockOnScore;
    public int[] scoreThresholds;

    private bool[] doorUnlocked;

    private bool gameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        doorUnlocked = new bool[doorsToUnlockOnScore.Length];
    }

    public void OnPlayerCaught()
    {
        if (gameOver) return;

        gameOver = true;
        Debug.Log("Game Over! Player was caught.");
        
        // TODO: Show Game Over UI here
    }
    public void CheckScoreForUnlock(int currentScore)
    {
        for (int i = 0; i < scoreThresholds.Length; i++)
        {
            if (!doorUnlocked[i] && currentScore >= scoreThresholds[i])
            {
                doorUnlocked[i] = true;
                doorsToUnlockOnScore[i].RegisterInteraction(); // or OpenDoor() directly if preferred
            }
        }
    }
}
