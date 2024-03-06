using System.Collections.Generic;
using TenacityLib;
using TenacityMain.ModSystem;
using UnityEngine;

namespace TenacityMain.TenacityMods.Misc
{
    internal class NoFingersModule : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "No Fingers"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public string Tab
        {
            get { return "Misc Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }
        public bool MenuOverride = false;

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() { }

        public void Update()
        {
            if (Enabled && !MenuOverride)
            {
                TenacityModSystem.NoFingers = true;
            } 
            else if (!Enabled && !MenuOverride)
            {
                TenacityModSystem.NoFingers = false;
            }
            else
            {
                return;
            }
        }

        public void Cleanup() { }
    }
}
