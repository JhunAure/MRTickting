using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Server : MonoBehaviour
{
    private const string DATA_LOCATION = "/Passenger Data/";

    public static void SaveToDabase(string _guid, string _name, StationNames _stationName)
    {
        if(!File.Exists(Application.dataPath + DATA_LOCATION + _guid +".txt"))
        {
            PassengerData newData = new PassengerData(){
                guid = _guid,
                name = _name,
                stationName = _stationName
            };
            string data = JsonUtility.ToJson(newData);
            File.WriteAllText(Application.dataPath + DATA_LOCATION + _guid + ".txt", data);
        }
    }
}

[Serializable] public class PassengerData
{
    public string guid;
    public string name;
    public string pin;
    public bool isOnBoard = false;
    public StationNames stationName;
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Server : MonoBehaviour
{
    private const string DATA_LOCATION = "/PassengerData.txt";
    public static Database database = new Database();

    private void Awake()
    {
        if (!File.Exists(Application.dataPath + DATA_LOCATION))
        {
            string data = JsonUtility.ToJson(database);
            File.WriteAllText(Application.dataPath + DATA_LOCATION, data);
        }
    }

    public static void SaveToDabase(string _guid, string _name, StationNames _stationName)
    {
        string file = File.ReadAllText(Application.dataPath + DATA_LOCATION);
        Database currentDatabase = JsonUtility.FromJson<Database>(file);

        if(!IsPassengerExists(currentDatabase, _name))
        {
            PassengerData passengerData = new PassengerData()
            {
                name = _name,
                guid = _guid,
                stationName = _stationName
            };
            currentDatabase.passengerList.Add(passengerData);
            string data = JsonUtility.ToJson(currentDatabase);
            File.WriteAllText(Application.dataPath + DATA_LOCATION, data);
        }
        else
        {
            Debug.LogError("Sorry passenger name already exists");
        }
    }

    private static bool IsPassengerExists(Database db, string _name)
    {
        foreach (var data in db.passengerList)
        {
            if (data.name == _name) return true;
        }
        return false;
    }
}

[Serializable] public class Database
{
    public List<PassengerData> passengerList = new List<PassengerData>();
}

[Serializable] public class PassengerData
{
    public string guid;
    public string name;
    public string pin;
    public bool isOnBoard = false;
    public StationNames stationName;
}using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Server : MonoBehaviour
{
    private const string DATA_LOCATION = "/PassengerData.txt";
    public static Database database = new Database();

    private void Awake()
    {
        if (!File.Exists(Application.dataPath + DATA_LOCATION))
        {
            string data = JsonUtility.ToJson(database);
            File.WriteAllText(Application.dataPath + DATA_LOCATION, data);
        }
    }

    public static void SaveToDabase(string _guid, string _name, StationNames _stationName)
    {
        string file = File.ReadAllText(Application.dataPath + DATA_LOCATION);
        Database currentDatabase = JsonUtility.FromJson<Database>(file);

        if(!IsPassengerExists(currentDatabase, _name))
        {
            PassengerData passengerData = new PassengerData()
            {
                name = _name,
                guid = _guid,
                stationName = _stationName
            };
            currentDatabase.passengerList.Add(passengerData);
            string data = JsonUtility.ToJson(currentDatabase);
            File.WriteAllText(Application.dataPath + DATA_LOCATION, data);
        }
        else
        {
            Debug.LogError("Sorry passenger name already exists");
        }
    }

    private static bool IsPassengerExists(Database db, string _name)
    {
        foreach (var data in db.passengerList)
        {
            if (data.name == _name) return true;
        }
        return false;
    }
}

[Serializable] public class Database
{
    public List<PassengerData> passengerList = new List<PassengerData>();
}

[Serializable] public class PassengerData
{
    public string guid;
    public string name;
    public string pin;
    public bool isOnBoard = false;
    public StationNames stationName;
} 

*/
