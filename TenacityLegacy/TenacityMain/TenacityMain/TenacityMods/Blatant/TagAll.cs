using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TenacityLib;
using TenacityMain.ModSystem;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    internal class TagAll : MonoBehaviourPunCallbacks, ITenacityModule
    {
        public string Name
        {
            get { return "Tag All"; }
        }

        public string Description
        {
            get { return "Spinny Spin Spin"; }
        }

        public string Tab
        {
            get { return "Blatant Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public float RotationSpeed = 20f;
        public float Speed = 1000f;
        public float CircleRadius = 1.25f;

        private bool IsMoving = true;
        private bool Cooldown, RegCooldown;
        private bool Nonce;
        private bool InProgress;
        private bool IsTaggingInProgress = false;
        private List<VRRig> VRRigs = new List<VRRig>();
        private int CurrentRigIndex = 0;
        private float Angle = 0.0f;
        private List<MeshCollider> MeshColliderList = new List<MeshCollider>();
        private Vector3 originalPosition;
        private bool nonce;
        private bool IsTaggin;

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Mode",
                    DropdownOptions =
                    {
                        "Spin",
                        "Regular"
                    },
                    SelectedDropdown = "Regular",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown
                }
            };
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void Start() { }

        void OnDestroy()
        {
            // Unsubscribe from network callbacks when the script is destroyed.
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            ResetTaggingState();
        }

        public void Update()
        {
            if (!Enabled) return;

            if (GorillaGameManager.instance.GameType() != GameModeType.Infection)
            {
                Enabled = false;
                TenacityModSystem.UpdateAllControllerStates();
                return;
            }

            if (Options[0].SelectedDropdown == "Spin")
            {
                 if (!nonce)
                {
                    MeshCollider[] meshes = Resources.FindObjectsOfTypeAll<MeshCollider>();
                    foreach (MeshCollider mesh in meshes)
                    {
                        MeshColliderList.Add(mesh);
                    }

                    // Ensure we're connected to the Photon server.
                    if (PhotonNetwork.IsConnected)
                    {
                        OnConnectedToMaster();
                    }
                    nonce = true;
                }

                if (!PhotonNetwork.InRoom) return;

                if (ControllerInputManager.rightGrip)
                {
                    if (!Cooldown)
                    {
                        if (IsTaggingInProgress)
                        {
                            IsTaggingInProgress = false;
                            ResetTaggingState();
                        }
                        else
                        {
                            InProgress = true;
                            Nonce = false;
                            Cooldown = true;
                            VRRigs.Clear();
                            VRRig[] rigs = GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren<VRRig>();
                            foreach (VRRig rig in rigs)
                            {
                                if (rig.gameObject.activeSelf && Vector3.Distance(rig.gameObject.transform.position, GorillaTagger.Instance.transform.position) < 200f && (rig.mainSkin.material.name != "infected (Instance)" || rig.mainSkin.material.name != "it") && rig.transform.position != new Vector3(58.857f, -57.5251f, 58.9085f))
                                {
                                    VRRigs.Add(rig);
                                }
                            }

                            if (VRRigs.Count > 0)
                            {
                                IsTaggingInProgress = true;
                                originalPosition = transform.position;
                            }
                        }
                    }
                }

                if (!ControllerInputManager.rightGrip)
                {
                    Cooldown = false;
                }

                if (!InProgress)
                {
                    foreach (MeshCollider mesh in MeshColliderList)
                    {
                        // Check if the mesh is not null before enabling it.
                        if (mesh != null)
                        {
                            mesh.enabled = true;
                        }
                    }

                    return;
                }

                if (InProgress)
                {
                    if (!Nonce)
                    {
                        foreach (MeshCollider mesh in MeshColliderList)
                        {
                            // Check if the mesh is not null before disabling it.
                            if (mesh != null)
                            {
                                mesh.enabled = false;
                            }
                        }

                        Nonce = true;
                    }
                }

                if (CurrentRigIndex >= VRRigs.Count)
                {
                    ResetTaggingState();
                }

                if (IsTaggingInProgress)
                {
                    if (ControllerInputManager.leftGrip)
                    {
                        ResetTaggingState();
                    }

                    if (CurrentRigIndex < VRRigs.Count)
                    {
                        VRRig CurrentRig = VRRigs[CurrentRigIndex];
                        Vector3 PlayerPosition = CurrentRig.transform.position + new Vector3(0, 0.1f, 0);

                        GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity -= new Vector3(0.1f, 0.1f, 0.1f);

                        if (IsMoving)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, Time.deltaTime * Speed);

                            if (Vector3.Distance(transform.position, PlayerPosition) < 0.1f)
                            {
                                IsMoving = false;
                            }
                        }
                        else
                        {
                            Angle += RotationSpeed * Time.deltaTime;
                            float x = Mathf.Cos(Angle) * CircleRadius;
                            float z = Mathf.Sin(Angle) * CircleRadius;
                            transform.position = PlayerPosition + new Vector3(x, 0.0f, z);

                            Vector3 position = CurrentRig.transform.position;

                            GorillaTagger.Instance.leftHandTransform.position = position;
                        }

                        if (!IsMoving && (CurrentRig.mainSkin.material.name == "infected (Instance)" || CurrentRig.mainSkin.material.name == "it"))
                        {
                            CurrentRigIndex++;

                            if (CurrentRigIndex < VRRigs.Count)
                            {
                                IsMoving = true;
                            }
                            else
                            {
                                ResetTaggingState();
                            }
                        }
                    }
                }
            }

            if (Options[0].SelectedDropdown == "Regular")
            {
                if (ControllerInputManager.rightGrip && !RegCooldown)
                {
                    if (GorillaTagger.Instance.offlineVRRig.mainSkin.material.name != "infected (Instance)")
                    {
                        return;
                    }
                    
                    RegCooldown = true;
                    IsTaggin = true;
                }

                if (IsTaggin)
                {
                    RegularTagPlayers();
                }
            }
        }

        void RegularTagPlayers()
        {
            VRRig[] rigs = GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren<VRRig>();

            int index = -1;
            for (int i = 0; i < rigs.Length; i++)
            {
                if (rigs[i].mainSkin.material.name != "infected (Instance)")
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                IsTaggin = false;
                RegCooldown = false;
                return;
            }

            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.position = rigs[index].transform.position;
            GorillaTagger.Instance.leftHandTransform.position = rigs[index].transform.position;
        }

        async void ResetTaggingState()
        {
            InProgress = false;
            Nonce = false;
            Cooldown = false;
            IsTaggingInProgress = false;
            CurrentRigIndex = 0;

            // Reset the player's position to the original position
            transform.position = originalPosition;

            await Task.Delay(25);

            foreach (MeshCollider mesh in MeshColliderList)
            {
                // Check if the mesh is not null before enabling it.
                if (mesh != null)
                {
                    mesh.enabled = true;
                }
            }
        }

        public void Cleanup() { }
    }
}
