﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ZXing;
using ZXing.Common;

public class MobileQRController : MonoBehaviour
{
    public static Action<bool> CameraDetecting;
    public static Action<bool> QRDisplaying;

    [SerializeField] Vector2Int qrDimension = new Vector2Int(512, 512);
    [SerializeField] Renderer cameraRenderer = null;

    WebCamTexture camTexture;

    bool isDetecting = false;
    bool isDrawReady = false;
    string currentGUID = "";

    private void Awake()
    {
        MobileServer.InitializeDataPath();
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

        if (isDrawReady)
        {
            if (!string.IsNullOrEmpty(currentGUID))
            {
                Texture2D newTexture = GenerateBarcode(currentGUID, BarcodeFormat.QR_CODE, qrDimension.x, qrDimension.y);
                cameraRenderer.material.mainTexture = newTexture;
            }
        }
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

    private void PrepareCamera()
    {
        camTexture = new WebCamTexture(){
            requestedWidth = 1600,
            requestedHeight = 900,
            requestedFPS = 40f
        };
        SetIsCameraDetecting(isDetecting);
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
                Debug.Log($"Decoded from QR: {barcodeResult.Text}");
            }
        }
        catch (ZXing.FormatException e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    private void SetIsCameraDetecting(bool status)
    {
        isDetecting = status;
        cameraRenderer.gameObject.SetActive(isDetecting);

        if(isDetecting)
        {
            camTexture.Play();
        }
        else
        {
            camTexture.Stop();
        }
    }
}
