using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingTextBounce : MonoBehaviour
{
    public TextMeshProUGUI loadingText; 
    public float bounceHeight = 20f;
    public float bounceDuration = 0.4f;
    public float delayBetweenChars = 0.1f;

    private TMP_TextInfo textInfo;

    void Start()
    {
        if (loadingText == null)
            loadingText = GetComponent<TextMeshProUGUI>();

        StartCoroutine(BounceLoop());
    }

    IEnumerator BounceLoop()
    {
        while (true)
        {
            loadingText.ForceMeshUpdate();
            textInfo = loadingText.textInfo;

            int charCount = textInfo.characterCount;
            if (charCount == 0) yield return null;

            // Bounce characters one by one
            for (int i = 0; i < charCount; i++)
            {
                if (textInfo.characterInfo[i].isVisible)
                    StartCoroutine(BounceChar(i));

                yield return new WaitForSeconds(delayBetweenChars);
            }
        }
    }

    IEnumerator BounceChar(int charIndex)
    {
        float time = 0;
        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
        int vertexIndex = charInfo.vertexIndex;
        int materialIndex = charInfo.materialReferenceIndex;

        Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
        Vector3[] originalVertices = (Vector3[])vertices.Clone();

        while (time < bounceDuration)
        {
            float progress = time / bounceDuration;
            float offsetY = Mathf.Sin(progress * Mathf.PI) * bounceHeight;

            for (int j = 0; j < 4; j++)
            {
                vertices[vertexIndex + j] = originalVertices[vertexIndex + j] + new Vector3(0, offsetY, 0);
            }

            loadingText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            time += Time.deltaTime;
            yield return null;
        }

        // Reset back to original position
        for (int j = 0; j < 4; j++)
        {
            vertices[vertexIndex + j] = originalVertices[vertexIndex + j];
        }
        loadingText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}