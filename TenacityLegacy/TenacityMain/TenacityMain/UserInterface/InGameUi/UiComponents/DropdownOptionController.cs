using System;
using TenacityLib;
using TMPro;
using UnityEngine;

namespace TenacityMain.UserInterface.InGameUi.UiComponents
{
    internal class DropdownOptionController : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public ITenacityModule Module;
        public int OptionIndex;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != GameObject.Find("TenacityPlayerUi").GetComponent<InGameUi>().rightHandCube) return;

            ITenacityModule m = (ITenacityModule)GorillaTagger.Instance.GetComponent(Module.GetType());

            int CurrentIndex = Array.IndexOf(m.Options[OptionIndex].DropdownOptions.ToArray(), m.Options[OptionIndex].SelectedDropdown);

            if (CurrentIndex == m.Options[OptionIndex].DropdownOptions.Count - 1)
            {
                m.Options[OptionIndex].SelectedDropdown = m.Options[OptionIndex].DropdownOptions[0];
            }
            else
            {
                m.Options[OptionIndex].SelectedDropdown = m.Options[OptionIndex].DropdownOptions[CurrentIndex + 1];
            }

            UpdateState();
        }

        public void UpdateState()
        {
            ITenacityModule m = (ITenacityModule)GorillaTagger.Instance.GetComponent(Module.GetType());
            text.text = m.Options[OptionIndex].SelectedDropdown + "   ";
        }
    }
}
