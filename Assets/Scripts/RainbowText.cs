using TMPro;
using UnityEngine;

public class RainbowText : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    [SerializeField] private float speed = 1f;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        textMeshPro.ForceMeshUpdate();
        var textInfo = textMeshPro.textInfo;

        for (int i = 0;i <textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;
            var meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];

            float hue = (Time.time * speed + i * 0.1f) % 1f;
            Color32 rainbowColor = Color.HSVToRGB(hue, 0.8f, 1f);

            for (int j = 0; j < 4; j++)
            {
                int vertexIndex = charInfo.vertexIndex + j;
                meshInfo.colors32[vertexIndex] = rainbowColor;
            }
        }
        textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
