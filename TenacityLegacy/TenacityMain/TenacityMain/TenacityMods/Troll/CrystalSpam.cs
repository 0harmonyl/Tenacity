using System.Collections.Generic;
using System.Threading.Tasks;
using TenacityLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Troll
{
    internal class CrystalSpam : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Crystal Spammer"; }
        }

        public string Description
        {
            get { return "Basic AF"; }
        }

        public string Tab
        {
            get { return "Troll Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        private bool tappedAll, nonce;
        private List<GorillaCaveCrystal> caveObjects = new List<GorillaCaveCrystal>();
        private int cooldown;

        public void Setup() 
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Cooldown",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions = new List<string>
                    {
                        "1",
                        "2",
                        "3",
                        "4",
                        "5"
                    },
                    SelectedDropdown = "2"
                }
            };
        }

        public void Start() { }

        public void Update() 
        {
            if (!Enabled) return;

            if(!nonce)
            {
                foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
                {
                    if (obj.GetComponent<GorillaCaveCrystal>() != null)
                    {
                        caveObjects.Add(obj.GetComponent<GorillaCaveCrystal>());
                    }
                }

                nonce = true;
            }

            if (Options[0].SelectedDropdown == "1")
                cooldown = 1000;
            if (Options[0].SelectedDropdown == "2")
                cooldown = 2000;
            if (Options[0].SelectedDropdown == "3")
                cooldown = 3000;
            if (Options[0].SelectedDropdown == "4")
                cooldown = 4000;
            if (Options[0].SelectedDropdown == "5")
                cooldown = 5000;

            if (!tappedAll)
            {
                foreach (GorillaCaveCrystal crystal in caveObjects)
                {
                    if (crystal == null) return;
                    crystal.OnTap(0.1f, 0.5f);
                }

                tappedAll = true;
                ResetTimer();
            }
        }

        private async void ResetTimer()
        {
            await Task.Delay(cooldown);
            tappedAll = false;
        }

        public void Cleanup() { }
    }
}
