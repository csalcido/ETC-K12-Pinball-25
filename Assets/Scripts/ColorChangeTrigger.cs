using UnityEngine;

public class ColorChangeTrigger : MonoBehaviour
{
    public bool useRandomColor = true;
    public enum enumPresetColor { Red, Green, Blue }
    public enumPresetColor presetColorChoice = enumPresetColor.Red;
    
    void Start()
    {
        // Make sure this object has a trigger collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            DynamicColor dynamicColor = other.GetComponent<DynamicColor>();
            if (dynamicColor != null)
            {
                Color newColor;
                if (useRandomColor)
                {
                    newColor = GetRandomColor();
                }
                else
                {
                    newColor = GetPresetColor();
                }
                dynamicColor.SetColor(newColor);
            }
        }
    }
    
    Color GetRandomColor()
    {
        int randomChoice = Random.Range(0, 3);
        if (randomChoice == 0)
        {
            return Color.red;
        }
        else if (randomChoice == 1)
        {
            return Color.green;
        }
        else
        {
            return Color.blue;
        }
    }

    Color GetPresetColor()
    {
        if (presetColorChoice == enumPresetColor.Red)
        {
            return Color.red;
        }
        else if (presetColorChoice == enumPresetColor.Green)
        {
            return Color.green;
        }
        else if (presetColorChoice == enumPresetColor.Blue)
        {
            return Color.blue;
        }
        else
        {
            Debug.LogWarning("Invalid preset color choice, defaulting to Red.");
            return Color.red;
        }
    }
}
