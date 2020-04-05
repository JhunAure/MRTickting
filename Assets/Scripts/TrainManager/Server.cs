using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace TrainManager
{
    public class Server
    {
        public delegate void OnSetDisplayMessage(bool isActive, string name, string desc, string amount, Color color);

        private const string DATA_LOCATION = "/Saves/Passenger Data/";

        private static PassengerData GetPassengerData(string _guid)
        {
            string data = File.ReadAllText(Application.persistentDataPath + DATA_LOCATION + _guid + ".txt");
            PassengerData passenger = JsonUtility.FromJson<PassengerData>(data);
            return passenger;
        }

        private static void SetUpdatePassengerData(string _guid, PassengerData newData)
        {
            string data = JsonUtility.ToJson(newData);
            File.WriteAllText(Application.persistentDataPath + DATA_LOCATION + _guid + ".txt", data);
        }

        private static bool IsPassengerExists(string _guid)
        {
            return File.Exists(Application.persistentDataPath + DATA_LOCATION + _guid + ".txt");
        }

        private static bool CalculateTransaction(PassengerData _data, StationMatrix _matrix, StationNames _in, StationNames _out)
        {
            foreach (var s in _matrix.GetMatrix()[0].stationMatrices)
            {
                if (s.from.Equals(_in))
                {
                    foreach (var a in s.stationToStations)
                    {
                        if (a.to.Equals(_out))
                        {
                            if (_data.balance >= a.price)
                            {
                                float price = a.price == 0? 13: a.price; //Deduct 13 if in and out is the same station
                                _data.balance -= price;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static void InitializeDataPath()
        {
            if (!Directory.Exists(Application.persistentDataPath + DATA_LOCATION))
            {
                Directory.CreateDirectory(Application.persistentDataPath + DATA_LOCATION);
            }
        }

        public static void SaveToDabase(string _guid, string _name, StationNames _stationName, string _date, string _pin)
        {
            if (!IsPassengerExists(_guid))
            {
                PassengerData newData = new PassengerData()
                {
                    guid = _guid,
                    name = _name,
                    stationIn = _stationName,
                    date = _date,
                    pin = _pin
                };
                SetUpdatePassengerData(_guid, newData);
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
                    SetUpdatePassengerData(_guid, passenger);
                }
                else
                {
                    Debug.LogError("Error: Already inside");
                }
            }
        }

        public static void LoadBalance(string _guid, float _amount, OnSetDisplayMessage onComplete = null)
        {
            if (IsPassengerExists(_guid))
            {
                PassengerData passenger = GetPassengerData(_guid);

                if (_amount > 0)
                {
                    passenger.balance += _amount;
                    SetUpdatePassengerData(_guid, passenger);
                    QRTranslator.OnLoadingBalance?.Invoke(false);
                    StationGate.QRProcessed?.Invoke(true, "Load Success", Color.cyan);
                    onComplete?.Invoke(true, passenger.name, "Thankyou!, your current balance is", passenger.balance.ToString(), Color.cyan);
                }
                else
                {
                    StationGate.QRProcessed?.Invoke(true, "Load Failed", Color.red);
                    onComplete?.Invoke(true, passenger.name, "Sorry invalid input amount", "Please try again", Color.red);
                }
            }
            else
            {
                StationGate.QRProcessed?.Invoke(true, "Denied", Color.red);
                onComplete?.Invoke(true, "Unknown", "QR Code is not registered in this system", "Please register at the teller", Color.red);
            }
        }

        public static void ProcessTransaction(string _guid, StationNames _station, StationMatrix _matrix, OnSetDisplayMessage onComplete = null)
        {
            if (IsPassengerExists(_guid))
            {
                PassengerData passenger = GetPassengerData(_guid);

                if (passenger.balance >= 13)//TODO consider replacing magic number to lowest price on matrix
                {
                    if (!passenger.isOnBoard)
                    {
                        passenger.isOnBoard = true;
                        passenger.stationIn = _station;
                        SetUpdatePassengerData(_guid, passenger);
                        StationGate.QRProcessed?.Invoke(true, "Accepted", Color.green);
                        onComplete?.Invoke(true, passenger.name, "Welcome!, your current balance is: ", passenger.balance.ToString(), Color.green);
                    }
                    else
                    {
                        passenger.stationOut = _station;
                        bool isSuccessful = CalculateTransaction(passenger, _matrix, passenger.stationIn, passenger.stationOut);

                        if (isSuccessful)
                        {
                            passenger.isOnBoard = false;
                            SetUpdatePassengerData(_guid, passenger);
                            StationGate.QRProcessed?.Invoke(true, "Accepted", Color.green);
                            onComplete?.Invoke(true, passenger.name, "Thankyou!, your current balance is: ", passenger.balance.ToString(), Color.green);
                        }
                        else
                        {
                            //TODO handle if balance is insufficient when exiting station
                            StationGate.QRProcessed?.Invoke(false, "Denied", Color.red);
                            onComplete?.Invoke(true, passenger.name, "Sorry your current balance is insufficient, please load at the teller: ", passenger.balance.ToString(), Color.red);
                        }
                    }
                }
                else
                {
                    StationGate.QRProcessed?.Invoke(false, "Denied", Color.red);
                    onComplete?.Invoke(true, passenger.name, "Sorry your current balance is insufficient, please load at the teller: ", passenger.balance.ToString(), Color.red);
                }
            }
            else
            {
                StationGate.QRProcessed?.Invoke(false, "Denied", Color.red);
                onComplete?.Invoke(true, "Unknown", "QR Code is not registered in this system", "Please register at the teller", Color.red);
            }
        }
    }

    [Serializable] public class PassengerData
    {
        public string guid = "";
        public string name = "";
        public string pin = "";
        public string date = "";
        public float balance = 100f;
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
}