using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace TenacityMain.TenacityMods.Patches
{
    [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCall")]
    public class NoRPCCall : MonoBehaviour
    {
        private static bool Prefix(PhotonMessageInfo info, string callingMethod = "")
        {
            return false;
        }
    }
}