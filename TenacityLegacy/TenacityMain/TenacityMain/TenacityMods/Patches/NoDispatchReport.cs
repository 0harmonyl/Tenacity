using HarmonyLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Patches
{
    [HarmonyPatch(typeof(GorillaNot), "DispatchReport")]
    public class NoDispatchReport : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}