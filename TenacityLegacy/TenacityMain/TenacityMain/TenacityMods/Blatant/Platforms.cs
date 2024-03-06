using System.Collections.Generic;
using TenacityLib;
using TenacityMain.Effects;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    internal class Platforms : MonoBehaviour, ITenacityModule
    {   
        public string Name
        {
            get { return "Platforms"; }
        }

        public string Description 
        {
            get { return "Regular Platforms"; } 
        }

        public string Tab
        {
            get { return "Blatant Tab";  }
        }

        public bool Enabled { get; set; }
        public List<TenacityOption> Options { get; set; }

        Material PlatformMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        Vector3 PlatformScale;
        Vector3 PlatformOffset;

        GameObject LeftPlat, RightPlat;
        bool LeftPlatToggled, RightPlatToggled, RendererEnabled;

        Transform LeftHand, RightHand;

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Sticky",
                    Enabled = false,
                    OptionType = TenacityOption.TenacityOptionType.Toggle
                },
                new TenacityOption()
                {
                    Name = "Invisible",
                    Enabled = false,
                    OptionType = TenacityOption.TenacityOptionType.Toggle
                },
                new TenacityOption()
                {
                    Name = "Color",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Red",
                        "Blue",
                        "White",
                        "Black",
                        "Purple",
                        "Tenacity"
                    },
                    SelectedDropdown = "Tenacity"
                }
            };
        }

        public void Start() { }

        void Update()
        {
            if (!Enabled) return;

            if (Options[1].Enabled)
            {
                RendererEnabled = false;
            }
            else
            {
                PlatformMaterial.color = new Color32(32, 32, 32, 255);
                RendererEnabled = true;
            }

            if (Options[0].Enabled)
            {
                PlatformScale = new Vector3(0.2f, 0.2f, 0.2f);
                PlatformOffset = new Vector3(0, 0, 0);
            }
            else
            {
                PlatformScale = new Vector3(0.4f, 0.05f, 0.4f);
                PlatformOffset = new Vector3(0f, -0.075f, 0f);
            }

            Color newColor = Color.white;
            if (Options[2].SelectedDropdown == "Red")
                newColor = Color.red;
            if (Options[2].SelectedDropdown == "Blue")
                newColor = Color.cyan;
            if (Options[2].SelectedDropdown == "White")
                newColor = Color.white;
            if (Options[2].SelectedDropdown == "Black")
                newColor = Color.black;
            if (Options[2].SelectedDropdown == "Purple")
                newColor = new Color(0.4f, 0, 0.8f);
            if (Options[2].SelectedDropdown == "Tenacity")
                newColor = TenacityColorManager.TenacityColor;

            PlatformMaterial.color = newColor;

            LeftHand = GorillaLocomotion.Player.Instance.leftControllerTransform;
            RightHand = GorillaLocomotion.Player.Instance.rightControllerTransform;

            if(ControllerInputManager.leftGrip)
            {
                if(!LeftPlatToggled)
                {
                    LeftPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    LeftPlat.GetComponent<Renderer>().material = PlatformMaterial;
                    LeftPlat.GetComponent<Renderer>().enabled = RendererEnabled;
                    LeftPlat.transform.localScale = PlatformScale;
                    Vector3 newPosition = new Vector3(LeftHand.position.x + PlatformOffset.x, LeftHand.position.y + PlatformOffset.y, LeftHand.position.z + PlatformOffset.z);
                    LeftPlat.transform.position = newPosition;
                    LeftPlatToggled = true;
                }
            }
            else
            {
                DestroyPlat("left");
            }

            if (ControllerInputManager.rightGrip)
            {
                if (!RightPlatToggled)
                {
                    RightPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    RightPlat.GetComponent<Renderer>().material = PlatformMaterial;
                    RightPlat.GetComponent<Renderer>().enabled = RendererEnabled;
                    RightPlat.transform.localScale = PlatformScale;
                    Vector3 newPosition = new Vector3(RightHand.position.x + PlatformOffset.x, RightHand.position.y + PlatformOffset.y, RightHand.position.z + PlatformOffset.z);
                    RightPlat.transform.position = newPosition;
                    RightPlatToggled = true;
                }
            }
            else if (!ControllerInputManager.rightGrip)
            {
                DestroyPlat("right");
            }
        }

        void DestroyPlat(string leftright)
        {
            if (leftright == "left")
            {
                Destroy(LeftPlat);
                LeftPlatToggled = false;
            }
            if (leftright == "right")
            {
                Destroy(RightPlat);
                RightPlatToggled = false;
            }
        }

        public void Cleanup()
        {
            DestroyPlat("left");
            DestroyPlat("right");
        }
    }
}
