using System.Collections;
using UnityEngine;


/// <summary>
/// Unity OSC monitors the fields in this class and sends changes
/// to Touchdesigner via OSC.
/// </summary>
public class OscMessage : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The current game state, 0 = Game Over, 1 = Playing")]
    public int gameState = 0;

    [SerializeField]
    [Tooltip("The AI prompt to be sent to to Touch Designer")]
    public string promptText;
}
