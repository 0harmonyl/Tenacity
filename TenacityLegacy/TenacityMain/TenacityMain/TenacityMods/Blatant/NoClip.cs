using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    internal class NoClip : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "NoClip"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public string Tab
        {
            get { return "Blatant Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public static List<MeshCollider> noClipList = new List<MeshCollider>();

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() 
        {
            foreach (MeshCollider mesh in Resources.FindObjectsOfTypeAll(typeof(MeshCollider)))
            {
                noClipList.Add(mesh);
            }
        }

        public void Update()
        {
            if (!Enabled) return;

            if (!ControllerInputManager.leftTrigger)
            {
                foreach (MeshCollider meh in noClipList)
                {
                    meh.enabled = true;
                }

                return;
            }

            foreach (MeshCollider mesh in noClipList)
            {
                mesh.enabled = false;
            }
        }

        public void Cleanup() { }
    }
}
