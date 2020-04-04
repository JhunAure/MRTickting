using UnityEngine;
using System;
using ZXing;

namespace TrainManager
{
    public class QRTranslator : MonoBehaviour
    {
        public static Action<bool> CameraDetecting;
        public static Action<bool> OnLoadingBalance;

        [SerializeField] Renderer cameraRenderer = null;
        [SerializeField] StationMatrix stationMatrix = null;
        [SerializeField] StationTeller stationTeller = null;

        bool isLoadingBalance = false;
        bool isDetecting = true;
        WebCamTexture camTexture;

        private void Awake()
        {
            PrepareCamera();
        }

        private void OnEnable()
        {
            CameraDetecting += SetIsCameraDetecting;
            OnLoadingBalance += SetIsLoadingBalance;
        }

        private void OnDisable()
        {
            CameraDetecting -= SetIsCameraDetecting;
            OnLoadingBalance += SetIsLoadingBalance;
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
                    QRGenerator.OnResetBarcode?.Invoke();

                    if(!isLoadingBalance)
                    {
                        Server.ProcessTransaction(barcodeResult.Text, stationTeller.GetStationName(), stationMatrix);
                    }
                    else
                    {
                        if(stationTeller.GetLoadAmount() > 0)
                        {
                            Server.LoadBalance(barcodeResult.Text, stationTeller.GetLoadAmount());
                        }
                        else
                        {
                            Debug.LogError("Invalid input amount");
                        }
                    }
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

        private void SetIsLoadingBalance(bool status)
        {
            isLoadingBalance = status;
        }
    }

}