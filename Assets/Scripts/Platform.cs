using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool startActive = true;
    private TerminalManager manager;

    private void Start()
    {
        manager = TerminalManager.Instance;
        manager.RegisterPlatform(gameObject);
        gameObject.SetActive(startActive);
    }

    private void OnDestroy()
    {
        manager.UnregisterPlatform(gameObject);
    }
}
