using GorillaLocomotion.Gameplay;
using System.Collections.Generic;
using System.Threading.Tasks;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Troll
{
    internal class RopesFreeze : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Ropes Freeze"; }
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

        bool cooldown;

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() { }

        public void Update()
        {
            if (!Enabled) return;

            StartRopeVelocity();
        }

        void StartRopeVelocity()
        {
            if (cooldown) return;
            cooldown = true;
            RopeDelay();
        }

        private async void RopeDelay()
        {
            await Task.Delay(50);

            RopeVelocity();

            await Task.Delay(50);

            cooldown = false;
        }

        private void RopeVelocity()
        {
            foreach (GorillaRopeSwing s in FindObjectsOfType<GorillaRopeSwing>())
            {
                object[] ar = new object[4];
                ar[0] = 100;
                ar[2] = true;
                s.photonView.RPC("SetVelocity", 0, ar);
            }
        }

        public void Cleanup() { }
    }
}
