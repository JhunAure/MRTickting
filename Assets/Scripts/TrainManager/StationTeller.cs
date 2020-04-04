using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace TrainManager
{
    public class StationTeller : MonoBehaviour
    {
        [SerializeField] GameObject loadButton = null;
        [SerializeField] GameObject newPassengerButton = null;
        [SerializeField] GameObject loadInputBox = null;
        [SerializeField] GameObject newPassengerInputBox = null;
        [SerializeField] TMP_InputField pinInput = null;
        [SerializeField] StationNames stationName;
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
            return stationName;
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
        #region Unity Button Click Events
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
        #endregion
    }
}