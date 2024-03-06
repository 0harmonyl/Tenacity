using System;
using System.Collections.Generic;
using TenacityLib;
using TenacityMain.ModSystem;
using UnityEngine;

namespace TenacityMain.TenacityMods.Configs
{
    internal class TenacityDefaultConfig : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Tenacity Default"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public string Tab
        {
            get { return "Configs Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }
        bool youreafuckingnonce;

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }

        public void Start() 
        {
            if(!youreafuckingnonce)
            {
                youreafuckingnonce = true;
                return;
            }

            foreach (Type type in TenacityModSystem.RegisteredModules)
            {
                ITenacityModule m = (ITenacityModule)GorillaTagger.Instance.GetComponent(type);

                if (m == null) return;

                if (m.Name == "China Hat")
                {
                    m.Enabled = true;
                    m.Options[0].SelectedDropdown = "Others";
                    m.Options[1].SelectedDropdown = "Tenacity";
                }
                else if (m.Name == "Chams")
                {
                    m.Enabled = true;
                    m.Options[0].Enabled = true;
                    m.Options[1].SelectedDropdown = "Tenacity";
                }
                else if (m.Name == "Hollow ESP")
                {
                    m.Enabled = true;
                    m.Options[0].SelectedDropdown = "Tenacity";
                }
                else if (m.Name == "Logo Settings")
                {
                    m.Enabled = true;
                    m.Options[0].Enabled = false;
                    m.Options[1].SelectedDropdown = "Pink";
                }
                else if (m.Name == "ALOR")
                {
                    m.Enabled = true;
                    m.Options[0].SelectedDropdown = "0.42";
                }
                else if (m.Name == "BMC")
                {
                    m.Enabled = true;
                    m.Options[0].SelectedDropdown = "Show Legal";
                }
                else if (m.Name == "Speedboost")
                {
                    m.Enabled = true;
                    m.Options[0].SelectedDropdown = "7.5";
                }
                else if (m.Name == "Trigger Fly")
                {
                    m.Enabled = false;
                    m.Options[0].Enabled = true;
                    m.Options[1].SelectedDropdown = "Tenacity";
                }
                else if (m.Name == "Tracers")
                {
                    m.Enabled = false;
                    m.Options[0].Enabled = true;
                    m.Options[1].SelectedDropdown = "Tenacity";
                }
                else if (m.Name == "Platforms")
                {
                    m.Enabled = false;
                    m.Options[0].SelectedDropdown = "Tenacity";
                } else if(m.Name == "Enabled Mods")
                {
                    m.Enabled = true;
                    m.Options[0].SelectedDropdown = "Default";
                    m.Options[1].SelectedDropdown = "Minecraft Bold";
                }
                else
                {
                    m.Enabled = false;
                }
            }

            TenacityModSystem.UpdateAllControllerStates();
        }

        public void Update()
        {
            if (!Enabled) return;
        }

        public void Cleanup() { }
    }
}
