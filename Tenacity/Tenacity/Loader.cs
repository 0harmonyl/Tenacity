using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using Tenacity.ModuleSystem;
using Tenacity.Ui;
using UnityEngine;
using UnityEngine.Serialization;
using Utilla;

namespace Tenacity
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.11")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Loader : BaseUnityPlugin
    {
        public static Loader Instance;
        public AssetBundle mainAssetBundle;
        public GameObject theWorld;
        public Vector3 startPosition;
        
        internal void Awake()
        {
            Events.GameInitialized += OnGameInitialized;
        }

        private void OnGameInitialized(object sender, EventArgs e)
        {
            Instance = this;
            
            mainAssetBundle = LoadAssetBundle("Tenacity.Resources.Bundle");
            
            HarmonyPatches.ApplyHarmonyPatches();
            GorillaGameManager.instance.gameObject.AddComponent<ControllerInputPoller>();
            GorillaGameManager.instance.gameObject.AddComponent<ModuleManager>();

            InitializeUi();
        }

        private async void InitializeUi()
        {
            // Main Menu Setup
            
            var mainMenu =
                GameObject.Instantiate(mainAssetBundle.LoadAsset<GameObject>("TenacityMainMenu.prefab"));
            theWorld = GameObject.Find("Environment Objects/LocalObjects_Prefab");
            theWorld.SetActive(false);

            await Task.Delay(5);

            startPosition = GorillaTagger.Instance.transform.position;
            GorillaTagger.Instance.transform.position = new Vector3(0, 1000f, 0);
            GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = false;
            GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
            mainMenu.transform.position = GorillaTagger.Instance.transform.position;
            mainMenu.name = "Tenacity Main Menu";

            mainMenu.AddComponent<MainMenu>();
            
            // Main Menu Setup Finished
        }

        private AssetBundle LoadAssetBundle(string path)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            var bundle = AssetBundle.LoadFromStream(stream);
            stream!.Close();
            return bundle;
        }
    }
}