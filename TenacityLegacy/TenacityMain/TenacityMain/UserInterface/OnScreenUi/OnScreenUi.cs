using Photon.Pun;
using TenacityLib;
using TenacityMain.Effects;
using TenacityMain.ModSystem;
using TenacityMain.UserInterface.NotificationLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenacityMain.UserInterface.OnScreenUi
{
    internal class OnScreenUi : MonoBehaviour
    {
        public static Canvas Canvas;
        Texture2D Logo;
        GameObject LogoContainer, EnabledModsContainer, TextLogoContainer; 
        RectTransform logoTransform;
        TextMeshProUGUI text;
        TextMeshProUGUI textEnabledMods;
        TextMeshProUGUI versionText;
        public static TMP_FontAsset fontAsset;
        public static TextMeshProUGUI OnScreenBottomRight, OnScreenBottomLeft;

        public void Initialize()
        {
            LoadImages();

            MainCanvas();

            ImageLogo();
            TextLogo();

            EnabledMods();

            OnScreenBottomLeft = CreateText(TextAlignmentOptions.BottomLeft, InGameUi.InGameUi.font);
            OnScreenBottomRight = CreateText(TextAlignmentOptions.BottomRight, InGameUi.InGameUi.font);
        }

        void Update()
        {
            OnScreenBottomLeft.text = "FPS: " + Mathf.Round(1f / Time.smoothDeltaTime) + "\nSpeed: " + Mathf.Round(GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity.magnitude);

            OnScreenBottomRight.text = "Beta - Private  |  " + PhotonNetwork.LocalPlayer.NickName;
            
            if(!TenacityModSystem.TextLogo)
            {
                LogoContainer.SetActive(true);
                TextLogoContainer.SetActive(false);
            } 
            else
            {
                LogoContainer.SetActive(false);
                TextLogoContainer.SetActive(true);
            }

            text.color = TenacityModSystem.TextLogoColor;
            versionText.color = TenacityModSystem.TextLogoColor;

            textEnabledMods.font = fontAsset;
        }

        void EnabledMods()
        {
            EnabledModsContainer = new GameObject();
            EnabledModsContainer.name = "Enabled Mods Text Container";
            EnabledModsContainer.transform.SetParent(gameObject.transform, false);
            EnabledModsContainer.AddComponent<RectTransform>().sizeDelta = new Vector2(1920-50, 1080-50);

            GameObject enabledModsTextGameObject = new GameObject();
            enabledModsTextGameObject.transform.SetParent(EnabledModsContainer.transform, false);
            enabledModsTextGameObject.AddComponent<RectTransform>().sizeDelta = new Vector2(1920-50, 1080-50);
            enabledModsTextGameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            textEnabledMods = enabledModsTextGameObject.AddComponent<TextMeshProUGUI>();
            textEnabledMods.gameObject.name = "*VL8N6Alvr5cfE#kW$EPjBTGT!jWxStr";
            textEnabledMods.verticalAlignment = VerticalAlignmentOptions.Top;
            textEnabledMods.horizontalAlignment = HorizontalAlignmentOptions.Right;
            textEnabledMods.fontSize = 35;
            textEnabledMods.gameObject.AddComponent<TextMeshProGradientEffect>();
        }

        TextMeshProUGUI CreateText(TextAlignmentOptions textAlignment, TMP_FontAsset font)
        {
            GameObject container = new GameObject();
            container.transform.SetParent(gameObject.transform, false);
            container.AddComponent<RectTransform>().sizeDelta = new Vector2(1920-50, 1080-50);

            GameObject textObject = new GameObject();
            textObject.transform.SetParent(container.transform, false);
            textObject.AddComponent<RectTransform>().sizeDelta = new Vector2(1920-50, 1080-50);
            textObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            TextMeshProUGUI tmpText = textObject.AddComponent<TextMeshProUGUI>();
            tmpText.fontSize = 32;
            tmpText.alignment = textAlignment;
            tmpText.font = font;

            return tmpText;
        }

        void LoadImages()
        {
            Logo = Plugin.MainAssetBundle.LoadAsset<Texture2D>("Tenacity Logo.png");
        }

        void MainCanvas()
        {
            gameObject.name = "TenacityOnScreenUi";
            Canvas = gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<CanvasScaler>();
            gameObject.AddComponent<GraphicRaycaster>();
            gameObject.AddComponent<RectTransform>();
            Canvas.enabled = true;
            Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            gameObject.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            gameObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        }

        void TextLogo()
        {
            TextLogoContainer = new GameObject();
            TextLogoContainer.name = "Text Container";
            TextLogoContainer.transform.SetParent(gameObject.transform, false);
            TextLogoContainer.AddComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);

            TMP_FontAsset font = Plugin.MainAssetBundle.LoadAsset<TMP_FontAsset>("Antipasto.asset");

            GameObject textContainer = new GameObject();
            textContainer.name = "LogoText";
            text = textContainer.AddComponent<TextMeshProUGUI>();
            text.gameObject.transform.SetParent(TextLogoContainer.transform, false);
            text.gameObject.AddComponent<RectTransform>();
            text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
            text.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(20, -20, 0);
            text.fontSize = 72;
            text.font = font;
            text.text = "Tenacity";
            text.alignment = TextAlignmentOptions.TopLeft;

            GameObject versionTextContainer = new GameObject();
            versionTextContainer.name = "LogoTextVersion";
            versionText = versionTextContainer.AddComponent<TextMeshProUGUI>();
            versionText.gameObject.transform.SetParent(TextLogoContainer.transform, false);
            versionText.gameObject.AddComponent<RectTransform>();
            versionText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
            versionText.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(295, -17.5f, 0);
            versionText.text = "Beta";
            versionText.fontSize = 38;
            versionText.font = font;
            versionText.alignment = TextAlignmentOptions.TopLeft;
        }

        void ImageLogo()
        {
            LogoContainer = new GameObject();
            LogoContainer.transform.SetParent(gameObject.transform, false);
            logoTransform = LogoContainer.AddComponent<RectTransform>();
            LogoContainer.AddComponent<Image>();

            Rect ERect = new Rect();
            ERect.width = Logo.width;
            ERect.height = Logo.height;

            Sprite sprite = Sprite.Create(Logo, ERect, new Vector2(0, 0));

            LogoContainer.GetComponent<Image>().transform.localScale = new Vector2(2.5f, 2.5f);
            LogoContainer.GetComponent<Image>().sprite = sprite;
            logoTransform.position = new Vector2(100, 965);
        }
    }
}
