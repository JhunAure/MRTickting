using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationTeller : MonoBehaviour
{
    private void Awake()
    {
        Server.InitializeDataPath();
    }
}