

using UnityEngine;
using System.Collections;
//using OpenCvSharp;
 // using OpenCvSharp.Demo;
using UnityEngine.UI;

public class WebCamTest : MonoBehaviour
{
    [SerializeField] private RawImage img = default;
    WebCamTexture webCam;
    

    private void Start()
    {
        webCam = new WebCamTexture();
        if (!webCam.isPlaying) webCam.Play();
        img.texture = webCam;
    }


    // Take a "screenshot" of a camera's Render Texture.






    /*protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {

        //this functions shows the live camera feed in grayscale using open cv

        Mat image = OpenCvSharp.Unity.TextureToMat(input);
        //Convert image to grayscale
        Mat imgGray = new Mat();
        Cv2.CvtColor(image, imgGray, ColorConversionCodes.BGR2GRAY);
        //image output
        if (output == null)
        {
            output = OpenCvSharp.Unity.MatToTexture(imgGray);
        }
        else
        {
             OpenCvSharp.Unity.MatToTexture(imgGray, output);
        }

       
        return true;

       
    } */




}
