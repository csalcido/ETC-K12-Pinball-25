using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Klak.Ndi;
using TMPro;


public class TakePhotos : MonoBehaviour
{

    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea; //this is the raw image the photo will be displayed on
    [SerializeField] private GameObject photoFrame; //this is a UI mask and image for the photo
    [SerializeField] private GameObject webCameraFeed; //this is the live feed


    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash; // point light for the flash
    [SerializeField] private float flashTime;
    [SerializeField] public SoundController flashSound;
    public Texture2D screenCapture;
    private bool viewingPhoto; //this sets the photo to active 

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;
    [SerializeField] private Animator slidingAnimation;
    public Animator cameraAnimator;

    public GameObject gumballManager; //this while be set to active and start the gumball sequence after the photo is taken
    

    [Header("3D Plane Display")]
    [SerializeField] private GameObject displayPlane; // 3D plane to display the photo
    [SerializeField] private Material planeMaterial; // material for the plane (this is set automatically)
    
    [Header("Pinball Tracking")]
    [SerializeField] private bool enablePinballTracking = true;
    [SerializeField] private float trackingRadius = 10f; // radius around pinball to mark as visited (in pixels)
    [SerializeField] private Material trackingMaterial; // material with tracking fragment shader

    [Header("NDI")]
    [SerializeField] private NdiSender ndiSender; // NDI sender component for tracking data
    [SerializeField] private NdiSender ndiSenderPhoto; // NDI sender component for full color photo
    [SerializeField] private NdiReceiver ndiReceiver; // NDI receiver component
    [SerializeField] private bool showRawTrackingTexture = false; // show tracking texture instead of NDI input

    private RenderTexture trackingRenderTexture; // texture with tracking data
    private RenderTexture tempRenderTexture; // temp texture for ping-pong rendering
    private GameObject[] pinballs; // array to store all pinball references

    // GPU tracking variables
    // max is 32 pinballs; this is hardcoded because the shader also expects
    // a fixed size array.
    private Vector4[] pinballPositions = new Vector4[32];
    private Vector4[] previousPinballPositions = new Vector4[32];
    private Color[] pinballColors = new Color[32]; // individual colors for each pinball
    
    // Tracking color variables
    public enum TrackingColor { Red, Green, Blue }
    private TrackingColor currentTrackingColor = TrackingColor.Red;
    private Color redColor = new Color(1f, 0f, 0f, 1f);
    private Color greenColor = new Color(0f, 1f, 0f, 1f);
    private Color blueColor = new Color(0f, 0f, 1f, 1f);
    
    public Color nextPinballColor;
    
    [Header("Countdown Display")]//text display for picture countdown
    [SerializeField] public int countdownTime;
    [SerializeField] public TextMeshProUGUI countdownDisplay;
    [SerializeField] public GameObject countdownDisplayBackground;
    [SerializeField] public SoundController countdownSound;

    

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        
        if (enablePinballTracking)
        {   
            if (trackingMaterial != null)
            {
                InitializeGPUTracking();
            }
            else
            {
                Debug.LogWarning("Pinball Tracker material not assigned");
            }
        }
    }
    
    void InitializeGPUTracking()
    {
        // Create render textures
        trackingRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        trackingRenderTexture.Create();
        
        tempRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        tempRenderTexture.Create();
        
        // Clear both textures
        RenderTexture.active = trackingRenderTexture;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = tempRenderTexture;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = null;
        
        // Set texture size in the shader
        trackingMaterial.SetVector("_TextureSize", new Vector2(Screen.width, Screen.height));
        trackingMaterial.SetFloat("_TrackingRadius", trackingRadius);

        // Initialize tracking color to red
        UpdateTrackingColor();

        // Init pinball positions and colors
        for (int i = 0; i < 32; i++)
        {
            previousPinballPositions[i] = Vector4.zero;
            pinballColors[i] = Color.clear;
        }
        
        // Initialize NDI sender - always send tracking texture
        if (ndiSender != null)
        {
            ndiSender.captureMethod = CaptureMethod.Texture;
            ndiSender.sourceTexture = trackingRenderTexture;
        }
        
        // Initialize NDI sender for photo
        if (ndiSenderPhoto != null)
        {
            ndiSenderPhoto.captureMethod = CaptureMethod.Texture;
        }
        
        // NDI receiver is configured in its own component - no setup needed here
    }

     public void ClickButton()
    {
        if (!viewingPhoto)
            {
                countdownDisplayBackground.SetActive(true);
                StartCoroutine(CountdownToPhoto());

            }
            else
            {
                RemovePhoto();
                cameraAnimator.SetBool("playGumballAnim", true);
                gumballManager.SetActive(true);

            }
    }

    private void Update()
    {
        

        if (enablePinballTracking && trackingMaterial != null && trackingRenderTexture != null)
        {
            UpdatePinballTracking();
        }

        // Debug controls
        if (displayPlane != null && displayPlane.activeInHierarchy)
        {
            // Press T to toggle between photo and tracking mask
            if (Input.GetKeyDown(KeyCode.T))
            {
                ToggleDisplayMode();
            }
        }

        // Press R to toggle raw tracking texture display
        // skip ndi receiving (debug)
        if (Input.GetKeyDown(KeyCode.R))
        {
            showRawTrackingTexture = !showRawTrackingTexture;
        }

        // Press C to clear/reset tracking texture
        if (Input.GetKeyDown(KeyCode.C))
        {
            ResetTrackingTexture();
        }
    }

    public void TakePhoto()
    {
        StartCoroutine(CapturePhoto());
    }



    IEnumerator CapturePhoto()
    {
        flashSound.PlaySound();
        viewingPhoto = true;
        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        
        // Send captured photo immediately via NDI
        if (ndiSenderPhoto != null)
        {
            ndiSenderPhoto.sourceTexture = screenCapture;
        }
        
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

    IEnumerator CountdownToPhoto()
    {
        while (countdownTime > 0)
        {
            countdownSound.PlaySound();
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;

        }
        StartCoroutine(CapturePhoto());
        countdownDisplayBackground.SetActive(false);
        //countdownDisplay.text = "Snap!";



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
        // Create or update the material with basic shader
        if (planeMaterial == null)
        {
            planeMaterial = new Material(Shader.Find("Unlit/Texture"));
        }

        // Choose texture source based on NDI input settings
        Texture textureToDisplay = GetDisplayTexture();
        
        // Set the texture to the material
        planeMaterial.mainTexture = textureToDisplay;
        
        // Apply the material to the plane
        Renderer planeRenderer = displayPlane.GetComponent<Renderer>();
        planeRenderer.material = planeMaterial;

        displayPlane.SetActive(true);
    }

    void UpdatePinballTracking()
    {
        // Find all pinballs
        pinballs = GameObject.FindGameObjectsWithTag("Ball");
        Array.Copy(pinballPositions, previousPinballPositions, 32);
        for (int i = 0; i < 32; i++)
        {
            pinballPositions[i] = Vector4.zero;
            pinballColors[i] = Color.clear;
        }
        
        // Convert to plane-relative coords
        int activePinballs = 0;
        for (int i = 0; i < pinballs.Length && i < 32; i++)
        {
            if (pinballs[i] != null && pinballs[i].activeInHierarchy && displayPlane != null)
            {
                // Project the ball's position onto the plane
                Vector3 ballWorldPos = pinballs[i].transform.position;
                Vector3 planeLocalPos = displayPlane.transform.InverseTransformPoint(ballWorldPos);
                
                // Convert local plane coords to texture coords
                float planeSize = 10f; // default for now TODO: make this dynamic based on plane size
                float textureX = (1.0f - (planeLocalPos.x / planeSize + 0.5f)) * trackingRenderTexture.width;
                float textureY = (1.0f - (planeLocalPos.z / planeSize + 0.5f)) * trackingRenderTexture.height;
                
                // Bounds check
                if (textureX >= 0 && textureX < trackingRenderTexture.width && 
                    textureY >= 0 && textureY < trackingRenderTexture.height)
                {
                    // Use ball's InstanceID as a unique id for shader
                    // so that balls are not overwritten
                    int ballID = pinballs[i].GetInstanceID();
                    
                    // Get the pinball's individual color from DynamicColor component
                    DynamicColor dynamicColor = pinballs[i].GetComponent<DynamicColor>();
                    Color ballColor = (dynamicColor != null) ? dynamicColor.color : GetPinballColor();
                    
                    pinballPositions[activePinballs] = new Vector4(
                        textureX, 
                        textureY,
                        1.0f, // Active flag necessary for shader to know this ball is a real pinball
                        ballID 
                    );
                    pinballColors[activePinballs] = ballColor;
                    activePinballs++;
                }
            }
        }
        
        // Send data to fragment shader
        trackingMaterial.SetVectorArray("_PinballPositions", pinballPositions);
        trackingMaterial.SetVectorArray("_PreviousPositions", previousPinballPositions);
        trackingMaterial.SetColorArray("_PinballColors", pinballColors);
        trackingMaterial.SetInt("_PinballCount", activePinballs);
        trackingMaterial.SetFloat("_TrackingRadius", trackingRadius);
        trackingMaterial.SetVector("_TextureSize", new Vector2(trackingRenderTexture.width, trackingRenderTexture.height));
        
        // Ping-pong rendering
        Graphics.Blit(trackingRenderTexture, tempRenderTexture, trackingMaterial);
        
        // Swap the textures
        RenderTexture temp = trackingRenderTexture;
        trackingRenderTexture = tempRenderTexture;
        tempRenderTexture = temp;
        
        // Update NDI sender texture (always send tracking data)
        if (ndiSender != null)
        {
            ndiSender.sourceTexture = trackingRenderTexture;
        }
    }
    
    void UpdateTrackingColor()
    {
        if (trackingMaterial == null) return;
        
        Color colorToSet;
        string colorName;
        switch (currentTrackingColor)
        {
            case TrackingColor.Red:
                colorToSet = redColor;
                colorName = "Red";
                break;
            case TrackingColor.Green:
                colorToSet = greenColor;
                colorName = "Green";
                break;
            case TrackingColor.Blue:
                colorToSet = blueColor;
                colorName = "Blue";
                break;
            default:
                colorToSet = redColor;
                colorName = "Red";
                break;
        }
        
        trackingMaterial.SetColor("_TrackingColor", colorToSet);
        Debug.Log($"Tracking color changed to: {colorName}");
    }
    
    Texture GetDisplayTexture()
    {
        // If debug mode is enabled, show raw tracking texture
        if (showRawTrackingTexture)
        {
            return trackingRenderTexture;
        }
        
        // If we have NDI input, use it; otherwise fall back to screen capture
        if (ndiReceiver != null && ndiReceiver.texture != null)
        {
            return ndiReceiver.texture;
        }
        
        // Default to screen capture
        return screenCapture;
    }
    
    public void ResetTrackingTexture()
    {
        if (trackingRenderTexture != null)
        {
            // Clear the tracking texture to black
            RenderTexture.active = trackingRenderTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;
            
            // Also clear the temp texture
            if (tempRenderTexture != null)
            {
                RenderTexture.active = tempRenderTexture;
                GL.Clear(true, true, Color.black);
                RenderTexture.active = null;
            }
            
            // Reset pinball position and color history
            for (int i = 0; i < 32; i++)
            {
                pinballPositions[i] = Vector4.zero;
                previousPinballPositions[i] = Vector4.zero;
                pinballColors[i] = Color.clear;
            }
        }
    }
    
    public Texture GetTrackingMask()
    {
        return trackingRenderTexture;
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

    public void DisplayTrackingMaskOnPlane()
    {
        if (displayPlane == null) return;
        
        // Always show the tracking mask
        Texture maskTexture = GetTrackingMask();
        if (maskTexture == null) return;
        
        if (planeMaterial == null)
        {
            planeMaterial = new Material(Shader.Find("Unlit/Texture"));
        }

        // display mask
        planeMaterial.mainTexture = maskTexture;
        Renderer planeRenderer = displayPlane.GetComponent<Renderer>();
        planeRenderer.material = planeMaterial;
        displayPlane.SetActive(true);
    }
    
    public void ToggleDisplayMode()
    {
        if (displayPlane == null || !displayPlane.activeInHierarchy) return;
        
        // Get current textures for comparison
        Texture currentTrackingMask = GetTrackingMask();
        Texture currentDisplayTexture = GetDisplayTexture();
        
        // Toggle between showing the photo/NDI input and tracking mask
        if (planeMaterial.mainTexture == currentDisplayTexture)
        {
            DisplayTrackingMaskOnPlane();
        }
        else
        {
            DisplayPhotoOnPlane();
        }
    }
    
    public Color GetPinballColor()
    {
        Color result;
        if (trackingRenderTexture == null || IsTrackingTextureEmpty())
        {
            result = RandomColor();
        }
        else
        {
            result = GetLeastUsedColor();
        }
        
        nextPinballColor = result;
        return result;
    }
    
    private bool IsTrackingTextureEmpty()
    {
        RenderTexture.active = trackingRenderTexture;
        Texture2D temp = new Texture2D(trackingRenderTexture.width, trackingRenderTexture.height, TextureFormat.RGB24, false);
        temp.ReadPixels(new Rect(0, 0, trackingRenderTexture.width, trackingRenderTexture.height), 0, 0);
        temp.Apply();
        RenderTexture.active = null;
        
        Color[] pixels = temp.GetPixels();
        int nonBlackPixels = 0;
        float threshold = 0.1f; // when to consider a pixel non-black
        
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r > threshold || pixels[i].g > threshold || pixels[i].b > threshold)
            {
                nonBlackPixels++;
            }
        }
        
        DestroyImmediate(temp);
        
        float nonBlackPercentage = (float)nonBlackPixels / pixels.Length;
        return nonBlackPercentage < 0.01f;
    }
    
    private Color GetLeastUsedColor()
    {
        RenderTexture.active = trackingRenderTexture;
        Texture2D temp = new Texture2D(trackingRenderTexture.width, trackingRenderTexture.height, TextureFormat.RGB24, false);
        temp.ReadPixels(new Rect(0, 0, trackingRenderTexture.width, trackingRenderTexture.height), 0, 0);
        temp.Apply();
        RenderTexture.active = null;
        
        Color[] pixels = temp.GetPixels();
        
        // Count usage of each tracking color
        int redCount = 0, greenCount = 0, blueCount = 0;
        float colorThreshold = 0.5f;
        
        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];
            
            if (pixel.r > colorThreshold && pixel.r > pixel.g && pixel.r > pixel.b)
            {
                redCount++;
            }
            else if (pixel.g > colorThreshold && pixel.g > pixel.r && pixel.g > pixel.b)
            {
                greenCount++;
            }
            else if (pixel.b > colorThreshold && pixel.b > pixel.r && pixel.b > pixel.g)
            {
                blueCount++;
            }
        }
        
        DestroyImmediate(temp);
        
        // Return the least used color
        if (redCount <= greenCount && redCount <= blueCount)
        {
            return redColor;
        }
        else if (greenCount <= blueCount)
        {
            return greenColor;
        }
        else
        {
            return blueColor;
        }
    }

    private Color RandomColor()
    {
        // pick a random red, green, or blue color
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0: return redColor;
            case 1: return greenColor;
            case 2: return blueColor;
            default: return redColor; // fallback
        }
    }
    
    private void OnDestroy()
    {
        // Clean up NDI sender
        if (ndiSender != null)
        {
            ndiSender.sourceTexture = null;
        }
        
        if (ndiSenderPhoto != null)
        {
            ndiSenderPhoto.sourceTexture = null;
        }
        
        // Clean up GPU textures
        if (trackingRenderTexture != null)
        {
            trackingRenderTexture.Release();
            trackingRenderTexture = null;
        }
        
        if (tempRenderTexture != null)
        {
            tempRenderTexture.Release();
            tempRenderTexture = null;
        }
    }
}
