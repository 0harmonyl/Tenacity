using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    public class Invis : MonoBehaviour, ITenacityModule
    {
        public string Name => "Invis";

        public string Description => "Simple Invis Monke";

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
            
            if (ControllerInputManager.leftGrip)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(0, -60000f, 0);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
    }
}