using UnityEngine;

public class ChangeMat : MonoBehaviour
{
    private Renderer bumperRenderer;

    private void Start()
    {
        bumperRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Renderer pinballRenderer = collision.gameObject.GetComponent<Renderer>();
            if (pinballRenderer != null)
            {
                bumperRenderer.material = pinballRenderer.material;
                SwitchImage(pinballRenderer.material);
            }
        }
    }

    private void SwitchImage(Material material)
    {
        string materialName = material.name;
        string numberPart = materialName.Replace("Material", "").Replace(" (Instance)", "").Trim();

        if (int.TryParse(numberPart, out int themeIndex))
        {
            if (themeIndex >= 0 && themeIndex < transform.childCount)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(i == themeIndex);
                }
            }
        }
    }
}
