using System;
using System.Collections.Generic;
using TenacityLib;
using TenacityMain.ModSystem;
using UnityEngine;

namespace TenacityMain.TenacityMods.Settings
{
    public class NotificationSettings : MonoBehaviour, ITenacityModule
    {
        public string Name => "Notifications";
        public string Description => "";
        public string Tab => "Settings Tab";
        
        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "AntiCheat Notif",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Self",
                        "All"
                    },
                    SelectedDropdown = "Self"
                },
                new TenacityOption()
                {
                    Name = "ALOR Notif",
                    OptionType = TenacityOption.TenacityOptionType.Toggle,
                    Enabled = true,
                }
            };
        }

        public void Start()
        {
            Enabled = true;
        }

        public void Update()
        {
            TenacityModSystem.AlorNotifications = Options[1].Enabled;
            TenacityModSystem.AntiCheatNotifType = Options[0].SelectedDropdown;
        }

        public void Cleanup()
        {
            Enabled = true;
        }
    }
}