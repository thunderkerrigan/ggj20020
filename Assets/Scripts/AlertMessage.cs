using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertMessage : MonoBehaviour
{
    public GameObject AlertUI;

    public static bool GameIsPaused = false;
    // Start is called before the first frame update



    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
                Debug.Log("Alert Message Missin Platforms ...");
                AlertUI.SetActive(true);
                Time.timeScale = 0f;
                GameIsPaused = true;
                
        }
    }

    public void Resume()
    {
        AlertUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }


}
