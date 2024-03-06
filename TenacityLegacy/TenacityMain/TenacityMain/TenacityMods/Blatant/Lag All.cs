using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Pun;
using TenacityLib;
using TenacityMain.ModSystem;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    public class Lag_All : MonoBehaviour
    {
        public string Name
        {
            get { return "Lag All"; }
        }

        public string Description 
        { 
            get { return "Projctile Based Lag All"; }
        }

        public string Tab
        {
            get { return "Blatant Tab"; }
        }

        public bool Enabled { get; set; }
        public List<TenacityOption> Options { get; set; }

        private bool Nonce, Nonce1;

        public void Setup()
        { 
            Options = new List<TenacityOption>();
        }

        public void Cleanup()
        {
            if (!Nonce1)
            {
                Nonce1 = true;
                return;
            }
            
            Destroy(particleSpawner);
        }

        public void Start()
        {
            if (!Nonce)
            {
                Nonce = true;
                return;
            }
            
            GameObject spammerObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            spammerObject.transform.SetParent(GameObject.Find("Environment Objects").transform, true);
            Destroy(spammerObject.GetComponent<MeshRenderer>());
            Destroy(spammerObject.GetComponent<Collider>());

            particleSpawner = spammerObject;

            particleSpawner.AddComponent<SpinFuckerSpin>();

            particleSpawner.AddComponent<ProjectileLagger>().ProjectileString = "Environment Objects/PersistentObjects_Prefab/GlobalObjectPools/SnowballProjectile(Clone)";
            particleSpawner.AddComponent<ProjectileLagger>().ProjectileString = "Environment Objects/PersistentObjects_Prefab/GlobalObjectPools/SnowballProjectile(Clone)";
            particleSpawner.AddComponent<ProjectileLagger>().ProjectileString = "Environment Objects/PersistentObjects_Prefab/GlobalObjectPools/SnowballProjectile(Clone)";
        }
        
        //////////////////////////////////////////////////////////////////

        private GameObject particleSpawner;

        public void Update()
        {
            if (!Enabled)
            {
                TenacityModSystem.Projctiles = true;
                return;
            };

            if (!PhotonNetwork.InRoom)
            {
                FullLagClear();
                Enabled = false;
                TenacityModSystem.UpdateAllControllerStates();
                Destroy(particleSpawner);
                return;
            }

            TenacityModSystem.Projctiles = false;
        }
        
        private bool DeleteMode;
        
        async void FullLagClear()
        {
            await Task.Delay(5000);

            int index = 0;
            
            foreach (Transform obj in GameObject.Find("Environment Objects/PersistentObjects_Prefab/GlobalObjectPools").transform)
            {
                if (index > 9000)
                {
                    DeleteMode = true;
                }
                else
                {
                    DeleteMode = false;
                }
                
                if (DeleteMode)
                {
                    Destroy(obj.gameObject);
                }
                else
                {
                    obj.gameObject.SetActive(false);
                }

                index++;
            }

            if (index > 9000)
            {
                DeleteMode = true;
            }
            else
            {
                DeleteMode = false;
            }
        }
    }

    internal class ProjectileLagger : MonoBehaviour
    {
        public string ProjectileString;
        private bool positive;

        void Update()
        {
            GameObject slingshotObject = GameObject.Find(ProjectileString);
            GameObject instantiatedSlingshot = ObjectPools.instance.Instantiate(slingshotObject);
            int SlingHashCode = PoolUtils.GameObjHashCode(instantiatedSlingshot); 
            SlingshotProjectile slingComponent = instantiatedSlingshot.GetComponent<SlingshotProjectile>();
            int ProjectileTrailHashCode =
                PoolUtils.GameObjHashCode(GorillaTagger.Instance.offlineVRRig.slingshot.projectileTrail);
            int PlayerProjectileCount = 0;
            Vector3 RightHandPos = transform.position;
            Vector3 SlingDirection = Vector3.zero;
            if (!positive)
            {
                positive = true;
                SlingDirection =
                    Vector3.up * Time.deltaTime * 750f;
            }
            else
            {
                positive = false;
                SlingDirection =
                    Vector3.up * Time.deltaTime * 750f;
            }
            GorillaGameManager.instance.returnPhotonView.RPC("LaunchSlingshotProjectile", RpcTarget.Others, new object[]
            {
                RightHandPos,
                SlingDirection,
                SlingHashCode,
                ProjectileTrailHashCode,
                false,
                PlayerProjectileCount,
                false,
                1f,
                1f,
                1f,
                1f
            });
            slingshotObject.SetActive(true);
            slingComponent.Launch(RightHandPos, SlingDirection, PhotonNetwork.LocalPlayer, true, 
                false, PlayerProjectileCount, 1f, false, Color.red);
        }
    }

    internal class SpinFuckerSpin : MonoBehaviour
    {
        void Update()
        {
            //transform.Rotate(Vector3.up, 100f * Time.deltaTime);

            transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 2f, 0f);
        }
    }
}