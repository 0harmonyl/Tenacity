using Photon.Pun;
using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Misc
{
    internal class UnTag : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Un Tag"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public string Tab
        {
            get { return "Misc Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Mode",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Keybind (RT)",
                        "Always"
                    },
                    SelectedDropdown = "Keybind (RT)",
                }
            };
        }

        public void Start() { }

        public void Update()
        {
            if (!Enabled) return;

            if (Options[0].SelectedDropdown != "Keybind (RT)" && PhotonNetwork.InRoom && PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                foreach (GorillaTagManager gorillaTagManager in FindObjectsOfType<GorillaTagManager>())
                {
                    gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                    gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                    gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                    gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                    gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                    gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                }
            }
            else if (Options[0].SelectedDropdown == "Keybind (RT)" && PhotonNetwork.InRoom && PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (ControllerInputManager.rightTrigger)
                {
                    foreach (GorillaTagManager gorillaTagManager in FindObjectsOfType<GorillaTagManager>())
                    {
                        gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                        gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                        gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                        gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                        gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                        gorillaTagManager.currentInfected.Remove(PhotonNetwork.LocalPlayer);
                    }
                }
            }
        }

        public void Cleanup() { }
    }
}
