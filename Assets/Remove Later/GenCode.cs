using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class GenCode : MonoBehaviour
{
    public GameObject inputfield;
    public string inputtext;

    public GameObject locationQR;
    Vector3 changPos;

    private float Codex, Codey;

    void Start()
    {
        changPos = locationQR.transform.position;
        Codex = changPos.x;
        Codey = changPos.y;
    }

    void OnGUI()//scanner
    {
        inputtext = inputfield.GetComponent<Text>().text;
        Texture2D myQR = generateQR(inputtext);
        if (inputtext != null)
        {
            if (GUI.Button(new Rect(Codex, Codey, 256, 256), myQR, GUIStyle.none)) { } //(x,y,height,width) of qrcode
        }
        //creates a new ui
    }

    //generates new qr code from text
    public Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }
    //takes data from generateQR and create the QR COde
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    /*public void generateCode()//button for stations
    {

    }*/
}
