using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPassengerController : MonoBehaviour
{
    public void OnQRScanSelected()
    {
        MobileQRController.CameraDetecting?.Invoke(true);
    }

    public void OnQRDisplaySelected()
    {
        MobileQRController.CameraDetecting?.Invoke(false);
    }
}
