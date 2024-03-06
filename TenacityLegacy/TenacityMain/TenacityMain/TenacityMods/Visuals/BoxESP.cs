using System.Collections.Generic;
using TenacityLib;
using UnityEngine;
using Photon.Pun;
using TenacityMain.Effects;

namespace TenacityMain.TenacityMods.Visuals
{
    internal class BoxESP : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Box ESP"; }
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

        public static float scale;
        public static GameObject boxObject;

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
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
            if (!Enabled) return;

            if(PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren<VRRig>())
                {
                    if (rig.GetComponent<AddBox>())
                    {
                        if (!rig.gameObject.transform.Find("HollowBox123hgnas").gameObject.activeSelf || !rig.gameObject.transform.Find("HollowBox123hgnas").gameObject)
                        {
                            rig.gameObject.transform.Find("HollowBox123hgnas").gameObject.SetActive(true);
                        }
                    }
                    if (!rig.GetComponent<AddBox>())
                        rig.gameObject.AddComponent<AddBox>();
                    else
                    {
                        AddBox box = rig.GetComponent<AddBox>();

                        Camera main = GameObject.Find("Main Camera").GetComponent<Camera>();
                        Matrix4x4 projectionMatrix = main.projectionMatrix;
                        Vector3 rigPosition = rig.transform.position;
                        float boxDistanceFromCamera = Vector3.Distance(rigPosition, main.transform.position);

                        Matrix4x4 worldToCameraMatrix = main.worldToCameraMatrix;

                        Vector4 objectClipPosition = projectionMatrix * worldToCameraMatrix * new Vector4(rigPosition.x, rigPosition.y, rigPosition.z, 1);
                        objectClipPosition /= objectClipPosition.w;

                        scale = boxDistanceFromCamera / objectClipPosition.w;

                        float minimumScale = 1f;
                        float maximumScale = 4.5f;

                        scale = Mathf.Clamp(scale, minimumScale, maximumScale);
                        box.topSide.transform.localScale = new Vector3(scale / 40, scale / 40, scale / 40);

                        Color newColor = new Color();

                        if (Options[0].SelectedDropdown == "Tenacity")
                            newColor = TenacityColorManager.TenacityColor;
                        if (Options[0].SelectedDropdown == "Dark Red")
                            newColor = new Color32(145, 15, 0, 255);
                        if (Options[0].SelectedDropdown == "Red")
                            newColor = Color.red;
                        if (Options[0].SelectedDropdown == "Orange")
                            newColor = new Color32(255, 190, 11, 255);
                        if (Options[0].SelectedDropdown == "Mint Green")
                            newColor = new Color32(128, 255, 114, 255);
                        if (Options[0].SelectedDropdown == "Green")
                            newColor = new Color32(53, 191, 29, 255);
                        if (Options[0].SelectedDropdown == "Blue")
                            newColor = new Color32(32, 164, 243, 255);
                        if (Options[0].SelectedDropdown == "Pink")
                            newColor = new Color32(255, 159, 243, 255);
                        if (Options[0].SelectedDropdown == "Purple")
                            newColor = new Color32(98, 71, 170, 255);

                        newColor.a = 0.6f;

                        box.topSide.GetComponent<Renderer>().material.color = newColor;
                    }
                }
            }
            else
            {
                foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren<VRRig>())
                {
                    if (rig.GetComponent<AddBox>())
                    {
                        rig.gameObject.transform.Find("HollowBox123hgnas").gameObject.SetActive(false);
                    }
                }
            }
        }

        public void Cleanup() 
        {
            foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren<VRRig>())
            {
                if (rig.GetComponent<AddBox>())
                {
                    rig.gameObject.transform.Find("HollowBox123hgnas").gameObject.SetActive(false);
                }
            }
        }
    }

    public class Box : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.LookAt(GameObject.Find("Main Camera").transform.position);
        }
    }

    public class AddBox : MonoBehaviour
    {
        public GameObject topSide;
        private GameObject hollowBoxGO;

        private void Start()
        {
            hollowBoxGO = new GameObject("HollowBox123hgnas");
            hollowBoxGO.transform.SetParent(transform);

            BoxESP.boxObject = hollowBoxGO;

            topSide = GameObject.CreatePrimitive(PrimitiveType.Plane);

            topSide.transform.SetParent(hollowBoxGO.transform);
            topSide.transform.localRotation = Quaternion.Euler(90, 0, 0);
            topSide.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
            Destroy(topSide.GetComponent<MeshCollider>());

            hollowBoxGO.transform.localPosition = new Vector3(0, -0.1f, 0);
            hollowBoxGO.transform.localRotation = Quaternion.identity;

            hollowBoxGO.AddComponent<Box>();
        }
    }
}
