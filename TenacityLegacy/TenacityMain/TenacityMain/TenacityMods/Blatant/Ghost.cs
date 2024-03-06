using System;
using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    public class Ghost : MonoBehaviour, ITenacityModule
    {
        public string Name => "Ghost";

        public string Description => "Simple Ghost Monke";

        public string Tab => "Blatant Tab";
        
        public bool Enabled { get; set; }
        public List<TenacityOption> Options { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() { }
        
        public void Cleanup() { }
        
        public void Update()
        {
            if (!Enabled) return;
            GorillaTagger.Instance.offlineVRRig.enabled = !ControllerInputManager.leftGrip;
        }
    }
}