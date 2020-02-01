using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleLog : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject consoleUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Quote)){
            if (GameIsPaused)
            {
                Resume();
            }else{
                Pause();
            }
        }
    }

    public void Resume()
    {
        consoleUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        consoleUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Debug.Log("Game is paused ...");
    }
   
}
