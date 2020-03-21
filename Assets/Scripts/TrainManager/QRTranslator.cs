using UnityEngine;
using System;
using ZXing;

namespace TrainManager
{
    public class QRTranslator : MonoBehaviour
    {
        public static Action<bool> CameraDetecting;

        [SerializeField] Renderer cameraRenderer = null;
        [SerializeField] StationNames stationName;
        [SerializeField] StationMatrix stationMatrix = null;

        bool isDetecting = true;
        WebCamTexture camTexture;

        private void Awake()
        {
            PrepareCamera();
        }

        private void OnEnable()
        {
            CameraDetecting += SetIsCameraDetecting;
        }

        private void OnDisable()
        {
            CameraDetecting -= SetIsCameraDetecting;
        }

        private void OnGUI()
        {
            if (isDetecting)
            {
                DrawCamera();
            }
        }

        private void DrawCamera()
        {
            cameraRenderer.material.mainTexture = camTexture;

            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                var barcodeResult = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);

                if (barcodeResult != null)
                {
                    Server.ProcessTransaction(barcodeResult.Text, stationName, stationMatrix);
                    Debug.Log($"Decoded from QR: {barcodeResult.Text}");
                }
            }
            catch (ZXing.FormatException e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        private void PrepareCamera()
        {
            camTexture = new WebCamTexture()
            {
                requestedWidth = 1600,
                requestedHeight = 900,
                requestedFPS = 40f
            };
            camTexture?.Play();
        }

        private void SetIsCameraDetecting(bool status)
        {
            isDetecting = status;
        }
    }

}