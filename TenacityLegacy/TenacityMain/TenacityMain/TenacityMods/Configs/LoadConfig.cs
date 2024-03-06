using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TenacityLib;
using TenacityMain.ModSystem;
using UnityEngine;

namespace TenacityMain.TenacityMods.Configs
{
    internal class LoadConfig : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Load Config"; }
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
        private List<Config> Configs = new List<Config>();
        private bool AvoidNext = true;

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Config: ",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        ""
                    },
                    SelectedDropdown = "",
                }
            };
            SetOptions();
        }

        public void SetOptions()
        {
            string[] fileNames = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\BepInEx\tenacity\configs\");

            foreach (string file in fileNames)
            {
                string fileContent = File.ReadAllText(file);
                Debug.Log(fileContent);

                Config newConfig = new Config();

                using (var reader = new StringReader(fileContent))
                {
                    string name = reader.ReadLine();

                    newConfig.Name = name;

                    Debug.Log(name);
                }

                foreach (string line in new LineReader(() => new StringReader(fileContent)))
                {
                    if (line.StartsWith("Module:"))
                    {
                        string[] splitLine = line.Split(":");
                        string moduleName = splitLine[1];

                        string moduleEnabled = splitLine[2];

                        newConfig.Modules.Add(new List<string>
                        {
                            moduleName,
                            moduleEnabled,
                        });
                    }

                    if (line.StartsWith("Option:"))
                    {
                        string[] splitLine = line.Split(":");
                        string moduleName = splitLine[1];
                        string moduleIndex = splitLine[2];
                        string moduleValue = splitLine[3];
                        string moduleType = splitLine[4];

                        newConfig.ModuleOptions.Add(new List<string>
                        {
                            moduleName,
                            moduleIndex,
                            moduleValue,
                            moduleType
                        });
                    }
                }

                Debug.Log(newConfig.Modules);

                Configs.Add(newConfig);
            }

            List<string> DropdownOptions = new List<string>();

            foreach (Config c in Configs)
            {
                DropdownOptions.Add(c.Name);
            }

            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Config: ",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions = DropdownOptions,
                    SelectedDropdown = DropdownOptions[0],
                }
            };

            TenacityModSystem.UpdateAllControllerStates();
        }

        public void Start()
        {
            if(AvoidNext)
            {
                AvoidNext = false;
                return;
            }

            Config toLoad = null;
            foreach(Config c in Configs)
            {
                if(c.Name == Options[0].SelectedDropdown)
                {
                    toLoad = c;
                }
            }

            if (toLoad == null) return;

            Debug.Log(toLoad.Modules);
            foreach(List<string> modules in toLoad.Modules)
            {
                string moduleName = modules[0];
                string moduleValue = modules[1];
                bool moduleEnabled = false;

                if(moduleValue == "true")
                {
                    moduleEnabled = true;
                }
                else
                {
                    moduleEnabled = false;
                }

                foreach(ITenacityModule module in TenacityModSystem.RegisteredModules)
                {
                    if (module.Name == moduleName)
                    {
                        module.Enabled = moduleEnabled;
                    }
                }
            }

            foreach(List<string> OptionObject in toLoad.ModuleOptions)
            {
                string moduleName = OptionObject[0];
                string optionIndex = OptionObject[1];
                string optionValue = OptionObject[2];
                string optionType = OptionObject[3];
                bool enabled = false, useBool = false;

                if (optionType == "Toggle")
                {
                    useBool = true;
                    if(optionValue == "true")
                    {
                        enabled = true;
                    }
                    else
                    {
                        enabled = false;
                    }
                }

                if(optionType == "Dropdown")
                {
                    useBool = false;
                }

                if (useBool)
                {
                    foreach (ITenacityModule module in TenacityModSystem.RegisteredModules)
                    {
                        if (module.Name == moduleName)
                        {
                            module.Options[int.Parse(optionIndex)].Enabled = enabled;
                        }
                    }
                }
                else
                {
                    foreach (ITenacityModule module in TenacityModSystem.RegisteredModules)
                    {
                        if (module.Name == moduleName)
                        {
                            module.Options[int.Parse(optionIndex)].SelectedDropdown = optionValue;
                        }
                    }
                }
            }

            AvoidNext = true;
            TenacityModSystem.UpdateAllControllerStates();
            Enabled = false;
        }

        public void Update()
        {
            if (!Enabled) return;


        }

        public void Cleanup() { }
    }
}
