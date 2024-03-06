using HarmonyLib;
using TenacityMain.ModSystem;

namespace TenacityMain.TenacityMods.Misc
{
    [HarmonyPatch(typeof(VRMapIndex), "MapMyFinger", MethodType.Normal)]
    class Index
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (TenacityModSystem.NoFingers)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(VRMapThumb), "MapMyFinger", MethodType.Normal)]
    class Thumb
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (TenacityModSystem.NoFingers)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(VRMapMiddle), "MapMyFinger", MethodType.Normal)]
    class Middle
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (TenacityModSystem.NoFingers)
            {
                return false;
            }
            return true;
        }
    }
}
