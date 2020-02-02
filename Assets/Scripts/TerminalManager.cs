using System;
using System.Collections;
using System.Collections.Generic;
using BTAI;
using CommandTerminal;
using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    // Start is called before the first frame update

    private List<GameObject> platforms = new List<GameObject>();
    public event Action<bool> OnPlatform;
    
    public static TerminalManager Instance
    {
        get { return s_Instance; }
    }

    protected static TerminalManager s_Instance;
    
    void Awake ()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            throw new UnityException("There cannot be more than one TerminalManager script.  The instances are " + s_Instance.name + " and " + name + ".");
    }
    void Start()
    {
            Terminal.Shell.AddCommand("PlatForm", PlatForm, 0, 2, "Manage Platforms");
    }
    
    public void RegisterPlatform(GameObject go)
    {
        if (!platforms.Contains(go))
        {
            platforms.Add(go);
        }
    }
    
    public void UnregisterPlatform(GameObject go)
    {
        Debug.Log("UNREGISTER");
        if (platforms.Contains(go))
        {
            platforms.Remove(go);
        }
    }

    private void PlatForm(CommandArg[] args)
    {

        if (args.Length == 0)
        {
            Terminal.Log("platForm list : show available platforms.");
                            Terminal.Log("platForm enable <number>: enable the requested platform.");
                            Terminal.Log("platForm disable <number> : disable the requested platform.");
                            return;
                                
        }
        switch (args[0].String.ToUpper())
        {
            case "LIST":
                Terminal.Log("Available platforms:");
                for (int i = 0; i < platforms.Count; i++)
                {
                    Terminal.Log(i + " : " + platforms[i].name);
                }
                break;
            case "ENABLE":
                 EnablePlatform(args);
                 break;
            case "DISABLE":
                DisablePlatform(args);
                break;
            default:
                Terminal.Log("platForm list : show available platforms.");
                Terminal.Log("platForm enable <number>: enable the requested platform.");
                Terminal.Log("platForm disable <number> : disable the requested platform.");
                break;
        }
        if (OnPlatform != null)
        {
            OnPlatform(true);
        }

    }

    private void EnablePlatform(CommandArg[] args)
    {
        var index = args[1].Int;
            if (Terminal.IssuedError)
            {
                return;
            }

            var go = platforms[index];
            go.SetActive(true);
    }
    
    private void DisablePlatform(CommandArg[] args)
    {
        var index = args[1].Int;
        if (Terminal.IssuedError)
        {
            return;
        }

        var go = platforms[index];
        go.SetActive(false);
    }

// Update is called once per frame
    void Update()
    {
        
    }
}
