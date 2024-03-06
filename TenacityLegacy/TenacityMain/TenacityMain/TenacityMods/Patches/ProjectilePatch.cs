using HarmonyLib;
using TenacityMain.ModSystem;
using UnityEngine;

namespace TenacityMain.TenacityMods.Patches
{
    [HarmonyPatch(typeof(Slingshot), "LaunchProjectile")]
    public class ProjectilePatch : MonoBehaviour
    {
        public static bool Prefix()
        {
            return TenacityModSystem.Projctiles;
        }
    }
}