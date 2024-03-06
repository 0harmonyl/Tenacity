using Photon.Pun;
using System.Collections.Generic;
using System.Threading.Tasks;
using TenacityLib;
using TenacityMain.Effects;
using UnityEngine;

namespace TenacityMain.TenacityMods.Visuals
{
    internal class Tracers : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Tracers"; }
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

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Custom Color",
                    Enabled = false,
                    OptionType = TenacityOption.TenacityOptionType.Toggle
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

        public async void Update()
        {
            if(Enabled)
            {
                foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren<VRRig>())
                {
                    LineRenderer lr;
                    if (rig.gameObject.GetComponent<LineRenderer>() == null)
                    {
                        lr = rig.gameObject.AddComponent<LineRenderer>();
                        lr.enabled = true;
                        lr.useWorldSpace = true;
                        lr.startWidth = 0.03f;
                        lr.endWidth = 0.02f;
                        lr.numCornerVertices = 10;
                        lr.numCapVertices = 10;
                        lr.material.shader = Shader.Find("GUI/Text Shader");
                    }
                    else
                    {
                        lr = rig.gameObject.GetComponent<LineRenderer>();
                        lr.SetPosition(0, rig.transform.position);
                        Vector3 pos = GorillaTagger.Instance.bodyCollider.transform.position;
                        pos.y -= 0.2f;
                        lr.SetPosition(1, pos);

                        if (!Options[0].Enabled)
                        {
                            lr.startColor = rig.mainSkin.material.color;
                            lr.endColor = rig.mainSkin.material.color;
                        }
                        else
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

                            lr.startColor = newColor;
                            lr.endColor = newColor;
                        }
                    }
                }

                await Task.Delay(35);
            }
            else
            {
                foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren<VRRig>())
                {
                    Destroy(rig.GetComponent<LineRenderer>());
                }
            }
        }

        public void Cleanup() { }
    }
}
