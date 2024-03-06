using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace TenacityMain.TenacityMods.Troll
{
    public class BaseWaterMod : MonoBehaviour
    {
        static bool cooldown = false;

        public void StartWaterBend(Transform LR)
        {
            if (cooldown) return;
            cooldown = true;
            StartCoroutine(WaterBendDelay(LR));
        }

        private static IEnumerator WaterBendDelay(Transform LR)
        {
            yield return new WaitForSeconds(0.15f);

            WaterBend(LR);

            yield return new WaitForSeconds(0.15f);

            cooldown = false;
        }

        private static void WaterBend(Transform LR)
        {
            GorillaTagger.Instance.myVRRig.RPC("PlaySplashEffect", RpcTarget.All, new object[]
            {
                LR.position,
                LR.rotation,
                4f,
                100f,
                true,
                false
            });
        }
    }
}
