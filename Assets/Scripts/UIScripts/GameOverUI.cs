using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static bool GameIsOver = false;
    public GameObject npcFootstepSource;

    public GameObject gameOverUI;
    public GameObject gameWonUI;

    private void Start()
    {
        // Make sure it's hidden at start
        gameOverUI.SetActive(false);
    }

    public void TriggerGameOver()
    {
        Debug.Log("Game Over triggered");
        AkUnitySoundEngine.PostEvent("Stop_concrete_footsteps_6752", npcFootstepSource);
        GameIsOver = true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TriggerGameWon()
    {
        Debug.Log("Game Won triggered");
        AkUnitySoundEngine.PostEvent("Stop_concrete_footsteps_6752", npcFootstepSource);
        GameIsOver = true;
        gameWonUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");

        if (GameManager.Instance != null)
        {
            AkUnitySoundEngine.PostEvent("Stop_meow_meow_meow_tiktok", GameManager.Instance.gameObject);
        }

        Time.timeScale = 1f;
        GameIsOver = false;
        SceneManager.LoadScene(1);  // Load scene by build inde
    }

}
