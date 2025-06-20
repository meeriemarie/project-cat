using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;
    public GameObject pauseMenuUI;
    public GameObject npcFootstepSource;

    void Awake()
    {
        GameIsPaused = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameIsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }

        }

    }

    public void Resume()
    {
        if (GameIsPaused)
        {
            GameIsPaused = false;
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    void Pause()
    {
        if (!GameIsPaused)
        {
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AkUnitySoundEngine.PostEvent("Stop_concrete_footsteps_6752", npcFootstepSource);
        }
    }

    public void LoadMenu()
    {
        StartCoroutine(StopSoundAndLoadScene());
    }

    private IEnumerator StopSoundAndLoadScene()
    {
        if (GameManager.Instance != null)
        {
            AkUnitySoundEngine.PostEvent("Stop_meow_meow_meow_tiktok", GameManager.Instance.gameObject);
        }
        // Wait a short moment even when time is paused
        yield return new WaitForSecondsRealtime(0.1f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }



    public void QuitGame()
    {
        Debug.Log("Quittttt");
        Application.Quit();
    }

}
