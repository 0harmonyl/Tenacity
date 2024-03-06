using System;
using System.Collections.Generic;
using TenacityLib;
using TenacityMain.Effects;
using TenacityMain.ModSystem;
using TenacityMain.UserInterface.OnScreenUi;
using TMPro;
using UnityEngine;

namespace TenacityMain.TenacityMods.Visuals
{
    internal class EnabledMods : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Enabled Mods"; }
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

        public TextMeshProUGUI text;
        private bool nonce;

        TMP_FontAsset Modern, Minecraft, Antipasto, Roman, MinecraftBold, Modern2;
        TMP_FontAsset CurrentAsset;

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Gradient",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Default",
                        "Red",
                        "Orange",
                        "Orange/Pink",
                        "Mint Green",
                        "Green",
                        "Blue",
                        "Pink",
                        "Purple",
                        "White/Gray",
                    },
                    SelectedDropdown = "Default"
                },
                new TenacityOption()
                {
                    Name = "Font",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Antipasto",
                        "Times New Roman",
                        "Minecraft",
                        "Minecraft Bold",
                        "Modern",
                        "Greycliff"
                    },
                    SelectedDropdown = "Antipasto"
                }
            };
            Enabled = true;
            Antipasto = Plugin.MainAssetBundle.LoadAsset<TMP_FontAsset>("Antipasto.asset");
            Modern = Plugin.MainAssetBundle.LoadAsset<TMP_FontAsset>("Modern.asset");
            Minecraft = Plugin.MainAssetBundle.LoadAsset<TMP_FontAsset>("Minecraft.asset");
            Roman = Plugin.MainAssetBundle.LoadAsset<TMP_FontAsset>("Roman.asset");
            MinecraftBold = Plugin.MainAssetBundle.LoadAsset<TMP_FontAsset>("MinecraftBold.asset");
            Modern2 = Plugin.MainAssetBundle.LoadAsset<TMP_FontAsset>("GreycliffCF.asset");
        }

        public void Start() 
        {
            ChangeFont();
        }

        public void Update()
        {
            ChangeFont();

            if (Options[0].SelectedDropdown == "Default")
            {
                TextMeshProGradientEffect.startColor = new Color32(245, 110, 213, 255);
                TextMeshProGradientEffect.middleColor = new Color32(0, 160, 230, 255);
            }

            if (Options[0].SelectedDropdown == "Red")
            {
                TextMeshProGradientEffect.startColor = new Color32(153, 0, 0, 255);
                TextMeshProGradientEffect.middleColor = new Color32(255, 0, 0, 255);
            }

            if (Options[0].SelectedDropdown == "Orange")
            {
                TextMeshProGradientEffect.startColor = new Color32(255, 190, 11, 255);
                TextMeshProGradientEffect.middleColor = new Color32(244, 43, 3, 255);
            }

            if (Options[0].SelectedDropdown == "Orange/Pink")
            {
                TextMeshProGradientEffect.startColor = new Color32(251, 130, 8, 255);
                TextMeshProGradientEffect.middleColor = new Color32(251, 184, 218, 255);
            }

            if (Options[0].SelectedDropdown == "Mint Green")
            {
                TextMeshProGradientEffect.startColor = new Color32(128, 255, 114, 255);
                TextMeshProGradientEffect.middleColor = new Color32(116, 225, 147, 255);
            }

            if (Options[0].SelectedDropdown == "Green")
            {
                TextMeshProGradientEffect.startColor = new Color32(53, 191, 29, 255);
                TextMeshProGradientEffect.middleColor = new Color32(26, 81, 46, 255);
            }

            if (Options[0].SelectedDropdown == "Blue")
            {
                TextMeshProGradientEffect.startColor = new Color32(32, 164, 243, 255);
                TextMeshProGradientEffect.middleColor = new Color32(24, 43, 58, 255);
            }

            if (Options[0].SelectedDropdown == "Pink")
            {
                TextMeshProGradientEffect.startColor = new Color32(255, 159, 243, 255);
                TextMeshProGradientEffect.middleColor = new Color32(255, 64, 129, 255);
            }

            if (Options[0].SelectedDropdown == "Purple")
            {
                TextMeshProGradientEffect.startColor = new Color32(98, 71, 170, 255);
                TextMeshProGradientEffect.middleColor = new Color32(165, 148, 249, 255);
            }

            if (Options[0].SelectedDropdown == "White/Gray")
            {
                TextMeshProGradientEffect.startColor = new Color32(211, 211, 211, 255);
                TextMeshProGradientEffect.middleColor = new Color32(45, 52, 54, 255);
            }

            try
            {
                if(!nonce)
                {
                    text = GameObject.Find("*VL8N6Alvr5cfE#kW$EPjBTGT!jWxStr").GetComponent<TextMeshProUGUI>();
                    nonce = true;
                }

                if (!Enabled)
                {
                    text.text = "";
                    return;
                }

                List<string> stringList = new List<string>();
                string newText = string.Empty;

                foreach(string s in TenacityModSystem.EnabledModules)
                {
                    stringList.Add(s);
                }

                stringList.Sort((a, b) => GetVisualSize(b).CompareTo(GetVisualSize(a)));

                foreach (string st in stringList)
                {
                    newText += st + "\n";
                }

                if (CurrentAsset == Roman)
                {
                    newText = newText.ToLower();
                }

                text.text = newText;
            }
            catch (Exception e)
            {
                Debug.Log(e.StackTrace);
            }
        }

        float GetVisualSize(string text)
        {
            TMP_Text tempText = new GameObject("TempText").AddComponent<TextMeshProUGUI>();
            tempText.font = CurrentAsset;
            tempText.text = text;

            Vector2 size = tempText.GetPreferredValues();

            Destroy(tempText.gameObject);

            return size.x;
        }

        public void Cleanup() { }
        
        void ChangeFont()
        {
            if (Options[1].SelectedDropdown == "Antipasto")
            {
                if (OnScreenUi.fontAsset == Antipasto) return;
                OnScreenUi.fontAsset = Antipasto;
            }
            if (Options[1].SelectedDropdown == "Times New Roman")
            {
                if (OnScreenUi.fontAsset == Roman) return;
                OnScreenUi.fontAsset = Roman;
            }
            if (Options[1].SelectedDropdown == "Minecraft")
            {
                if (OnScreenUi.fontAsset == Minecraft) return;
                OnScreenUi.fontAsset = Minecraft;
            }
            if (Options[1].SelectedDropdown == "Minecraft Bold")
            {
                if (OnScreenUi.fontAsset == MinecraftBold) return;
                OnScreenUi.fontAsset = MinecraftBold;
            }
            if (Options[1].SelectedDropdown == "Modern")
            {
                if (OnScreenUi.fontAsset == Modern) return;
                OnScreenUi.fontAsset = Modern;
            }
            if (Options[1].SelectedDropdown == "Greycliff")
            {
                if (OnScreenUi.fontAsset == Modern2) return;
                OnScreenUi.fontAsset = Modern2;
            }

            CurrentAsset = OnScreenUi.fontAsset;
        }
    }
}
