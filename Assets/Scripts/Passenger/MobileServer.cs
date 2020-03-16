using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MobileServer
{
    private const string DATA_LOCATION = "/Mobile Saves/Passenger Data/";

    private static PassengerData GetPassengerData(string _guid)
    {
        string data = File.ReadAllText(Application.persistentDataPath + DATA_LOCATION + _guid + ".txt");
        PassengerData passenger = JsonUtility.FromJson<PassengerData>(data);
        return passenger;
    }

    private static void SetPassengerData(string _guid, PassengerData newData)
    {
        string data = JsonUtility.ToJson(newData);
        File.WriteAllText(Application.persistentDataPath + DATA_LOCATION + _guid + ".txt", data);
    }

    public static void InitializeDataPath()
    {
        if (!Directory.Exists(Application.persistentDataPath + DATA_LOCATION))
        {
            Directory.CreateDirectory(Application.persistentDataPath + DATA_LOCATION);
        }
    }
}
