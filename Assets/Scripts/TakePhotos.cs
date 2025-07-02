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
    private Texture2D screenCapture;
    private bool viewingPhoto; //this sets the photo to active 

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;
    [SerializeField] private Animator slidingAnimation;

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
