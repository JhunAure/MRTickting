using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Server
{
    private const string DATA_LOCATION = "/Saves/Passenger Data/";

    public static void SaveToDabase(string _guid, string _name, StationNames _stationName)
    {
        if(!File.Exists(Application.dataPath + DATA_LOCATION + _guid +".txt"))
        {
            PassengerData newData = new PassengerData(){
                guid = _guid,
                name = _name,
                stationIn = _stationName
            };
            string data = JsonUtility.ToJson(newData);
            File.WriteAllText(Application.dataPath + DATA_LOCATION + _guid + ".txt", data);
        }
    }

    public static void PassengerEnter(string _guid, StationNames _stationName)
    {
        if(File.Exists(Application.dataPath + DATA_LOCATION + _guid + ".txt"))
        {
            string data = File.ReadAllText(Application.dataPath + DATA_LOCATION + _guid + ".txt");
            PassengerData passenger = JsonUtility.FromJson<PassengerData>(data);

            if(!passenger.isOnBoard)
            {
                passenger.isOnBoard = true;
                passenger.stationIn = _stationName;
            }
            else
            {
                Debug.LogError("Error: Already inside");
            }
        }
    }

    public static void CalculateBalance(float _balance, StationNames _station)
    {

    }
}

[Serializable] public class PassengerData
{
    public string guid = "";
    public string name = "";
    public string pin = "";
    public float balance = 0;
    public bool isOnBoard = false;
    public StationNames stationIn;
    public StationNames stationOut;
}

[Serializable] public enum StationNames
{
    NORTH_AVENUE,
    QUEZON_AVE,
    GMA_KAMUNING,
    CUBAO,
    SANTOLAN_ANNAPOLIS,
    ORTIGAS,
    SHAW_BOULEVARD,
    BONI,
    GUADALUPE,
    BUENDIA,
    AYALA,
    MAGALLANES,
    BACLARAN
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
