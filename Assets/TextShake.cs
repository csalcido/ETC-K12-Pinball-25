using UnityEngine;
using TMPro;

public class TextShake : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float shakeMagnitude = 2f; // how far it shakes
    public float shakeSpeed = 25f;    // how fast it shakes

    private Mesh mesh;
    private Vector3[] vertices;

    void Awake()
    {
        if (textMeshPro == null)
            textMeshPro = GetComponent<TMP_Text>();
    }

    void Update()
    {
        textMeshPro.ForceMeshUpdate();
        mesh = textMeshPro.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < textMeshPro.textInfo.characterCount; i++)
        {
            if (!textMeshPro.textInfo.characterInfo[i].isVisible)
                continue;

            int vertexIndex = textMeshPro.textInfo.characterInfo[i].vertexIndex;

            // Random offset to make each character wiggle
            Vector3 offset = new Vector3(
                Mathf.Sin(Time.time * shakeSpeed + i) * shakeMagnitude,
                Mathf.Cos(Time.time * shakeSpeed + i) * shakeMagnitude,
                0);

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        mesh.vertices = vertices;
        textMeshPro.canvasRenderer.SetMesh(mesh);
    }
}