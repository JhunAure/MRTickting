using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Passenger
{
    public class MobileServer
    {
        private const string DATA_LOCATION = "/Mobile Saves/Passenger Data/";

        public static bool IsQRCodeExists()
        {
            var files = Directory.GetFiles(Application.persistentDataPath + DATA_LOCATION);
            return files.Length > 0;
        }

        public static void SaveQRCode(string _guid, PassengerData _data)
        {
            if(!IsQRCodeExists())
            {
                string data = JsonUtility.ToJson(_data);
                File.WriteAllText(Application.persistentDataPath + DATA_LOCATION + _guid + ".txt", data);
            }
            else
            {
                Debug.LogError("There is already existing qr for this passenger");
            }
            MobileQRController.CameraDetecting?.Invoke(false);
        }

        public static string GetQRCode()
        {
            if (Directory.Exists(Application.persistentDataPath + DATA_LOCATION))
            {
                var files = Directory.GetFiles(Application.persistentDataPath + DATA_LOCATION);

                foreach (var _file in files) //returns the first filename in the array of files inside the Directory
                {
                    string data = File.ReadAllText(_file);
                    PassengerData passengerData = JsonUtility.FromJson<PassengerData>(data);
                    return passengerData.guid;
                }
            }
            return null;
        }

        public static void InitializeDataPath()
        {
            if (!Directory.Exists(Application.persistentDataPath + DATA_LOCATION))
            {
                Directory.CreateDirectory(Application.persistentDataPath + DATA_LOCATION);
            }
        }
    }

    [System.Serializable] public struct PassengerData
    {
        public string guid;
    }
}