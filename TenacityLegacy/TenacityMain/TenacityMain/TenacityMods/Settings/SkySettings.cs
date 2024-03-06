using System;
using System.Collections.Generic;
using TenacityLib;
using TenacityMain.Effects;
using UnityEngine;

namespace TenacityMain.TenacityMods.Settings
{
    public class SkySettings : MonoBehaviour, ITenacityModule
    {
        public string Name => "Sky Settings";
        public string Description => "Settings for the sky";
        public string Tab => "Settings Tab";
        
        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
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
        
        public void Cleanup() { }

        ///////////////////////////////////////////////////////////
        
        private Renderer sky, sky2;
        private Material skyMaterial;
        private Color skyColor;
        private Material defaultMaterial;

        public void Update()
        {
            if (!Enabled) return;

            if (sky == null || sky2 == null)
            {
                sky = GameObject.Find("newsky (1)").GetComponent<Renderer>();
                sky2 = GameObject.Find("Standard Sky").GetComponent<Renderer>();
                defaultMaterial = sky.material;
            }

            if (skyMaterial == null)
            {
                skyMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            }
            
            if (Options[0].SelectedDropdown == "Tenacity")
                skyColor = TenacityColorManager.TenacityColor;
            if (Options[0].SelectedDropdown == "Dark Red")
                skyColor = new Color32(145, 15, 0, 255);
            if (Options[0].SelectedDropdown == "Red")
                skyColor = Color.red;
            if (Options[0].SelectedDropdown == "Orange")
                skyColor = new Color32(255, 190, 11, 255);
            if (Options[0].SelectedDropdown == "Mint Green")
                skyColor = new Color32(128, 255, 114, 255);
            if (Options[0].SelectedDropdown == "Green")
                skyColor = new Color32(53, 191, 29, 255);
            if (Options[0].SelectedDropdown == "Blue")
                skyColor = new Color32(32, 164, 243, 255);
            if (Options[0].SelectedDropdown == "Pink")
                skyColor = new Color32(255, 159, 243, 255);
            if (Options[0].SelectedDropdown == "Purple")
                skyColor = new Color32(98, 71, 170, 255);

            skyMaterial.color = skyColor;

            sky.material = skyMaterial;
            sky2.material = skyMaterial;
        }
    }
}