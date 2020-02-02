using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameMaster gm;

    public int checkpointNumber;
    public CharacterStatsSO characterStats;

    void Start(){
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }
   void OnTriggerEnter2D(Collider2D other) {
       if(other.CompareTag("Player")){
           Debug.Log("Last Checkpoint ...");
           gm.lastCheckPointPos= transform.position;
           characterStats.ScrambleStats(checkpointNumber);
           
       }
   }

}
