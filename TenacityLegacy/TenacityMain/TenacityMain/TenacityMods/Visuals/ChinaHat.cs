using Photon.Pun;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TenacityLib;
using TenacityMain.Effects;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace TenacityMain.TenacityMods.Visuals
{
    public class ChinaHat : MonoBehaviour, ITenacityModule
    { 
        public string Name
        {
            get { return "China Hat"; }
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

        public Color color;

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Mode",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Self",
                        "Others"
                    },
                    SelectedDropdown = "Others",
                },
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

        public void Update()
        {
            try
            {
                Color newColor = new Color();

                if (Options[1].SelectedDropdown == "Tenacity")
                    newColor = TenacityColorManager.TenacityColor;
                if (Options[1].SelectedDropdown == "Dark Red")
                    newColor = new Color32(145, 15, 0, 255);
                if (Options[1].SelectedDropdown == "Red")
                    newColor = Color.red;
                if (Options[1].SelectedDropdown == "Orange")
                    newColor = new Color32(255, 190, 11, 255);
                if (Options[1].SelectedDropdown == "Mint Green")
                    newColor = new Color32(128, 255, 114, 255);
                if (Options[1].SelectedDropdown == "Green")
                    newColor = new Color32(53, 191, 29, 255);
                if (Options[1].SelectedDropdown == "Blue")
                    newColor = new Color32(32, 164, 243, 255);
                if (Options[1].SelectedDropdown == "Pink")
                    newColor = new Color32(255, 159, 243, 255);
                if (Options[1].SelectedDropdown == "Purple")
                    newColor = new Color32(98, 71, 170, 255);

                color = newColor;

                if (Enabled && Options[0].SelectedDropdown == "Others")
                {
                    foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren(typeof(VRRig)))
                    {
                        if(!rig.gameObject.activeSelf)
                        {
                            if (rig.GetComponent<VisibleChinaHat>() != null)
                            {
                                if (rig.GetComponent<VisibleChinaHat>().hat != null)
                                {
                                    Destroy(rig.GetComponent<VisibleChinaHat>().hat);
                                }
                                Destroy(rig.GetComponent<VisibleChinaHat>());
                            }
                        }

                        if (rig.GetComponent<VisibleChinaHat>() == null)
                        {
                            rig.gameObject.AddComponent<VisibleChinaHat>().hatManager = this;
                        }
                    }

                    if (GorillaTagger.Instance.GetComponent<VisibleChinaHat>() != null)
                    {
                        if (GorillaTagger.Instance.GetComponent<VisibleChinaHat>().hat != null)
                        {
                            Destroy(GorillaTagger.Instance.GetComponent<VisibleChinaHat>().hat);
                        }
                        Destroy(GorillaTagger.Instance.GetComponent<VisibleChinaHat>());
                    }
                }
                else if (Enabled && Options[0].SelectedDropdown == "Self")
                {
                    foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren(typeof(VRRig)))
                    {
                        if (rig.GetComponent<VisibleChinaHat>() != null)
                        {
                            if (rig.GetComponent<VisibleChinaHat>().hat != null)
                            {
                                Destroy(rig.GetComponent<VisibleChinaHat>().hat);
                            }
                            Destroy(rig.GetComponent<VisibleChinaHat>());
                        }
                    }

                    if (GorillaTagger.Instance.GetComponent<VisibleChinaHat>() == null)
                    {
                        GorillaTagger.Instance.gameObject.AddComponent<VisibleChinaHat>().hatManager = this;
                    }
                }
                else if (!Enabled)
                {
                    foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren(typeof(VRRig)))
                    {
                        if (!rig.gameObject.activeSelf)
                        {
                            if (rig.GetComponent<VisibleChinaHat>() != null)
                            {
                                if (rig.GetComponent<VisibleChinaHat>().hat != null)
                                {
                                    Destroy(rig.GetComponent<VisibleChinaHat>().hat);
                                }
                                Destroy(rig.GetComponent<VisibleChinaHat>());
                            }
                        }

                        if (rig.GetComponent<VisibleChinaHat>() != null)
                        {
                            if (rig.GetComponent<VisibleChinaHat>().hat != null)
                            {
                                Destroy(rig.GetComponent<VisibleChinaHat>().hat);
                            }
                            Destroy(rig.GetComponent<VisibleChinaHat>());
                        }
                    }

                    if (GorillaTagger.Instance.GetComponent<VisibleChinaHat>() != null)
                    {
                        if (GorillaTagger.Instance.GetComponent<VisibleChinaHat>().hat != null)
                        {
                            Destroy(GorillaTagger.Instance.GetComponent<VisibleChinaHat>().hat);
                        }
                        Destroy(GorillaTagger.Instance.GetComponent<VisibleChinaHat>());
                    }
                }
            } catch (Exception e)
            {
                Debug.Log(e.StackTrace);
            }
        }

        public void Cleanup() { }
    }

    public class VisibleChinaHat : MonoBehaviour
    {
        public GameObject hat;
        public ChinaHat hatManager;

        void Start()
        {
            hat = Instantiate(Plugin.MainAssetBundle.LoadAsset<GameObject>("ChinaHat.prefab"));
            hat.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        void Update()
        {
            hat.transform.Find("chinahat").GetComponent<MeshRenderer>().materials[0].color = hatManager.color;

            if (hatManager.Options[0].SelectedDropdown == "Others")
            {
                if(!PhotonNetwork.InRoom)
                {
                    Destroy(hat);
                    Destroy(this);
                }
                if (GetComponent<VRRig>() == null)
                {
                    if (hat == null) return;
                    Destroy(hat);
                    Destroy(this);
                    return;
                }
                hat.transform.position = GetComponent<VRRig>().headMesh.transform.position + new Vector3(0, 0.2f, 0);
            } 
            else 
            {
                hat.transform.position = GameObject.Find("Main Camera").transform.position + new Vector3(0, 0.2f, 0);
            }
        }
    }
}
