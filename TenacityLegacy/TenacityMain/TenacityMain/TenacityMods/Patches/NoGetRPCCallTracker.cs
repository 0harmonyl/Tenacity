using HarmonyLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Patches
{
    [HarmonyPatch(typeof(GorillaNot), "GetRPCCallTracker")]
    public class NoGetRPCCallTracker : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}