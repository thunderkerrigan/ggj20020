using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons_Script : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animator>();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator startScene()
    {
        yield return new WaitForSeconds(35);
        SceneManager.LoadScene(1);
    }
    public void play()
    {

        anim.SetBool("State", true);
        IEnumerator ExampleCoroutine()
        {
            //Print the time of when the function is first called.
            Debug.Log("Started Coroutine at timestamp : " + Time.time);

            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(3);
        }
            Time.timeScale = 1;
            StartCoroutine(startScene());
    }
    
}
