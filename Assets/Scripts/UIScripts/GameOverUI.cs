using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static bool GameIsOver = false;
    public GameObject npcFootstepSource;

    public GameObject gameOverUI;  

    private void Start()
    {
        // Make sure it's hidden at start
        gameOverUI.SetActive(false);
    }

    public void TriggerGameOver()
    {
        Debug.Log("Game Over triggered");
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        AkUnitySoundEngine.PostEvent("Stop_concrete_footsteps_6752", npcFootstepSource);
        GameIsOver = true;
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");

          if (GameManager.Instance != null)
        {
        AkUnitySoundEngine.PostEvent("Stop_meow_meow_meow_tiktok", GameManager.Instance.gameObject);
        }

        Time.timeScale = 1f;

        SceneManager.LoadScene(1);  // Load scene by build index
        GameIsOver = false;
        
    
}

}
