using UnityEngine;
using ZXing;
using ZXing.Common;
using System;

namespace TrainManager
{
    public class QRGenerator : MonoBehaviour
    {
        [SerializeField] Vector2Int qrDimension = new Vector2Int(512, 512);
        [SerializeField] Renderer qrRenderer = null;

        bool isDrawReady = false;
        string currentGUID = "";

        void OnDisable()
        {
            QRTranslator.OnLoadingBalance += HideQRViewer;
        }

        void OnEnable()
        {
            QRTranslator.OnLoadingBalance += HideQRViewer;
        }

        private void OnGUI()
        {
            if (isDrawReady)
            {
                if (!string.IsNullOrEmpty(currentGUID))
                {
                    Texture2D newTexture = GenerateBarcode(currentGUID, BarcodeFormat.QR_CODE, qrDimension.x, qrDimension.y);
                    qrRenderer.material.mainTexture = newTexture;
                }
            }
        }

        private void HideQRViewer(bool status)
        {
            qrRenderer.gameObject.SetActive(!status);
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
            //add station ID, time and date stamp, passenger 4pins
            Guid newId = Guid.NewGuid();
            Server.SaveToDabase(newId.ToString(), "sample_name", StationNames.AYALA);
            return newId.ToString();
        }

        public void OnGenerateSelected()
        {
            currentGUID = GenerateUniqueCode();
            isDrawReady = true;
            Debug.LogWarning(currentGUID);
        }
    }
}