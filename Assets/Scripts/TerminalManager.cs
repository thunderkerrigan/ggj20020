using System;
using System.Collections;
using System.Collections.Generic;
using BTAI;
using CommandTerminal;
using ggjj2020;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerminalManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera camera;
    private List<GameObject> platforms = new List<GameObject>();
    private GlitchEffect glitchEffect;
    private Coroutine glitchcoroutine;
    private Coroutine glitchCountdownCoroutine;
    public GameObject bg;
    public GameObject hd;
    
    private GameObject warningCanvas;
    
    public event Action<bool> OnPlatform;

    private bool isGlitched = false;

    public static TerminalManager Instance
    {
        get { return s_Instance; }
    }

    protected static TerminalManager s_Instance;

    void Awake()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            throw new UnityException("There cannot be more than one TerminalManager script.  The instances are " +
                                     s_Instance.name + " and " + name + ".");
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
        warningCanvas = GameObject.FindGameObjectWithTag("Respawn");
        StartCoroutine(ScrambleDecorations());
        Terminal.Shell.AddCommand("platform", PlatForm, 0, 2, "Manage Platforms. What was it even for ?");
        Terminal.Shell.AddCommand("cam", Camera, 1, 1, "Manage Camera. Those godamned bees messing with my brains!");
        Terminal.Shell.AddCommand("graphism", Graphism, 1, 1, "Manage Graphism. Let's get beautiful !");
        Terminal.Shell.AddCommand("respawn", Respawn, 0, 0, "I have a feeling of déjà-vu...");
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
            if (glitchEffect && isGlitched)
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
        glitchCountdownCoroutine = StartCoroutine(GlitchCountdown());
        glitchcoroutine = StartCoroutine(Glitch());
    }

    public void StopGlitch()
    {
        StopCoroutine(glitchcoroutine);
    }

    public void ResetGlitchCamera()
    {
        isGlitched = false;
        if (glitchCountdownCoroutine != null)
        {
            StopCoroutine(glitchCountdownCoroutine);
        }

        glitchCountdownCoroutine = StartCoroutine(GlitchCountdown());
        if (glitchEffect)
        {
            glitchEffect.intensity = 0;
            glitchEffect.flipIntensity = 0;
            glitchEffect.colorIntensity = 0;
        }
    }

    public void UnregisterPlatform(GameObject go)
    {
        if (platforms.Contains(go))
        {
            platforms.Remove(go);
        }
    }

    private void Respawn(CommandArg[] args)
    {
        var player = GameObject.FindWithTag("Player");
        warningCanvas.GetComponent<RetryWarning>().RetryLastCheckPoint();
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
        var pos = camera.transform.position;
        if (args.Length == 0)
        {
            Terminal.Log("cam reset : Reset camera view. Might get rid of those sneaky glitches.");
            Terminal.Log("cam zoom : zoom in. enhance... enhance... enhance...");
            Terminal.Log("cam unzoom : zoom out. Sometimes, all you need is to take a step back.");
            return;
                                
        }
        switch (args[0].String.ToUpper())
        {
            case "RESET":
                Terminal.Log("Cameras reset!");
                ResetGlitchCamera();
                break;
            case "ZOOM":
                pos.z = -13.5f;
                camera.transform.position = pos;
                Terminal.Log("We need to go deeper !");
                break;
            case "UNZOOM":
                pos.z = -20.5f;
                camera.transform.position = pos;
                Terminal.Log("let's go back!");
                break;
            default:
                Terminal.Log("cam reset : Reset camera view. Might get rid of those sneaky glitches.");
                Terminal.Log("cam zoom : zoom in. enhance... enhance... enhance...");
                Terminal.Log("cam unzoom : zoom out. Sometimes, all you need is to take a step back.");
                break;
        }
    }
    private void Graphism(CommandArg[] args)
    {

        if (args.Length == 0)
        {
            Terminal.Log("Graphism <low | medium | high> : set graphism settings. Candy eye for the soul!.");
            return;
                                
        }
        switch (args[0].String.ToUpper())
        {
            case "LOW":
                bg.SetActive(false);
                hd.SetActive(false);
                Terminal.Log("* GASP * Are you sure about that ?");
                break;
            case "MEDIUM":
                bg.SetActive(true);
                hd.SetActive(false);
                Terminal.Log("Guess that's your jam... not judging.");
                break;
            case "HIGH":
                bg.SetActive(true);
                hd.SetActive(true);
                Terminal.Log("😎 Now we're talking... that's smooth as heck 😎");
                break;
            default:
                Terminal.Log("Graphism <low | medium | high> : set graphism settings. Candy eye for the soul!.");

                break;
        }
    }

    private IEnumerator ScrambleDecorations()
    {
        yield return new WaitForSeconds(30);
        while (true)
        {
            foreach (Transform component in hd.transform)
            {
                var gos = component.transform;
                if (gos != null)
                {
                    foreach (Transform go in gos)
                    {
                        if (isGlitched)
                        {
                            var active = Random.value >= 0.5f;
                            go.gameObject.SetActive(active);    
                        }
                        else
                        {
                            go.gameObject.SetActive(true);
                        }
                        
                    }
                }
            }
            yield return new WaitForSeconds(2);
        }
    }

    private IEnumerator GlitchCountdown()
    {
        Debug.Log("start countdown");
        yield return new WaitForSeconds(10);
        Debug.Log("end countdown");
        isGlitched = true;
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
