using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    internal class Speedboost : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Speedboost"; }
        }

        public string Description
        {
            get { return "Basic Bitch Boost"; }
        }

        public string Tab
        {
            get { return "Blatant Tab"; }
        }

        public bool Enabled { get; set; }
        public List<TenacityOption> Options { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption
                {
                    Name = "Speed",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "7.3",
                        "7.5",
                        "8.0",
                        "8.5",
                        "9.0",
                        "9.5"
                    },
                    SelectedDropdown = "7.5"
                }
            };
        }

        public void Start() { }

        void Update()
        {
            if(!Enabled) return;

            string[] SplitString = Options[0].SelectedDropdown.Split(".");
            int speed = int.Parse(SplitString[0]) + int.Parse(SplitString[1]) / 10;

            GorillaLocomotion.Player.Instance.maxJumpSpeed = speed;
        }

        public void Cleanup() { }
    }
}
