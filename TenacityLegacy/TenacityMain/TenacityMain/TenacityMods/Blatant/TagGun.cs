using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TenacityLib;
using TenacityMain.ModSystem;
using TenacityMain.UserInterface.InGameUi;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    public class TagGun : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Tag Gun"; }
        }

        public string Description 
        { 
            get { return "Regular Tag Gun"; }
        }

        public string Tab
        {
            get { return "Blatant Tab"; }
        }

        public bool Enabled { get; set; }
        public List<TenacityOption> Options { get; set; }

        private GameObject circle = null;
        private float DistanceToLockOn = 15f;
        private bool LockedOn;
        private bool IsTagging;
        private VRRig TargetRig;
        private Vector3 StartPosition;
        private List<MeshCollider> meshes = new List<MeshCollider>();

        private List<GorillaTagManager> tagManagers = new List<GorillaTagManager>();

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() 
        {
            tagManagers.AddRange(Resources.FindObjectsOfTypeAll<GorillaTagManager>());
        }

        void Update()
        {
            if (!PhotonNetwork.InRoom && Enabled)
            {
                if (tagManagers.Count > 0)
                {
                    tagManagers.Clear();
                }
            }
            
            if (!Enabled) return;

            if (tagManagers.Count == 0)
            {
                Start();
            }

            if (ControllerInputManager.rightGrip && !IsTagging)
            {
                bool isTagged = false;

                foreach (var tm in tagManagers)
                {
                    if (tm.currentInfectedArray.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
                    {
                        isTagged = true;
                    }
                }

                if (!isTagged) return;

                RaycastHit hit;
                Transform right = GorillaTagger.Instance.rightHandTransform;
                if (Physics.Raycast(right.position - right.up, -right.up, out hit, 1000f))
                {
                    if (circle == null)
                    {
                        circle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        circle.AddComponent<MeshRenderer>();
                        circle.GetComponent<MeshRenderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                        Destroy(circle.GetComponent<Collider>());
                        Destroy(circle.GetComponent<Rigidbody>());
                        circle.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    }

                    circle.transform.position = hit.point;

                    foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent")
                                 .GetComponentsInChildren<VRRig>())
                    {
                        if (rig.mainSkin.material.name == "infected (Instance)" || rig.mainSkin.material.name == "it")
                        {
                            continue;
                        }

                        float distance = Vector3.Distance(rig.transform.position, hit.point);

                        if (distance < DistanceToLockOn)
                        {
                            hit.point = rig.transform.position;
                            TargetRig = rig;
                            LockedOn = true;
                            circle.transform.position = hit.point;
                        }
                    }

                    if (LockedOn)
                    {
                        circle.transform.localScale = new Vector3(2f, 2f, 2f);
                    }

                    if (!LockedOn)
                    {
                        circle.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        return;
                    }

                    if (ControllerInputManager.rightTrigger)
                    {
                        IsTagging = true;
                        StartPosition = GorillaTagger.Instance.transform.position;
                    }

                    LockedOn = false;
                }
            }
            else if (!ControllerInputManager.rightGrip)
            {
                Destroy(circle);
                circle = null;
            }

            if (IsTagging)
            {
                TagPlayer();
            }
        }

        private void TagPlayer()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;

            GorillaTagger.Instance.offlineVRRig.transform.position = TargetRig.transform.position;

            GorillaTagger.Instance.leftHandTransform.position = TargetRig.transform.position;

            if (TargetRig.mainSkin.material.name == "infected (Instance)" || TargetRig.mainSkin.material.name == "it")
            {
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;

                TargetRig = null;

                IsTagging = false;
            }
        }

        public void Cleanup() { }
    }
}