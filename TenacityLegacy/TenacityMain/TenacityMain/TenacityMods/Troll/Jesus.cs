using GorillaLocomotion.Swimming;
using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Troll
{
    internal class Jesus : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Jesus"; }
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

        bool nonce;
        List<WaterVolume> volumes = new List<WaterVolume>();

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() { }

        public void Update()
        {
            if (!Enabled) return;

            if (ControllerInputManager.leftGrip)
            {
                if (!nonce)
                {
                    foreach (WaterVolume v in Resources.FindObjectsOfTypeAll(typeof(WaterVolume)))
                    {
                        volumes.Add(v);
                    }
                    nonce = true;
                }

                foreach (WaterVolume v in volumes)
                {
                    v.gameObject.layer = 0;
                }
            }
            else
            {
                foreach (WaterVolume v in volumes)
                {
                    v.gameObject.layer = LayerMask.NameToLayer("Water");
                }
            }
        }

        public void Cleanup() { }
    }
}
