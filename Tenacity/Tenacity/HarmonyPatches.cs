using System.Reflection;
using HarmonyLib;

namespace Tenacity
{
    public class HarmonyPatches
    {
        private static Harmony _instance;

        private static bool IsPatched { get; set; }
        private const string InstanceId = PluginInfo.GUID;

        internal static void ApplyHarmonyPatches()
        {
            if (!IsPatched)
            {
                _instance ??= new Harmony(InstanceId);

                _instance.PatchAll(Assembly.GetExecutingAssembly());
                IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (_instance == null || !IsPatched) return;
            _instance.UnpatchSelf();
            IsPatched = false;
        }
    }
}