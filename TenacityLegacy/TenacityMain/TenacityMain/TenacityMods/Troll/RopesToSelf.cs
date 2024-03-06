using GorillaLocomotion.Gameplay;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Troll
{
    internal class RopesToSelf : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Ropes To Self"; }
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

        private float ropeSpeed = 500f;
        static bool active = false;
        private bool isRopesMoving;

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() { }

        public void Update()
        {
            if (!Enabled) return;

            if (ControllerInputManager.leftTrigger && PhotonNetwork.InRoom)
            {
                isRopesMoving = true;
                if (!active)
                {
                    StartCoroutine(RopeFollow());
                    active = true;
                }
            }
            else
            {
                isRopesMoving = false;
                active = false;
            }
        }

        IEnumerator RopeFollow()
        {
            while (isRopesMoving)
            {
                foreach (GorillaRopeSwing s in FindObjectsOfType<GorillaRopeSwing>())
                {
                    Vector3 direction = (GorillaTagger.Instance.headCollider.transform.position - s.transform.position).normalized * ropeSpeed;

                    object[] ar = new object[4];
                    ar[0] = 100;
                    ar[1] = direction;
                    ar[2] = true;
                    s.photonView.RPC("SetVelocity", RpcTarget.All, ar);
                }

                yield return new WaitForSeconds(0.2f);
            }
        }

        public void Cleanup() { }
    }
}
