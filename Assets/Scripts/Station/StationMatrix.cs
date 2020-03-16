using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationMatrix : MonoBehaviour
{
    [SerializeField] Matrix[] matrices; //TODO array not necessary: Consider removing to single Matrix

    public Matrix[] GetMatrix()
    {
        return matrices;
    }
}

[System.Serializable] public class Matrix
{
    public StationMatrices[] stationMatrices;
}

[System.Serializable] public class StationMatrices
{
    public StationNames from;
    public StationToStation[] stationToStations;
}

[System.Serializable] public class StationToStation
{
    public StationNames to;
    public float price = 0;
}