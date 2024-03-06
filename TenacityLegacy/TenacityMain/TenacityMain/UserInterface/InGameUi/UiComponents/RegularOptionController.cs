using System;
using TenacityLib;
using UnityEngine;
using UnityEngine.UI;

namespace TenacityMain.UserInterface.InGameUi.UiComponents
{
    internal class RegularOptionController : MonoBehaviour
    {
        public ITenacityModule Module;
        public int OptionIndex;

        void Start()
        {
            UpdateState();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != GameObject.Find("TenacityPlayerUi").GetComponent<InGameUi>().rightHandCube) return;

            ITenacityModule m = (ITenacityModule)GorillaTagger.Instance.GetComponent(Module.GetType());
            m.Options[OptionIndex].Enabled = !m.Options[OptionIndex].Enabled;

            UpdateState();
        }

        public void UpdateState()
        {
            ITenacityModule m = (ITenacityModule)GorillaTagger.Instance.GetComponent(Module.GetType());
            if (m.Options[OptionIndex].Enabled) gameObject.GetComponent<RawImage>().color = new Color32(45, 45, 45, 255);
            if (!m.Options[OptionIndex].Enabled) gameObject.GetComponent<RawImage>().color = new Color32(35, 35, 35, 255);
        }
    }
}
