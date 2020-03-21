using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Passenger
{
    public class UIPassengerController : MonoBehaviour
    {
        [SerializeField] GameObject scanButton = null;
        [SerializeField] GameObject displayQRButton = null;

        private void SetButtons(bool isScanning)
        {
            scanButton.SetActive(!isScanning);
            displayQRButton.SetActive(isScanning);
        }

        public void OnQRScanSelected()
        {
            SetButtons(true);
            MobileQRController.CameraDetecting?.Invoke(true);
            MobileQRController.QRDisplaying?.Invoke(false);
        }

        public void OnQRDisplaySelected()
        {
            SetButtons(false);
            MobileQRController.CameraDetecting?.Invoke(false);
            MobileQRController.QRDisplaying?.Invoke(true);
        }
    }

}