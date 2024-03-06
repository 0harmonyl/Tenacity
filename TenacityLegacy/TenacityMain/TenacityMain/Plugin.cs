using BepInEx;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ExitGames.Client.Photon.StructWrapping;
using TenacityLib;
using TenacityMain.Effects;
using TenacityMain.ModSystem;
using TenacityMain.Passive;
using TenacityMain.UserInterface.InGameUi;
using TenacityMain.UserInterface.MainMenu;
using TenacityMain.UserInterface.NotificationLib;
using TenacityMain.UserInterface.OnScreenUi;
using TenacityMain.Utils;
using UnityEngine;
using Utilla;

namespace TenacityMain
{
    /// <summary>
    /// Tenacity Main Class
    /// </summary>

    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.11")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static AssetBundle MainAssetBundle;
        public static GameObject TheWorld;
        public static Vector3 StartPosition;
        public static Material HeadLockMat;

        private void Start()
        {
            Debug.Log("I'm Starting");
        }

        public void Awake()
        {
            Events.GameInitialized += OnGameInitialized;
        }

        public void OnGameInitialized(object sender, EventArgs e)
        {
            HarmonyPatches.ApplyHarmonyPatches();

            MainAssetBundle = LoadAssetBundle("TenacityMain.Resources.tenacitybundle");

            GorillaTagger.Instance.gameObject.AddComponent<ControllerInputManager>();

            GorillaTagger.Instance.gameObject.AddComponent<TenacityModSystem>();
            GorillaTagger.Instance.gameObject.AddComponent<GorillaNotFucker>();
            GorillaTagger.Instance.gameObject.AddComponent<TenacityColorManager>();
            GorillaTagger.Instance.gameObject.AddComponent<TenacityProjectileTracker>();
            GorillaTagger.Instance.gameObject.AddComponent<InGameNotificationCounter>();

            OpenMainMenu();

            SetupOnScreenUi();

            SetupInGameUi();
        }

        void SetupOnScreenUi()
        {
            GameObject onScreenUi = new GameObject();
            onScreenUi.AddComponent<OnScreenUi>();
            onScreenUi.GetComponent<OnScreenUi>().Initialize();
        }

        void SetupInGameUi()
        {
            GameObject inGameUi = new GameObject();
            inGameUi.name = "TenacityPlayerUi";
            inGameUi.transform.SetParent(GameObject.Find("GorillaPlayer").transform);
            inGameUi.AddComponent<InGameUi>().Initialize();
        }

        async void OpenMainMenu()
        {
            GameObject MainMenu = GameObject.Instantiate(MainAssetBundle.LoadAsset<GameObject>("TenacityMainMenu.prefab"));
            TheWorld = GameObject.Find("Environment Objects/LocalObjects_Prefab");
            TheWorld.SetActive(false);

            await Task.Delay(10);

            StartPosition = GorillaTagger.Instance.transform.position;
            GorillaTagger.Instance.transform.position = new Vector3(0, 1000f, 0);
            GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = false;
            GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
            MainMenu.transform.position = GorillaTagger.Instance.transform.position;
            MainMenu.name = "Tenacity Main Menu";

            MainMenu.AddComponent<TenacityMainMenu>();

            if (GameObject.Find("CameraTablet(Clone)"))
            {
                GameObject.Find("CameraTablet(Clone)").transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0.5f, 0, 0);
            }
        }

        private AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }
    }
}
