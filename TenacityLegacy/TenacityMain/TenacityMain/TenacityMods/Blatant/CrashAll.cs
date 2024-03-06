using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Pun;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    public class CrashAll : MonoBehaviour, ITenacityModule
    {
        public string Name => "Crash All";

        public string Description => "Basic Crash All";

        public string Tab => "Blatant Tab";

        public bool Enabled { get; set; }
        public List<TenacityOption> Options { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }
        
        public void Start() { }

        private bool HasAddedSpammers;
        
        public void Update()
        {
            if (Enabled)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-92.2935f, 45.3772f, -20.7123f);
                //GorillaLocomotion.Player.Instance.rightControllerTransform.position =
                    //GorillaTagger.Instance.offlineVRRig.transform.position;

                if (!HasAddedSpammers)
                {
                    AddSpammers();
                }
            }
        }

        public void Cleanup()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;

            RemoveSpammers();
        }

        async void AddSpammers()
        {
            HasAddedSpammers = true;

            await Task.Delay(10000);
            
            int WaterBallonHash = -1674517839;
            int DeadshotHash = 693334698;
            
            GorillaTagger.Instance.gameObject.AddComponent<CrashPPL>().ProjectileHash = WaterBallonHash;
            GorillaTagger.Instance.gameObject.AddComponent<CrashPPL>().ProjectileHash = DeadshotHash;
        }

        void RemoveSpammers()
        {
            foreach (CrashPPL component in GorillaTagger.Instance.gameObject.GetComponents<CrashPPL>())
            {
                Destroy(component);
            }
            
            HasAddedSpammers = false;
        }
    }

    public class CrashPPL : MonoBehaviour
    {
        public int ProjectileHash;
        
        void Update()
        {
            int SlingHashCode = ProjectileHash;
            int ProjectileTrailHashCode =
                PoolUtils.GameObjHashCode(GorillaTagger.Instance.offlineVRRig.slingshot.projectileTrail);
            int PlayerProjectileCount = 0;
            Vector3 RightHandPos = GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(0, -3f, 0);
            Vector3 SlingDirection = Vector3.zero;
            PhotonView.Get(GorillaGameManager.instance).RPC("LaunchSlingshotProjectile", RpcTarget.Others, new object[]
            {
                RightHandPos,
                SlingDirection,
                SlingHashCode,
                ProjectileTrailHashCode,
                false,
                PlayerProjectileCount,
                false,
                0f,
                0f,
                0f,
                1f
            });
        }
    }
}