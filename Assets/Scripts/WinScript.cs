using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        
     // can be Collider HardDick if you want.. I'm not judging you
    Debug.Log("its Colliding!"); //1 is the build order it could be 1065 for you if you have that many scenes
        if(other.CompareTag("Player"))
        {
                SceneManager.LoadScene(1);
                Debug.Log("its Colliding!"); //1 is the build order it could be 1065 for you if you have that many scenes
        }

    }
   

}
