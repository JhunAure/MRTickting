using UnityEngine;
using ZXing;
using ZXing.Common;

public class QRGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int qrDimension = new Vector2Int(512, 512);
    [SerializeField] Renderer qrRenderer;

    bool isDrawReady = false;
    string currentGUID = "";

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
        System.Guid newId = System.Guid.NewGuid();
        return newId.ToString();
    }

    public void OnGenerateSelected()
    {
        currentGUID = GenerateUniqueCode();
        isDrawReady = true;
        Debug.LogWarning(currentGUID);
    }
}

//public Texture2D GenerateQR(string text)
//{
//    var encoded = new Texture2D(dimension.x, dimension.y);
//    var color32 = Encode(text, encoded.width, encoded.height);
//    encoded.SetPixels32(color32);
//    encoded.Apply();
//    return encoded;
//}

//private static Color32[] Encode(string textForEncoding, int width, int height)
//{
//    var writer = new BarcodeWriter
//    {
//        Format = BarcodeFormat.QR_CODE,
//        Options = new QrCodeEncodingOptions
//        {
//            Height = height,
//            Width = width
//        }
//    };
//    return writer.Write(textForEncoding);
//}