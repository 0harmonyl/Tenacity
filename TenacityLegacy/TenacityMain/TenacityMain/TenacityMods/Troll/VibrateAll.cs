using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Troll
{
    internal class VibrateAll : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Vibrate All";  }
        }

        public string Description
        {
            get { return "Vibrate All Controllers"; }
        }

        public string Tab
        {
            get { return "Troll Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() { }

        public void Update()
        {
            if (PhotonNetwork.InRoom && Enabled)
            {
                Photon.Realtime.Player[] pL = PhotonNetwork.PlayerList;
                GorillaGameManager m = GorillaGameManager.instance;
                foreach (Photon.Realtime.Player p in pL)
                {
                    PhotonView pV = m.FindVRRigForPlayer(p);
                    if (pV != null)
                    {
                        pV.RPC("SetJoinTaggedTime", RpcTarget.Others, Array.Empty<object>());
                    }
                }
            }
        }

        public void Cleanup() { }
    }
}
