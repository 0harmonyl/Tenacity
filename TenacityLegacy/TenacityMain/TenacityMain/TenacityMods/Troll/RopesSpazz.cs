using GorillaLocomotion.Gameplay;
using System.Collections.Generic;
using System.Threading.Tasks;
using TenacityLib;
using UnityEngine;
using Random = System.Random;

namespace TenacityMain.TenacityMods.Troll
{
    internal class RopesSpazz : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Ropes Spazz"; }
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

        static bool cooldown;

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

        private Vector3 GetRandomPosition()
        {
            Random r = new Random();

            float z = (float)r.NextDouble() * (100f - -100f) + -100f;

            float x = (float)r.NextDouble() * (100f - -100f) + -100f;

            float y = (float)r.NextDouble() * (100f - -100f) + -100f;

            Vector3 result = new Vector3(x, y, z);

            return result;
        }

        void StartRopeVelocity()
        {
            if (cooldown) return;
            cooldown = true;
            RopeDelay();
        }

        private async void RopeDelay()
        {
            await Task.Delay(200);

            RopeVelocity();

            await Task.Delay(200);

            cooldown = false;
        }

        private void RopeVelocity()
        {
            foreach (GorillaRopeSwing s in FindObjectsOfType<GorillaRopeSwing>())
            {
                object[] ar = new object[4];
                ar[0] = 100;
                ar[1] = GetRandomPosition();
                ar[2] = true;
                s.photonView.RPC("SetVelocity", 0, ar);
            }
        }

        public void Cleanup() { }
    }
}
