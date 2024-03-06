using System.Collections.Generic;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    internal class ExcelFly : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Excel Fly"; }
        }

        public string Description 
        { 
            get { return "Basic ExcelFly"; }
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
                new TenacityOption()
                {
                    Name = "SpeedDivider",
                    DropdownOptions = new List<string>
                    {
                        "1.5",
                        "2.0",
                        "2.5",
                        "3.0",
                        "3.5",
                    },
                    SelectedDropdown = "2.0",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown
                }
            };
        }

        public void Start() { }

        public void Update()
        {
            if(!Enabled) return;

            string[] SplitString = Options[0].SelectedDropdown.Split(".");
            int speed = int.Parse(SplitString[0]) + int.Parse(SplitString[1]) / 10;

            if (ControllerInputManager.rightTrigger)
            {
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity += GorillaTagger.Instance.rightHandTransform.right / speed;
            }

            if (ControllerInputManager.leftTrigger)
            {
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity += -GorillaTagger.Instance.leftHandTransform.right / speed;
            }
        }

        public void Cleanup() { }
    }
}
