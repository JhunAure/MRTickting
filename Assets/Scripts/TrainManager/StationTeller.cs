using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrainManager
{
    public class StationTeller : MonoBehaviour
    {
        private void Awake()
        {
            Server.InitializeDataPath();
        }
    }
}