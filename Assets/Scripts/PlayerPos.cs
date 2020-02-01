using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    private GameMaster gm;
    private GameObject warningCanvas;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        warningCanvas = GameObject.FindGameObjectWithTag("Respawn");
        transform.position = gm.lastCheckPointPos;
    }
    

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter2D(Collider2D other) {
       if(other.CompareTag("Enemy")){
           warningCanvas.GetComponent<RetryWarning>().Pause();
           Debug.Log("Dead ...");
       }
   }
}
