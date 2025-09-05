using System.Collections;
using UnityEngine;


/// <summary>
/// Unity OSC monitors the fields in this class and sends changes
/// to Touchdesigner via OSC.
/// </summary>
public class OscMessage : MonoBehaviour
{
    [Tooltip("0 = Game In Progress, 1 = Game Over")]
    public int gameOver = 0;

    [Tooltip("The AI prompt to be sent to to Touch Designer")]
    public string promptText;
}
