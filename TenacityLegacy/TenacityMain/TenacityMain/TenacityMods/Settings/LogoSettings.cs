using System.Collections.Generic;
using TenacityLib;
using TenacityMain.ModSystem;
using UnityEngine;

namespace TenacityMain.TenacityMods.Settings
{
    internal class LogoSettings : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Logo Settings"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public string Tab
        {
            get { return "Settings Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Text Logo?",
                    OptionType = TenacityOption.TenacityOptionType.Toggle,
                    Enabled = false
                },
                new TenacityOption()
                {
                    Name = "Text Color",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Default",
                        "Dark Red",
                        "Red",
                        "Orange",
                        "Mint Green",
                        "Green",
                        "Blue",
                        "Pink",
                        "Purple",
                    }
                }
            };
        }

        public void Start() { }

        public void Update()
        {
            if (Enabled)
            {
                if (Options[0].Enabled)
                {
                    TenacityModSystem.TextLogo = true;
                }
                else
                {
                    TenacityModSystem.TextLogo = false;
                }

                if (Options[1].SelectedDropdown == "Default")
                    TenacityModSystem.TextLogoColor = Color.white;
                if (Options[1].SelectedDropdown == "Dark Red")
                    TenacityModSystem.TextLogoColor = new Color32(145, 15, 0, 255);
                if (Options[1].SelectedDropdown == "Red")
                    TenacityModSystem.TextLogoColor = Color.red;
                if (Options[1].SelectedDropdown == "Orange")
                    TenacityModSystem.TextLogoColor = new Color32(255, 190, 11, 255);
                if (Options[1].SelectedDropdown == "Mint Green")
                    TenacityModSystem.TextLogoColor = new Color32(128, 255, 114, 255);
                if (Options[1].SelectedDropdown == "Green")
                    TenacityModSystem.TextLogoColor = new Color32(53, 191, 29, 255);
                if (Options[1].SelectedDropdown == "Blue")
                    TenacityModSystem.TextLogoColor = new Color32(32, 164, 243, 255);
                if (Options[1].SelectedDropdown == "Pink")
                    TenacityModSystem.TextLogoColor = new Color32(255, 159, 243, 255);
                if (Options[1].SelectedDropdown == "Purple")
                    TenacityModSystem.TextLogoColor = new Color32(98, 71, 170, 255);
            } 
            else
            {
                TenacityModSystem.TextLogo = false;
            }
        }

        public void Cleanup() { }
    }
}
