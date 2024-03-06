using GorillaLocomotion.Gameplay;
using System.Collections.Generic;
using System.Threading.Tasks;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Troll
{
    internal class RopesDown : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Ropes Down"; }
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

        bool cooldown = false;

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() { }

        public void Update()
        {
            if (!Enabled) return;

            StartRopeVelocity(200f);
        }

        void StartRopeVelocity(float strength)
        {
            if (cooldown) return;
            cooldown = true;
            RopeDelay(strength);
        }

        private async void RopeDelay(float strength)
        {
            await Task.Delay(200);

            RopeVelocity(strength);

            await Task.Delay(200);

            cooldown = false;
        }

        private void RopeVelocity(float strength)
        {
            foreach (GorillaRopeSwing s in FindObjectsOfType<GorillaRopeSwing>())
            {
                float velocityStrength = -(strength - 100) * 50f;
                Vector3 velocity = new Vector3(0, velocityStrength, 0);

                object[] ar = new object[4];
                ar[0] = 100;
                ar[1] = velocity;
                ar[2] = true;
                GorillaGameManager.instance.returnPhotonView.RPC("SetVelocity", 0, ar);
            }
        }

        public void Cleanup() { }
    }
}
