using UnityEngine;
using System;
using ZXing;

public class QRTranslator : MonoBehaviour
{
    public static Action<bool> CameraDetecting;

    [SerializeField] Renderer cameraRenderer;
    //[SerializeField] RectTransform cameraViewer;
    //[SerializeField] CameraDimension cameraDimension;

    bool isDetecting = true;

    //[System.Serializable] struct CameraDimension
    //{
    //    public float posX;
    //    public float posY;
    //    public float width;
    //    public float height;
    //}

    private Rect cameraViewRect;
    private WebCamTexture camTexture;

    private void Awake()
    {
        StartCamera();
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
        if(isDetecting)
        {
            DrawCamera();
        }
    }

    private void DrawCamera()
    {
        //var newRect = new Rect(cameraDimension.posX, cameraDimension.posY, cameraDimension.width, cameraDimension.height);
        //GUI.DrawTexture(newRect, camTexture, ScaleMode.ScaleToFit);
        cameraRenderer.material.mainTexture = camTexture;

        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            var barcodeResult = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);

            if (barcodeResult != null)
            {
                StationGate.QRProcessed?.Invoke(true);
                DisplayDecodeQR(barcodeResult.Text);
            }
        }
        catch(ZXing.FormatException e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    private void DisplayDecodeQR(string decodedText)
    {
        Debug.Log($"Decoded from QR: {decodedText}");
    }

    private void StartCamera()
    {
        //SetupCameraViewer();
        PrepareCamera();
    }

    //private void SetupCameraViewer()
    //{
    //    Rect viewerRect = cameraViewer.rect;
    //    float posX = viewerRect.x;
    //    float posY = viewerRect.y;
    //    float width = viewerRect.width;
    //    float height = viewerRect.height;
    //    cameraViewRect = new Rect(-(posX), -(posY), width, height);
    //}

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
