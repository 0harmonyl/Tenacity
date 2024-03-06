using System;
using System.Collections.Generic;
using TenacityLib;
using TenacityMain.ModSystem;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;

namespace TenacityMain.TenacityMods.Configs
{
    internal class SaveConfig : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Save Config"; }
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
        private bool AvoidNext = true;

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Saved As: ",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        ""
                    },
                    SelectedDropdown = ""
                }
            };
        }

        public void Start() 
        { 
            if(AvoidNext)
            {
                AvoidNext = false;
                return;
            }

            Config newConfig = new Config();
            newConfig.Name = "config" + GenerateRandomString(6);
            List<ITenacityModule> Modules = new List<ITenacityModule>();

            foreach(Type t in TenacityModSystem.RegisteredModules)
            {
                ITenacityModule m = (ITenacityModule) GorillaTagger.Instance.GetComponent(t);

                if (m == null) return;

                Modules.Add(m);
            }

            string jsonString = string.Empty;

            jsonString += newConfig.Name + "\n";

            foreach(ITenacityModule m in Modules) 
            { 
                jsonString += "Module:" + m.Name + ":" + m.Enabled.ToString() + "\n";
                
                if(m.Options.Count > 0)
                {
                    foreach(TenacityOption option in m.Options)
                    {
                        if(option.OptionType == TenacityOption.TenacityOptionType.Toggle)
                        {
                            jsonString += "Option:" + m.Name + ":" + m.Options.IndexOf(option) + ":" + option.Enabled.ToString() + ":Toggle" + "\n";
                                        //Trigger Fly:1:true             string.Split(":")[2]
                        }

                        if(option.OptionType == TenacityOption.TenacityOptionType.Dropdown)
                        {
                            jsonString += "Option:" + m.Name + ":" + m.Options.IndexOf(option) + ":" + option.SelectedDropdown + ":Dropdown" + "\n";
                                        //Speedboost:0:7.5               string.Split(":")[2]
                        }
                    }
                }
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + @"\BepInEx\tenacity\configs\" + newConfig.Name + ".tenacityconfig", jsonString);

            Options[0].SelectedDropdown = newConfig.Name;

            AvoidNext = true;

            TenacityModSystem.UpdateAllControllerStates();

            Enabled = false;
        }

        public void Update()
        {
            if (!Enabled) return;
        }

        public void Cleanup() { }

        string GenerateRandomString(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int charsLength = chars.Length;
            string result = string.Empty;
            for(int i = 0; i < length; i++)
            {
                result += chars[(int)Mathf.Floor(Random.Range(0, charsLength))];
            }
            return result;
        }
    }
}
