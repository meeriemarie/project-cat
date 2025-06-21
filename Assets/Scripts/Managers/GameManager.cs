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
        Debug.Log("Game over start");
        AkUnitySoundEngine.PostEvent("Play_meow_meow_meow_tiktok", gameObject);
        Debug.Log("Game over end");
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
                doorsToUnlockOnScore[i].OpenDoor();
            }
        }
    }

    public void OnGameWon()
    {
        if (gameOver) return;

        gameOver = true;
        Debug.Log("Game Won! Player escaped.");

        // Play a win sound (if you have one)
        AkUnitySoundEngine.PostEvent("Play_game_won", gameObject);

        // Trigger the Game Over UI with a “win” message
        if (gameOverUI != null)
            gameOverUI.TriggerGameWon();
    }

}
