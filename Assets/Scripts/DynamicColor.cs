using UnityEngine;

public class DynamicColor : MonoBehaviour
{
    public Color color;
    public TakePhotos photoManager;
    private bool colorAssigned = false;

    void Start()
    {
        if (photoManager == null)
        {
            photoManager = FindAnyObjectByType<TakePhotos>();
        }

        AssignColor();
    }

    void AssignColor()
    {
        if (photoManager != null && !colorAssigned)
        {
            color = photoManager.GetPinballColor();
            colorAssigned = true;
        }
    }
    
    public void ReassignColor()
    {
        colorAssigned = false;
        AssignColor();
    }
    
    public void SetColor(Color newColor)
    {
        color = newColor;
        colorAssigned = true;
    }
}
