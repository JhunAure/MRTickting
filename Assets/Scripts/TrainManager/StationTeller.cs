using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace TrainManager
{
    public class StationTeller : MonoBehaviour
    {
        public static Func<float> OnRequestLoadAmount;

        [SerializeField] GameObject loadInputBox = null;

        private void Awake()
        {
            Server.InitializeDataPath();
        }

        void OnEnable()
        {
            QRTranslator.OnLoadingBalance += SetInputField;
            OnRequestLoadAmount += GetLoadAmount;
        }

        void OnDisable()
        {
            QRTranslator.OnLoadingBalance -= SetInputField;
            OnRequestLoadAmount -= GetLoadAmount;
        }

        private void SetInputField(bool status)
        {
            loadInputBox.SetActive(status);
        }

        private float GetLoadAmount()
        {
            string amount = loadInputBox.GetComponent<TMP_InputField>().text;
            return float.Parse(amount);
        }

        public void OnLoadBalanceSelected()
        {
            QRTranslator.OnLoadingBalance?.Invoke(true);
        }
    }
}