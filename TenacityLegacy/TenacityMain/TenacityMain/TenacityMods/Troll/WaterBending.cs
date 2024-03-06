using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Troll
{
    internal class WaterBending : BaseWaterMod, ITenacityModule
    {
        public string Name
        {
            get { return "Water Bending"; }
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

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() { }

        public void Update()
        {
            if (!Enabled) return;

            if (ControllerInputManager.leftGrip)
                StartWaterBend(GorillaTagger.Instance.leftHandTransform);
            if (ControllerInputManager.rightGrip)
                StartWaterBend(GorillaTagger.Instance.rightHandTransform);
        }

        public void Cleanup() { }
    }
}
