using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject npcFootstepSource;


    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            
            if(GameIsPaused) {
                Resume();
            } else {
                Pause();
            }

        }
        
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AkUnitySoundEngine.PostEvent("Stop_concrete_footsteps_6752", npcFootstepSource);
        GameIsPaused = true;
     
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
         if (GameManager.Instance != null)
        {
        AkUnitySoundEngine.PostEvent("Stop_meow_meow_meow_tiktok", GameManager.Instance.gameObject);
        }
    }
 
    public void QuitGame() {
        Debug.Log("Quittttt");
        Application.Quit();
    }

}
