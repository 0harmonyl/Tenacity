using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Troll
{
    internal class WateryAir : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Watery Air"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public string Tab
        {
            get { return "Troll Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        private GameObject waterbox;

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() { }

        public void Update()
        {
            if (!Enabled) return;

            if (waterbox == null)
            {
                waterbox = Instantiate(GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach/ForestToBeach_Prefab_V4/CaveWaterVolume"));
                Destroy(waterbox.GetComponentInChildren<Renderer>());
            }

            if (ControllerInputManager.leftTrigger && ControllerInputManager.rightTrigger)
            {
                waterbox.transform.position = GorillaTagger.Instance.transform.position + new Vector3(0, 2, 0);
            }
            else
            {
                waterbox.transform.position = new Vector3(0, -69420, 0);
            }
        }

        public void Cleanup() { }
    }
}
