using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class TakePhotos : MonoBehaviour
{

    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea; //this is the raw image the photo will be displayed on
    [SerializeField] private GameObject photoFrame; //this is a UI mask and image for the photo
    [SerializeField] private GameObject webCameraFeed; //this is the live feed


    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash; // point light for the flash
    [SerializeField] private float flashTime;
    public Texture2D screenCapture;
    private bool viewingPhoto; //this sets the photo to active 

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;
    [SerializeField] private Animator slidingAnimation;

    [Header("3D Plane Display")]
    [SerializeField] private GameObject displayPlane; // 3D plane to display the photo
    [SerializeField] private Material planeMaterial; // Material for the plane
    
    [Header("Color Channel Control")]
    [SerializeField] private bool enableChannelSwitching = true; // Enable color channel switching
    private enum ColorChannel { Full, Red, Green, Blue }
    private ColorChannel currentChannel = ColorChannel.Full;
    private Texture2D originalTexture; // Store the original full-color texture

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //input TBD by hardware 
        {
            if (!viewingPhoto)
            {
                StartCoroutine(CapturePhoto());
            }
            else
            {
                RemovePhoto();
            }
        }
        
        // Handle color channel switching
        if (enableChannelSwitching && displayPlane != null && displayPlane.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchToColorChannel(ColorChannel.Red);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchToColorChannel(ColorChannel.Green);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchToColorChannel(ColorChannel.Blue);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchToColorChannel(ColorChannel.Full);
            }
        }
    }

    public void TakePhoto()
    {
        StartCoroutine(CapturePhoto());
    }



    IEnumerator CapturePhoto()
    {
        viewingPhoto = true;
        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        ShowPhoto();
        webCameraFeed.SetActive(false);


        //return image and store photo file in folder;
        /*
                byte[] bytes = screenCapture.EncodeToPNG();
                string fileName = DateTime.Now.ToString("yyyymmdd_hhmmss") + ".png";
                string filePath = Application.dataPath + "Materials/Photo/" + fileName;
                UnityEngine.Windows.File.WriteAllBytes(filePath, bytes);

                //Destroy(rt);
                //Destroy(screenCapture);

                */

    }

    void ShowPhoto()
    {
        //shows photo on the image as a sprite in the photo frame GameObject
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;

        photoFrame.SetActive(true);
        StartCoroutine(CameraFlashEffect());
        fadingAnimation.Play("PhotoFade");
        slidingAnimation.Play("PhotoSlideUp");

        // Display photo on 3D plane
        DisplayPhotoOnPlane();
    }

    void DisplayPhotoOnPlane()
    {
        // Store the original texture
        originalTexture = screenCapture;
        
        // Create or update the material with custom shader
        if (planeMaterial == null)
        {
            Shader colorChannelShader = Shader.Find("Custom/ColorChannelShader");
            if (colorChannelShader != null)
            {
                planeMaterial = new Material(colorChannelShader);
            }
            else
            {
                // Fallback to Unlit/Texture if custom shader not found
                planeMaterial = new Material(Shader.Find("Unlit/Texture"));
            }
        }

        // Set the captured texture to the material
        planeMaterial.mainTexture = screenCapture;
        
        // Apply the material to the plane
        Renderer planeRenderer = displayPlane.GetComponent<Renderer>();
        planeRenderer.material = planeMaterial;

        displayPlane.SetActive(true);
        
        // Reset to full color channel
        currentChannel = ColorChannel.Full;
        SetChannelMask(ColorChannel.Full);
    }

    void SwitchToColorChannel(ColorChannel channel)
    {
        if (originalTexture == null || displayPlane == null || planeMaterial == null) return;
        
        currentChannel = channel;
        SetChannelMask(channel);
    }

    void SetChannelMask(ColorChannel channel)
    {
        Vector4 channelMask;
        
        switch (channel)
        {
            case ColorChannel.Red:
                channelMask = new Vector4(1, 0, 0, 1);
                break;
            case ColorChannel.Green:
                channelMask = new Vector4(0, 1, 0, 1);
                break;
            case ColorChannel.Blue:
                channelMask = new Vector4(0, 0, 1, 1);
                break;
            case ColorChannel.Full:
            default:
                channelMask = new Vector4(1, 1, 1, 1);
                break;
        }
        
        planeMaterial.SetVector("_ChannelMask", channelMask);
    }

    IEnumerator CameraFlashEffect()
    {
        //play audio
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);

    }   
    
    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);
    }

   
    


}
