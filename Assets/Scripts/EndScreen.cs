using System.Collections;
using UnityEngine;

public class EndScreen : MonoBehaviour
{

    public Animator mainCameraAnimator;
    public GameStateManager gameStateManager;
    public GameObject originalPlane;
    public GameObject finalResultPlane;



    public void EndTheGame()
    {

        //make a new Texture2D to show as result and send to printer
        ShowFinalResult();
        StartCoroutine(PlayEndAnimation());

    }

    

    public IEnumerator PlayEndAnimation()
    {
        mainCameraAnimator.SetBool("playEndAnim", true);
        yield return new WaitForSeconds(5f);

        //change printed flag in gameStateManager
        gameStateManager.photoPrinted = true;   // TODO: Move this OscMessage / GameStateManger

    }

    void ShowFinalResult()
    {
        Texture2D finalTexture = CapturePlaneTexture(originalPlane);

        // Get the renderer of the end screen plane
        Renderer renderer = finalResultPlane.GetComponent<Renderer>();
    
       // Assign the captured texture
        renderer.material.mainTexture = finalTexture;
    }

    public Texture2D CapturePlaneTexture(GameObject plane)
    {
        // Check if the plane reference is null; if so, return null to avoid errors
        if (plane == null) return null;

        // Get the Renderer and main texture assigned to the plane’s material
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        Texture sourceTexture = planeRenderer.material.mainTexture;

        if (sourceTexture is RenderTexture renderTex)
        {
            RenderTexture.active = renderTex;

            // Create a new Texture2D with the same width and height as the RenderTexture
            Texture2D tex = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            tex.Apply();

            // Reset the active RenderTexture to null (cleanup)
            RenderTexture.active = null;
            return tex;
        }
        else if (sourceTexture is Texture2D tex2D)
        {
            // Create a copy with the same dimensions and format
            Texture2D copy = new Texture2D(tex2D.width, tex2D.height, tex2D.format, false);

            // Copy all pixel data from the original texture
            copy.SetPixels(tex2D.GetPixels());
            copy.Apply();
            return copy;
        }

        Debug.LogWarning("Plane material has unsupported texture type.");
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
