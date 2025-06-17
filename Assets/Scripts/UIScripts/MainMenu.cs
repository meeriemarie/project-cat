using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void PlayGame() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    // still need to index the scenes (File -> Build Settings -> Add Scenes to Queue 
    // Menu - Game...)
  }

  public void QuitGame() {
    Debug.Log("Quitting");
    Application.Quit();
  }
}
