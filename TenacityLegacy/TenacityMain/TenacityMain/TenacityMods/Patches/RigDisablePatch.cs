using HarmonyLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Patches
{
    [HarmonyPatch(typeof(VRRig), "OnDisable")]
    public class RigDisablePatch : MonoBehaviour
    {
        public static bool Prefix(VRRig __instance)
        {
            if (__instance == GorillaTagger.Instance.offlineVRRig)
            {
                return false;
            }

            return true;
        }
    }
}