using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score Thresholds for Unlocking Doors")]
    public DoorController[] doorsToUnlockOnScore;
    public int[] scoreThresholds;

    private bool[] doorUnlocked;

    public bool IsGameOver => gameOver;

    private bool gameOver = false;

    public GameOverUI gameOverUI; 


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

        AkUnitySoundEngine.PostEvent("Play_meow_meow_meow_tiktok", gameObject);

        if (gameOverUI != null)
        gameOverUI.TriggerGameOver();
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
