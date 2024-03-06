using Photon.Pun;
using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Misc
{
    internal class Disconnect : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Disconnect"; }
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
                        "Keybind (LT & LG)",
                        "Always",
                    },
                    SelectedDropdown = "Keybind (LT & LG)"
                }
            };
        }

        public void Start() { }

        public void Update()
        {
            if (!Enabled) return;

            if (Options[0].SelectedDropdown == "Keybind (LT & LG)")
            {
                if (ControllerInputManager.leftGrip && ControllerInputManager.leftTrigger)
                {
                    PhotonNetwork.Disconnect();
                }
            }
            else
            {
                PhotonNetwork.Disconnect();
            }
        }

        public void Cleanup() { }
    }
}
