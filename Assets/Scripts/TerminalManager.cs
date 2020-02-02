using System;
using System.Collections;
using System.Collections.Generic;
using BTAI;
using CommandTerminal;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerminalManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera camera;
    private List<GameObject> platforms = new List<GameObject>();
    private GlitchEffect glitchEffect;
    private Coroutine glitchcoroutine;
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
        // var camera = Camera.current;
        if (camera != null)
        {
            Debug.Log("got cam");
            glitchEffect = camera.GetComponent<GlitchEffect>();
            StartGlitch();
        }
        Terminal.Shell.AddCommand("PlatForm", PlatForm, 0, 2, "Manage Platforms");
        Terminal.Shell.AddCommand("cam", Camera, 1, 1, "Manage Camera");
    }
    
    public void RegisterPlatform(GameObject go)
    {
        if (!platforms.Contains(go))
        {
            platforms.Add(go);
        }
    }

    private IEnumerator Glitch()
    {
        while (true)
        {
            if (glitchEffect)
            {
                glitchEffect.intensity = Math.Min(glitchEffect.intensity + Random.Range(0, 0.08f), 1f);
                glitchEffect.flipIntensity = Math.Min(glitchEffect.flipIntensity + Random.Range(0, 0.08f), 1f);
                glitchEffect.colorIntensity = Math.Min(glitchEffect.colorIntensity + Random.Range(0, 0.08f), 1f);    
            }
            yield return new WaitForSeconds(6);
        }
    }

    public void StartGlitch()
    {
        glitchcoroutine = StartCoroutine(Glitch());
    }
    
    public void StopGlitch()
    {
        StopCoroutine(glitchcoroutine);
    }

    public void ResetGlitchCamera()
    {
        if (glitchEffect)
        {
        glitchEffect.intensity = 0;
        glitchEffect.flipIntensity = 0;
        glitchEffect.colorIntensity = 0;
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
    
    private void Camera(CommandArg[] args)
    {

        if (args.Length == 0)
        {
            Terminal.Log("cam reset : Reset camera view. Might get rid of those sneaky glitches.");
            return;
                                
        }
        switch (args[0].String.ToUpper())
        {
            case "RESET":
                Terminal.Log("Cameras reset!");
                ResetGlitchCamera();
                break;
            default:
                Terminal.Log("camera reset : Reset camera view. Might get rid of those sneaky glitches.");
                break;
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
