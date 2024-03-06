using Photon.Pun;
using System.Collections.Generic;
using TenacityLib;
using TenacityMain.Effects;
using UnityEngine;

namespace TenacityMain.TenacityMods.Visuals
{
    internal class Chams : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Chams"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public string Tab
        {
            get { return "TenacityInGameUiVisualsTab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Custom Color",
                    OptionType = TenacityOption.TenacityOptionType.Toggle,
                    Enabled = false,
                },
                new TenacityOption()
                {
                    Name = "Color",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Tenacity",
                        "Dark Red",
                        "Red",
                        "Orange",
                        "Mint Green",
                        "Green",
                        "Blue",
                        "Pink",
                        "Purple",
                    },
                    SelectedDropdown = "Tenacity"
                }
            };
        }

        public void Start() { }

        public void Update()
        {
            if (Enabled && PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren<VRRig>())
                {
                    if (Options[0].Enabled)
                    {
                        rig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");

                        Color newColor = new Color();

                        if (Options[1].SelectedDropdown == "Tenacity")
                            newColor = TenacityColorManager.TenacityColor;
                        if (Options[1].SelectedDropdown == "Dark Red")
                            newColor = new Color32(145, 15, 0, 255);
                        if (Options[1].SelectedDropdown == "Red")
                            newColor = Color.red;
                        if (Options[1].SelectedDropdown == "Orange")
                            newColor = new Color32(255, 190, 11, 255);
                        if (Options[1].SelectedDropdown == "Mint Green")
                            newColor = new Color32(128, 255, 114, 255);
                        if (Options[1].SelectedDropdown == "Green")
                            newColor = new Color32(53, 191, 29, 255);
                        if (Options[1].SelectedDropdown == "Blue")
                            newColor = new Color32(32, 164, 243, 255);
                        if (Options[1].SelectedDropdown == "Pink")
                            newColor = new Color32(255, 159, 243, 255);
                        if (Options[1].SelectedDropdown == "Purple")
                            newColor = new Color32(98, 71, 170, 255);

                        if (rig.mainSkin.material.name == "infected (Instance)" || rig.mainSkin.material.name == "it")
                        {
                            rig.mainSkin.material.color = new Color32(237, 138, 0, 255);
                        }
                        else
                        {
                            rig.mainSkin.material.color = newColor;
                        }
                    }
                    else
                    {
                        rig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                        if (rig.mainSkin.material.name == "infected (Instance)" || rig.mainSkin.material.name == "it")
                        {
                            rig.mainSkin.material.color = new Color32(237, 138, 0, 255);
                        }
                        else
                        {
                            rig.mainSkin.material.color = rig.playerColor;
                        }
                    }
                }
            }
            else
            {
                foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren<VRRig>())
                {
                    rig.mainSkin.material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    if (rig.mainSkin.material.name == "infected (Instance)" || rig.mainSkin.material.name == "it")
                    {
                        rig.mainSkin.material.color = Color.white;
                    }
                    else
                    {
                        rig.mainSkin.material.color = rig.playerColor;
                    }
                }
            }
        }

        public void Cleanup() { }
    }
}
