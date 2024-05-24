using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainContext : MonoBehaviour
    {
    public static MainContext Instance { get; private set; }
    public LevelHandler LevelHandler;

    private void Awake()
        {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
            {
            Destroy(this);
            }
        else
            {
            Instance = this;
            }
        }
    }
