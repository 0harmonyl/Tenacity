using System;
using System.Collections.Generic;
using TenacityLib;
using TenacityMain.Effects;
using TenacityMain.ModSystem;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    internal class TriggerFly : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Trigger Fly"; }
        }

        public string Description
        {
            get { return "Basic TFly"; }
        }

        public string Tab
        {
            get { return "Blatant Tab"; }
        }

        public bool Enabled { get; set; }
        public List<TenacityOption> Options { get; set; }
        bool nonce;
        float yPos;
        float speed;

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Speed",
                    DropdownOptions = new List<string>
                    {
                        "0.3",
                        "0.45",
                        "0.6"
                    },
                    SelectedDropdown = "0.45",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown
                },
                new TenacityOption()
                {
                    Name = "Lock-Y",
                    Enabled = false,
                    OptionType = TenacityOption.TenacityOptionType.Toggle
                },
                new TenacityOption()
                {
                    Name = "Visual Color",
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

        GameObject yLockedSquare = null;
        Material squareMaterial;
        Vector3 yOffset;

        public void Update()
        {
            if (!Enabled) return;

            if (Options[0].SelectedDropdown == "0.45")
                speed = 0.45f;
            if (Options[0].SelectedDropdown == "0.3")
                speed = 0.3f;
            if (Options[0].SelectedDropdown == "0.6")
                speed = 0.6f;

            if (Options[1].Enabled)
            {
                try
                {
                    if (!nonce)
                    {
                        yPos = GorillaTagger.Instance.transform.position.y;
                        squareMaterial = Plugin.MainAssetBundle.LoadAsset<Material>("TriggerFlyYLock.mat");
                        yOffset = new Vector3(0, 0.25f, 0);
                        if (yLockedSquare == null)
                        {
                            yLockedSquare = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            yLockedSquare.transform.localScale = new Vector3(1.25f, 0.01f, 1.25f);
                            yLockedSquare.GetComponent<Renderer>().material = squareMaterial;
                            try
                            {
                                Destroy(yLockedSquare.GetComponent<Collider>());
                                Destroy(yLockedSquare.GetComponent<Rigidbody>());
                            }
                            catch (Exception e)
                            {
                                Debug.Log(e.StackTrace);
                                throw e;
                            }
                        }
                        nonce = true;
                    }

                    Color newColor = Color.white;

                    if (Options[2].SelectedDropdown == "Tenacity")
                        newColor = TenacityColorManager.TenacityColor;
                    if (Options[2].SelectedDropdown == "Dark Red")
                        newColor = new Color32(145, 15, 0, 255);
                    if (Options[2].SelectedDropdown == "Red")
                        newColor = Color.red;
                    if (Options[2].SelectedDropdown == "Orange")
                        newColor = new Color32(255, 190, 11, 255);
                    if (Options[2].SelectedDropdown == "Mint Green")
                        newColor = new Color32(128, 255, 114, 255);
                    if (Options[2].SelectedDropdown == "Green")
                        newColor = new Color32(53, 191, 29, 255);
                    if (Options[2].SelectedDropdown == "Blue")
                        newColor = new Color32(32, 164, 243, 255);
                    if (Options[2].SelectedDropdown == "Pink")
                        newColor = new Color32(255, 159, 243, 255);
                    if (Options[2].SelectedDropdown == "Purple")
                        newColor = new Color32(98, 71, 170, 255);

                    newColor = new Color(newColor.r, newColor.g, newColor.b, 0.5f);

                    squareMaterial.color = newColor;

                    if (ControllerInputManager.leftTrigger)
                    {
                        GorillaLocomotion.Player.Instance.transform.position += GameObject.Find("Main Camera").transform.forward * speed;
                        GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
                        GorillaLocomotion.Player.Instance.transform.position = new Vector3(GorillaLocomotion.Player.Instance.transform.position.x, yPos, GorillaLocomotion.Player.Instance.transform.position.z);
                        yLockedSquare.transform.position = GameObject.Find("Main Camera").transform.position + yOffset;
                    }
                    else
                    {
                        nonce = false;
                        if (yLockedSquare != null)
                        {
                            Destroy(yLockedSquare);
                            yLockedSquare = null;
                        }
                    }
                } catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
            }
            else
            {
                if(ControllerInputManager.leftTrigger)
                {
                    GorillaLocomotion.Player.Instance.transform.position += GameObject.Find("Main Camera").transform.forward * speed;
                    GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
                }
            }
        }

        public void Cleanup() 
        {
            nonce = false;
        }
    }
}
