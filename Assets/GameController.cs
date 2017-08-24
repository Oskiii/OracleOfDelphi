using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [NonSerialized]
    public DebugFunctions Debug;

    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
        Debug = FindObjectOfType<DebugFunctions>();
    }
}
