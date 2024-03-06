using UnityEngine;

namespace TenacityMain.Effects
{
    internal class TenacityColorManager : MonoBehaviour
    {
        public static Color TenacityColor;
        public static Color StartColor = new Color32(245, 110, 213, 255);
        public static Color EndColor = new Color32(0, 160, 230, 255);

        private float LerpTime;
        private float LerpDuration = 1f;

        void Start()
        {
            TenacityColor = StartColor;
        }

        void Update()
        {
            LerpTime = Mathf.PingPong(Time.time / LerpDuration, 1.0f);
            TenacityColor = Color.Lerp(StartColor, EndColor, LerpTime);
        }
    }
}
