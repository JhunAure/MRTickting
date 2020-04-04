using UnityEngine;
using ZXing;
using ZXing.Common;
using System;

namespace TrainManager
{
    public class QRGenerator : MonoBehaviour
    {
        public static Action<bool> OnSetNewPassenger;
        public static Action OnResetBarcode;

        [SerializeField] Vector2Int qrDimension = new Vector2Int(512, 512);
        [SerializeField] Renderer qrRenderer = null;
        [SerializeField] StationTeller stationTeller = null;

        bool isDrawReady = false;
        string currentGUID = "";

        void OnEnable()
        {
            QRTranslator.OnLoadingBalance += HideQRViewer;
            QRGenerator.OnSetNewPassenger += HideQRViewer;
            OnResetBarcode += ResetBarcodeDisplay;
        }

        void OnDisable()
        {
            QRTranslator.OnLoadingBalance -= HideQRViewer;
            QRGenerator.OnSetNewPassenger -= HideQRViewer;
            OnResetBarcode -= ResetBarcodeDisplay;
        }

        private void OnGUI()
        {
            if (isDrawReady)
            {
                if (!string.IsNullOrEmpty(currentGUID))
                {
                    Texture2D newTexture = GenerateBarcode(currentGUID, BarcodeFormat.QR_CODE, qrDimension.x, qrDimension.y);
                    qrRenderer.material.mainTexture = newTexture;
                    isDrawReady = false;
                }
            }
        }

        private void HideQRViewer(bool status)
        {
            qrRenderer.gameObject.SetActive(!status);
            stationTeller.SetInterfaceButtons(!status);
        }

        private Texture2D GenerateBarcode(string data, BarcodeFormat format, int width, int height)
        {
            BitMatrix bitMatrix = new MultiFormatWriter().encode(data, format, width, height);// Generate the BitMatrix
            Color[] pixels = new Color[bitMatrix.Width * bitMatrix.Height];// Generate the pixel array
            int pos = 0;

            for (int y = 0; y < bitMatrix.Height; y++)
            {
                for (int x = 0; x < bitMatrix.Width; x++)
                {
                    pixels[pos++] = bitMatrix[x, y] ? Color.black : Color.white;
                }
            }
            Texture2D newTexture = new Texture2D(bitMatrix.Width, bitMatrix.Height);// Setup the texture
            newTexture.SetPixels(pixels);
            newTexture.Apply();
            return newTexture;
        }

        private string GenerateUniqueCode()
        {
            string passengerName = stationTeller.GetPassengerName();
            string pin = stationTeller.GetPin();
            string date = System.DateTime.Today.ToShortDateString();
            int stationId = (int)stationTeller.GetStationName();
            string newGUID = ProcessGUID(passengerName, stationId, date);
            Server.SaveToDabase(newGUID, passengerName, StationNames.AYALA, date, pin);
            isDrawReady = true;
            return newGUID;
        }

        private string ProcessGUID(string _passengerName, int _stationId, string _date)
        {
            string newId = Guid.NewGuid().ToString();
            string[] newIds = newId.Split('-');
            _passengerName = string.IsNullOrEmpty(_passengerName)? "Unknown": _passengerName;
            _passengerName = _passengerName.Replace(' ', '_').ToLower();
            _date = _date.Replace("/", string.Empty);
            newId = string.Concat(_passengerName, "-", _stationId,'-', newIds[0], '-', _date);
            return newId;
        }

        private void ResetBarcodeDisplay()
        {
            qrRenderer.material.mainTexture = null;
            currentGUID = "";
            Debug.Log("Reset Barcode");
        }

        #region Unity Button Click Events
        public void OnGenerateSelected()
        {
            if(!stationTeller.isInputsValid()) return;
            currentGUID = GenerateUniqueCode();
            stationTeller.SetInputFields(false);
            stationTeller.SetInterfaceButtons(true);
            HideQRViewer(false);
            Debug.Log($"Generated GUID: {currentGUID}");
        }
        #endregion
    }
}