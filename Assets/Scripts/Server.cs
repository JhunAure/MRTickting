using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Server
{
    private const string DATA_LOCATION = "/Saves/Passenger Data/";

    private static PassengerData GetPassengerData(string _guid)
    {
        string data = File.ReadAllText(Application.dataPath + DATA_LOCATION + _guid + ".txt");
        PassengerData passenger = JsonUtility.FromJson<PassengerData>(data);
        return passenger;
    }

    private static void SetPassengerData(string _guid, PassengerData newData)
    {
        string data = JsonUtility.ToJson(newData);
        File.WriteAllText(Application.dataPath + DATA_LOCATION + _guid + ".txt", data);
    }

    private static bool IsPassengerExists(string _guid)
    {
        return File.Exists(Application.dataPath + DATA_LOCATION + _guid + ".txt");
    }

    public static void SaveToDabase(string _guid, string _name, StationNames _stationName)
    {
        if(!IsPassengerExists(_guid))
        {
            PassengerData newData = new PassengerData()
            {
                guid = _guid,
                name = _name,
                stationIn = _stationName
            };
            SetPassengerData(_guid, newData);
        }
    }

    public static void PassengerEnter(string _guid, StationNames _stationName)
    {
        if (IsPassengerExists(_guid))
        {
            PassengerData passenger = GetPassengerData(_guid);

            if (!passenger.isOnBoard)
            {
                passenger.isOnBoard = true;
                passenger.stationIn = _stationName;
                SetPassengerData(_guid, passenger);
            }
            else
            {
                Debug.LogError("Error: Already inside");
            }
        }
    }

    public static void ProcessTransaction(string _guid, StationNames _station, StationMatrix _matrix)
    {
        if(IsPassengerExists(_guid))
        {
            PassengerData passenger = GetPassengerData(_guid);

            if(passenger.balance >= 13)//TODO consider replacing magic number to lowest price on matrix
            {
                if (!passenger.isOnBoard)
                {
                    passenger.isOnBoard = true;
                    passenger.intIsOnBoard = 1;
                    passenger.stationIn = _station;
                    SetPassengerData(_guid, passenger);
                    StationGate.QRProcessed?.Invoke(true);
                }
                else
                {
                    passenger.stationOut = _station;
                    bool isSuccessful = CalculateTransaction(passenger, _matrix, passenger.stationIn, passenger.stationOut);

                    if(isSuccessful)
                    {
                        passenger.isOnBoard = false;
                        passenger.intIsOnBoard = 0;
                        SetPassengerData(_guid, passenger);
                        StationGate.QRProcessed?.Invoke(true);
                    }
                    else
                    {
                        //TODO handle if balance is insufficient when exiting station
                        StationGate.QRProcessed?.Invoke(false);
                        Debug.LogError("Error: Insufficient Balance, please pay at the teller or load your account");
                    }
                }
            }
            else
            {
                Debug.LogError("Error: Insufficient Balance");
            }
        }
        else
        {
            Debug.LogError("Error: Passenger unknown");
        }
    }

    private static bool CalculateTransaction(PassengerData _data, StationMatrix _matrix, StationNames _in, StationNames _out)
    {
        foreach (var s in _matrix.GetMatrix()[0].stationMatrices)//TODO array not necessary: Consider removing after changing Matrix[] to Matrix
        {
            if (s.from.Equals(_in))
            {
                foreach (var a in s.stationToStations)
                {
                    if (a.to.Equals(_out))
                    {
                        if(_data.balance >= a.price)
                        {
                            float price = a.price;
                            _data.balance -= price;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}

[Serializable] public class PassengerData
{
    public string guid = "";
    public string name = "";
    public string pin = "";
    public float balance = 100f;
    public bool isOnBoard = false;
    public int intIsOnBoard = 0;
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
