using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class StationGate : MonoBehaviour
{
    public static Action<bool> QRProcessed;

    [SerializeField] StationNames currentStationName;
    [SerializeField] TextMeshProUGUI statusText = null;
    [SerializeField] Image statusImage = null;

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

    private void SetStatuses(bool _isAccepted)
    {
        string message = _isAccepted ? "Success" : "Failed";
        Color color = _isAccepted ? Color.green : Color.red;
        statusText.text = message;
        statusImage.color = color;
        QRTranslator.CameraDetecting?.Invoke(false);
    }

    private void ResetStatuses()
    {
        statusText.text = "Stand by...";
        statusImage.color = Color.black;
        QRTranslator.CameraDetecting?.Invoke(true);
    }

    public void OnExitSelected()
    {
        ResetStatuses();
    }
}