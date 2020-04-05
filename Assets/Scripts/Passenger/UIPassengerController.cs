using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Passenger
{
    public class UIPassengerController : MonoBehaviour
    {
        [SerializeField] GameObject scanButton = null;
        [SerializeField] GameObject displayQRButton = null;

        void Start()
        {
            SetButtons(false);
        }

        private void SetButtons(bool status)
        {
            if (MobileServer.IsQRCodeExists())
            {
                status = true;
            }
            scanButton.SetActive(!status);
            displayQRButton.SetActive(status);
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

        public void OnCloseAppSelected()
        {
            Application.Quit();
        }
    }
}