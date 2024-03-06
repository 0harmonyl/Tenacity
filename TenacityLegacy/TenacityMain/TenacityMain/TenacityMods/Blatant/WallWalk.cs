using System.Collections.Generic;
using TenacityLib;
using UnityEngine;
using static TenacityLib.TenacityOption;

namespace TenacityMain.TenacityMods.Blatant
{
    internal class WallWalk : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "Wall Walk"; }
        }

        public string Description 
        { 
            get { return ""; }
        }

        public string Tab
        {
            get { return "Blatant Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        LayerMask layers;

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Power",
                    OptionType = TenacityOptionType.Dropdown,
                    DropdownOptions = new List<string>
                    {
                        "7.4",
                        "7.8",
                        "8.8",
                        "9.8"
                    },
                    SelectedDropdown = "7.4",
                }
            };
        }

        public void Start() { }

        public void Update()
        {
            if(!Enabled) return;

            layers = int.MaxValue;
            bool grip = ControllerInputManager.leftGrip;

            if (Options[0].SelectedDropdown == "7.4")
            {
                if (grip)
                {
                    WallWalkV1(1f, 7.4f);
                }
                else
                {
                    GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;
                }
            }

            if (Options[0].SelectedDropdown == "7.8")
            {
                if (grip)
                {
                    WallWalkV1(1f, 7.8f);
                }
                else
                {
                    GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;
                }
            }

            if (Options[0].SelectedDropdown == "8.8")
            {
                if (grip)
                {
                    WallWalkV1(1f, 8.8f);
                }
                else
                {
                    GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;
                }
            }

            if (Options[0].SelectedDropdown == "9.8")
            {
                if (grip)
                {
                    WallWalkV1(1f, 9.8f);
                }
                else
                {
                    GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;
                }
            }
        }

        void WallWalkV1(float distance, float power)
        {
            float dist2;
            Vector3 normal2;
            Vector3 vel2;
            float maxD2 = distance;

            RaycastHit raycastHit3;
            Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, -GorillaTagger.Instance.rightHandTransform.right, out raycastHit3, 4, layers);
            RaycastHit raycastHit4;
            Physics.Raycast(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.right, out raycastHit4, 4, layers);
            if (raycastHit4.distance > raycastHit3.distance)
            {
                normal2 = raycastHit3.normal;
                dist2 = raycastHit3.distance;
            }
            else
            {
                normal2 = raycastHit4.normal;
                dist2 = raycastHit4.distance;
            }
            if (dist2 < maxD2)
            {
                vel2 = normal2 * (power * Time.deltaTime);
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity -= vel2;
            }
            else
            {
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;
            }
        }

        public void Cleanup() { }
    }
}
