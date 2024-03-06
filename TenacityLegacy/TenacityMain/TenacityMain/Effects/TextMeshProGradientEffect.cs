using UnityEngine;
using TMPro;

namespace TenacityMain.Effects
{
    public class TextMeshProGradientEffect : MonoBehaviour
    {
        public static Color startColor = new Color32(245, 110, 213, 255);
        public static Color middleColor = new Color32(0, 160, 230, 255);
        public float animationSpeed = 0.75f;

        private TextMeshProUGUI textMeshPro;
        private TMP_TextInfo textInfo;
        private Material material;
        private float gradientOffset = 0f;

        private void Start()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            material = new Material(textMeshPro.material);
            textMeshPro.material = material;
            textInfo = textMeshPro.textInfo;
        }

        private void Update()
        {
            gradientOffset += animationSpeed * Time.deltaTime;

            UpdateGradient();
        }

        private void UpdateGradient()
        {
            if (textInfo != null)
            {
                float maxBoundsX = textInfo.meshInfo[0].mesh.bounds.size.x;

                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    float characterX = textInfo.characterInfo[i].bottomLeft.x;
                    float gradientPosition = characterX / maxBoundsX + gradientOffset;

                    // Ensure that the gradientPosition is within the [0, 1] range
                    gradientPosition = Mathf.Repeat(gradientPosition, 1f);

                    Color color;
                    if (gradientPosition < 0.5f)
                    {
                        color = Color.Lerp(startColor, middleColor, gradientPosition * 2);
                    }
                    else
                    {
                        color = Color.Lerp(middleColor, startColor, (gradientPosition - 0.5f) * 2);
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        textInfo.meshInfo[materialIndex].colors32[vertexIndex + j] = color;
                    }
                }

                textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
        }
    }
}
