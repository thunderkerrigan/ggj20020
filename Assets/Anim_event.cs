using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_event : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void animEvent()
    {
        player.SetActive(true);
    }
}
