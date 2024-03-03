using System;
using BepInEx;
using Tenacity.ModuleSystem;
using Utilla;

namespace Tenacity
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.11")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Loader : BaseUnityPlugin
    {
        public static Loader Instance;
        
        internal void Awake()
        {
            Events.GameInitialized += OnGameInitialized;
        }

        private void OnGameInitialized(object sender, EventArgs e)
        {
            Instance = this;
            
            HarmonyPatches.ApplyHarmonyPatches();
            GorillaGameManager.instance.gameObject.AddComponent<ControllerInputPoller>();
            GorillaGameManager.instance.gameObject.AddComponent<ModuleManager>();
        }
    }
}