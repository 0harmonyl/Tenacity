using GorillaNetworking;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TenacityLib;
using TenacityMain.Effects;
using TenacityMain.ModSystem;
using TenacityMain.TenacityMods.Misc;
using TenacityMain.TenacityMods.Settings;
using TenacityMain.UserInterface.InGameUi.UiComponents;
using TenacityMain.UserInterface.NotificationLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;
using static System.Net.Mime.MediaTypeNames;
using Text = UnityEngine.UI.Text;

namespace TenacityMain.UserInterface.InGameUi
{
    internal class InGameUi : MonoBehaviour
    {
        public GameObject canvasPrefab, positionTracker, rightHandCube;
        public static TMP_FontAsset font;
        bool toggled, cooldown, uiinteractcooldown;

        public GameObject InGameCanvas;
        public static TextMeshProUGUI BottomRightText, BottomLeftText, CenterTopText, CenterText, TopRightText, TopLeftText;

        public void Initialize()
        {
            font = Plugin.MainAssetBundle.LoadAsset<TMP_FontAsset>("Antipasto.asset");

            // Tenacity Ui
            canvasPrefab = Instantiate(Plugin.MainAssetBundle.LoadAsset<GameObject>("TenacityInGameUi.prefab"), GameObject.Find("TurnParent").transform);
            positionTracker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            positionTracker.GetComponent<Collider>().enabled = false;
            positionTracker.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            positionTracker.transform.SetParent(GameObject.Find("Main Camera").transform, false);
            positionTracker.transform.localPosition = new Vector3(0f, 0f, 1f);
            positionTracker.GetComponent<MeshRenderer>().enabled = false;
            canvasPrefab.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            canvasPrefab.transform.position = new Vector3(GameObject.Find("Main Camera").transform.position.x, GameObject.Find("Main Camera").transform.position.y, GameObject.Find("Main Camera").transform.position.z + 1f);
            canvasPrefab.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
            canvasPrefab.transform.rotation = GameObject.Find("Main Camera").transform.rotation;
            canvasPrefab.AddComponent<TabSwitcher>();
            canvasPrefab.AddComponent<TrackedDeviceGraphicRaycaster>();
            canvasPrefab.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

            TenacityUiButton BlatantButton =
                    GameObject.Find("TabButtonContainer/Blatant").AddComponent<TenacityUiButton>();
            BlatantButton.buttonPress = canvasPrefab.GetComponent<TabSwitcher>();
            TenacityUiButton MiscButton =
                GameObject.Find("TabButtonContainer/Misc").AddComponent<TenacityUiButton>();
            MiscButton.buttonPress = canvasPrefab.GetComponent<TabSwitcher>();
            TenacityUiButton TrollButton =
                GameObject.Find("TabButtonContainer/Troll").AddComponent<TenacityUiButton>();
            TrollButton.buttonPress = canvasPrefab.GetComponent<TabSwitcher>();
            TenacityUiButton SafetyButton =
                GameObject.Find("TabButtonContainer/Safety").AddComponent<TenacityUiButton>();
            SafetyButton.buttonPress = canvasPrefab.GetComponent<TabSwitcher>();
            TenacityUiButton VisualsButton =
                GameObject.Find("TabButtonContainer/Visuals").AddComponent<TenacityUiButton>();
            VisualsButton.buttonPress = canvasPrefab.GetComponent<TabSwitcher>();
            TenacityUiButton SettingsButton =
                GameObject.Find("TabButtonContainer/Settings").AddComponent<TenacityUiButton>();
            SettingsButton.buttonPress = canvasPrefab.GetComponent<TabSwitcher>();
            TenacityUiButton ConfigsButton =
                GameObject.Find("TabButtonContainer/Configs").AddComponent<TenacityUiButton>();
            ConfigsButton.buttonPress = canvasPrefab.GetComponent<TabSwitcher>();
            TenacityUiButton CustomButton =
                GameObject.Find("TabButtonContainer/Custom").AddComponent<TenacityUiButton>();
            CustomButton.buttonPress = canvasPrefab.GetComponent<TabSwitcher>();

            rightHandCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightHandCube.name = "TenacityUiCursor";
            rightHandCube.layer = LayerMask.NameToLayer("Water");
            rightHandCube.AddComponent<MeshRenderer>();
            rightHandCube.AddComponent<MeshFilter>();
            rightHandCube.GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            rightHandCube.transform.localPosition = new Vector3(0, 0, 0.3f);
            rightHandCube.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            rightHandCube.GetComponent<BoxCollider>().size = new Vector3(0.25f, 0.25f, 0.25f);
            rightHandCube.GetComponent<BoxCollider>().isTrigger = true;

            GameObject.Find("RightHand Controller").AddComponent<UiInteractor>();

            // Setup In Game Ui
            InGameCanvas = new GameObject();
            InGameCanvas.name = "Tenacity In Game Canvas";
            Canvas canvas = InGameCanvas.AddComponent<Canvas>();
            InGameCanvas.AddComponent<CanvasScaler>();
            InGameCanvas.AddComponent<GraphicRaycaster>();
            canvas.enabled = true;
            canvas.planeDistance = 0.5f;
            try
            {
                canvas.gameObject.AddComponent<RectTransform>();
            } catch
            {

            }
            canvas.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(750, 750);
            InGameCanvas.GetComponent<RectTransform>().localScale = new Vector3(0.0004f, 0.0004f, 0.0004f);
            InGameCanvas.transform.SetParent(Camera.main.transform, false);
            InGameCanvas.transform.localPosition = new Vector3(0, 0, 0.5f);
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = GorillaTagger.Instance.mainCamera.GetComponent<Camera>();
            canvas.scaleFactor = 0.5f;

            BottomLeftText = createText(TextAlignmentOptions.BottomLeft, font);
            BottomRightText = createText(TextAlignmentOptions.BottomRight, font);
            CenterText = createText(TextAlignmentOptions.Center, font);
            CenterTopText = createText(TextAlignmentOptions.Top, font);
            TopRightText = createText(TextAlignmentOptions.TopRight, font);
            TopLeftText = createText(TextAlignmentOptions.TopLeft, font);

            foreach (TextMeshProUGUI text in Resources.FindObjectsOfTypeAll(typeof(TextMeshProUGUI)))
            {
                if (text.gameObject.name == "LogoText" || text.gameObject.name == "LogoTextVersion") return;
                text.font = font;
            }

            CenterTopText.text = "Starting Notifications...";
            
            string coc = "Tenacity - Private\nPress both joysticks down to open the menu and use the right trigger to press on the different module and or tabs.\nYou scroll each side using their relative joystick, the tabs with the left and the open tab with the right joystick.\nNow fuck off and ruin the game for kids.";
            GameObject.Find("COC Text").GetComponent<Text>().text = coc;
            GameObject.Find("CodeOfConduct").GetComponent<Text>().text = coc;
        }

        TextMeshProUGUI createText(TextAlignmentOptions textAlign, TMP_FontAsset font)
        {
            GameObject InGameCanvasText2 = new GameObject();
            InGameCanvasText2.transform.SetParent(InGameCanvas.transform, false);
            RectTransform transform2;
            if (!InGameCanvasText2.TryGetComponent(out transform2))
            {
                transform2 = InGameCanvasText2.AddComponent<RectTransform>();
            }
            transform2.sizeDelta = new Vector2(1200, 750);
            transform2.localPosition = new Vector3(0, 0, -0.5f);
            transform2.localScale = new Vector3(0.55f, 0.55f, 0.55f);
            TextMeshProUGUI TextComponent = InGameCanvasText2.AddComponent<TextMeshProUGUI>();
            TextComponent.alignment = textAlign;
            TextComponent.text = "";
            TextComponent.fontSize = 50;
            TextComponent.richText = true;
            TextComponent.font = font;

            return TextComponent;
        }

        async void Update()
        {
            try
            {
                BottomLeftText.color = TenacityModSystem.TextLogoColor;
                BottomRightText.color = TenacityModSystem.TextLogoColor;

                BottomLeftText.text = "FPS: " + Mathf.Round(1f / Time.smoothDeltaTime) + "\nSpeed: " + Mathf.Round(GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity.magnitude);

                BottomRightText.text = "Beta - Private  |  " + PhotonNetwork.LocalPlayer.NickName;
            } catch (Exception e)
            {
                Debug.Log(e.StackTrace);
                Debug.Log(e.Message);
            }

            if (!toggled)
            {
                canvasPrefab.SetActive(false);
                TenacityModSystem.NoFingers = false;
            }
            else
            {
                TenacityModSystem.NoFingers = true;
            }
            rightHandCube.SetActive(toggled);

            if (ControllerInputManager.rightTrigger && !uiinteractcooldown)
            {
                rightHandCube.GetComponent<BoxCollider>().enabled = true;
                await Task.Delay(5);
                rightHandCube.GetComponent<BoxCollider>().enabled = false;
                uiinteractcooldown = true;

            }
            else if (!ControllerInputManager.rightTrigger)
            {
                rightHandCube.GetComponent<BoxCollider>().enabled = false;
                uiinteractcooldown = false;
            }

            if (ControllerInputManager.leftAxis2DClick && ControllerInputManager.rightAxis2DClick && !cooldown)
            {
                cooldown = true;

                if (toggled)
                {
                    toggled = false;
                    return;
                }

                if (!toggled)
                {
                    toggled = true;
                    canvasPrefab.SetActive(true);
                    canvasPrefab.transform.rotation = GameObject.Find("Main Camera").transform.rotation;
                    canvasPrefab.transform.position = new Vector3(positionTracker.transform.position.x, GameObject.Find("Main Camera").transform.position.y, positionTracker.transform.position.z);
                }
            }

            if (!ControllerInputManager.leftAxis2DClick && !ControllerInputManager.rightAxis2DClick && cooldown)
            {
                cooldown = false;
            }
        }
    }
}
