
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WarningPath : MonoBehaviour
{
    public CharacterStatsSO basta;

    void Start()
    {
        Debug.Log("text");
        Debug.Log("[105-521]Corrupted File Finded : " +basta.path+"\nPlease make sure the variables are correct. \nErrorCode : 124:547JumpSpeed is not enough.");
        var textmeshPro = GetComponent<TMPro.TextMeshProUGUI>();
        
       // textmeshPro.SetText("")
        textmeshPro.SetText("[105-521]Corrupted File Finded : " +basta.path+"\nPlease make sure the variables are correct. \nErrorCode : 124:547JumpSpeed is not enough.");
        
    }
}