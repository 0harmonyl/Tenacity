using GorillaLocomotion;
using Photon.Pun;
using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Troll
{
    internal class DougHalo : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Doug Halo"; }
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

        float RotationSpeed = 2;
        float UpAndDownSpeed = 0.25f;
        float UpAndDownRange = 1f;
        float CircleRadius = 0.5f;
        float ElevationOffset = 0.5f;
        Vector3 position;
        private float angle;

        public void Setup() 
        {
            Options = new List<TenacityOption>();
        }

        public void Start() { }

        public void Update()
        {
            if(!Enabled) return;

            GameObject floatingBug = GameObject.Find("Floating Bug Holdable");
            if (floatingBug != null && PhotonNetwork.InRoom && floatingBug.GetComponent<ThrowableBug>().ownerRig != GorillaTagger.Instance.myVRRig)
            {
                floatingBug.GetComponent<ThrowableBug>().OnGrab(null, null);

                Transform playerHeadTransform = Player.Instance.headCollider.transform;
                Vector3 position2;
                position2 = playerHeadTransform.position;
                position2.y -= 0.55f;
                position.Set(
                    Mathf.Cos(angle) * CircleRadius,
                    ElevationOffset,
                    Mathf.Sin(angle) * CircleRadius
                );
                floatingBug.transform.position = position2 + position;
                angle += Time.deltaTime * RotationSpeed;
                ElevationOffset = Mathf.PingPong(Time.time * UpAndDownSpeed, UpAndDownRange);
            }
        }

        public void Cleanup() { }
    }
}
