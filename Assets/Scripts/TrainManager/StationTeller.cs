using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace TrainManager
{
    public class StationTeller : MonoBehaviour
    {
        [SerializeField] StationNames stationName;
        [SerializeField] StationMatrix stationMatrix = null;
        [SerializeField] TMP_Dropdown stationSelector = null;
        [SerializeField] GameObject loadButton = null;
        [SerializeField] GameObject newPassengerButton = null;
        [SerializeField] GameObject clearDisplayButton = null;
        [SerializeField] GameObject loadInputBox = null;
        [SerializeField] GameObject newPassengerInputBox = null;
        [SerializeField] TMP_InputField pinInput = null;
        [SerializeField] TMP_Text stationNameText = null;
        [SerializeField] int minPinCount = 6;
        [SerializeField] int minNameCount = 4;

        private void Awake()
        {
            Server.InitializeDataPath();
        }

        void OnEnable()
        {
            QRTranslator.OnLoadingBalance += SetLoadingInputBox;
            QRGenerator.OnSetNewPassenger += SetNewPassengerInputBox;
        }

        void OnDisable()
        {
            QRTranslator.OnLoadingBalance -= SetLoadingInputBox;
            QRGenerator.OnSetNewPassenger -= SetNewPassengerInputBox;
        }

        void Start()
        {
            var stations = stationMatrix.GetMatrix();

            for(int i = 0; i < stations[0].stationMatrices.Length; i++)
            {
                var newItem = new TMP_Dropdown.OptionData();
                newItem.text = stations[0].stationMatrices[i].from.ToString();
                stationSelector.options.Add(newItem);
            }
        }

        private void SetLoadingInputBox(bool status)
        {
            loadInputBox.SetActive(status);
            if(status == false)
                ResetInputFields(loadInputBox);
        }

        private void SetNewPassengerInputBox(bool status)
        {
            newPassengerInputBox.SetActive(status);
            if (status == false)
            {
                ResetInputFields(newPassengerInputBox);
                ResetInputFields(pinInput.gameObject);
            }
        }

        private void ResetInputFields(GameObject inputBox)
        {
            inputBox.GetComponent<TMP_InputField>().text = null;
        }

        public void SetInterfaceButtons(bool status)
        {
            loadButton.SetActive(status);
            newPassengerButton.SetActive(status);
            clearDisplayButton.SetActive(status);
            stationSelector.gameObject.SetActive(status);
        }

        public void SetInputFields(bool status)
        {
            SetLoadingInputBox(status);
            SetNewPassengerInputBox(status);
        }

        public float GetLoadAmount()
        {
            string amount = loadInputBox.GetComponent<TMP_InputField>().text;
            amount = string.IsNullOrEmpty(amount)? "0": amount;
            return float.Parse(amount);
        }

        public string GetPassengerName()
        {
            string passengerName = newPassengerInputBox.GetComponent<TMP_InputField>().text;
            return passengerName;
        }

        public StationNames GetStationName()
        {
            int i = stationSelector.value;
            StationNames parsedEnum = (StationNames)Enum.Parse(typeof(StationNames), stationSelector.options[i].text);
            return parsedEnum;
        }

        public string GetPin()
        {
            string pin = pinInput.GetComponent<TMP_InputField>().text;
            return pin;
        }

        public bool isInputsValid()
        {
            if (GetPin().Length < minPinCount || string.IsNullOrEmpty(GetPassengerName()) || GetPassengerName().Length < minNameCount)
            {
                Debug.LogError("Please enter valid inputs");
                Debug.LogError($"minimun Pin: {minPinCount}");
                Debug.LogError($"minimun Name Chars: {minNameCount}");
                Debug.LogError($"Do not leave empty input box");
                return false;
            }
            return true;
        }

        #region Unity Events

        public void OnChangeStationSelectorValue()
        {
            Debug.Log(GetStationName().ToString());
        }

        public void OnCreateNewPassenger()
        {
            QRGenerator.OnSetNewPassenger?.Invoke(true);
            SetInterfaceButtons(false);
        }

        public void OnLoadBalanceSelected()
        {
            QRTranslator.OnLoadingBalance?.Invoke(true);
            SetInterfaceButtons(false);
        }

        public void OnCancelSelected()
        {
            ResetInputFields(newPassengerInputBox);
            ResetInputFields(loadInputBox);
            SetInputFields(false);
            OnClearBarcodeSelected();
            QRTranslator.OnLoadingBalance?.Invoke(false);
            QRGenerator.OnSetNewPassenger?.Invoke(false);
        }

        public void OnClearBarcodeSelected()
        {
            QRGenerator.OnResetBarcode?.Invoke();
        }

        public void CloseAppSelected()
        {
            Application.Quit();
        }
        #endregion
    }
}