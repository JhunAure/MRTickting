using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace TrainManager
{
    public class StationGate : MonoBehaviour
    {
        public static Action<bool, string, Color> QRProcessed;

        [SerializeField] TextMeshProUGUI statusText = null;
        [SerializeField] Image statusImage = null;
        [SerializeField] GameObject messageContatiner = null;
        [SerializeField] TMP_Text passengerNameMsgText = null;
        [SerializeField] TMP_Text descriptionMsgText = null;
        [SerializeField] TMP_Text amountMsgText = null;

        private void OnEnable()
        {
            QRProcessed += SetStatuses;
        }

        private void OnDisable()
        {
            QRProcessed -= SetStatuses;
        }

        private void Start()
        {
            ResetStatuses();
        }

        private void SetStatuses(bool _isAccepted, string _msg, Color _color)
        {
            //string message = _isAccepted ? "Success" : "Failed";
            //Color color = _isAccepted ? Color.green : Color.red;
            statusText.text = _msg;
            statusImage.color = _color;
            QRTranslator.CameraDetecting?.Invoke(false);
        }

        private void ResetStatuses()
        {
            statusText.text = "Stand by...";
            statusImage.color = Color.black;
            QRTranslator.CameraDetecting?.Invoke(true);
        }

        public void SetMessagesDisplay(bool isActive, string passengerName, string descriptionMsg, string amountMsg, Color color)
        {
            messageContatiner.SetActive(isActive);
            passengerNameMsgText.text = passengerName;
            descriptionMsgText.text = descriptionMsg;
            amountMsgText.text = amountMsg;
            
            passengerNameMsgText.color = color;
            descriptionMsgText.color = color;
            amountMsgText.color = color;
        }

        public void OnExitSelected()
        {
            ResetStatuses();
        }
    }
}