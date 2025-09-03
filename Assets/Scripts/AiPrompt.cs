using System.Collections;
using UnityEngine;

/// <summary>
/// The Unity OSC package is designed to monitor specific fields
/// in a MonoBehaviour script, and send changes as OSC messages.
/// </summary>
public class AiPrompt : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The AI prompt to be sent to to Touch Designer")]
    public string promptText;
}
