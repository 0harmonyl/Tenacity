using TenacityLib;
using TenacityMain.ModSystem;
using UnityEngine;
using UnityEngine.UI;

namespace TenacityMain.UserInterface.InGameUi.UiComponents
{
    internal class TenacityModuleController : MonoBehaviour
    {
        public ITenacityModule Module;

        void Start()
        {
            UpdateState();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != GameObject.Find("TenacityPlayerUi").GetComponent<InGameUi>().rightHandCube) return;

            ITenacityModule m = (ITenacityModule)GorillaTagger.Instance.GetComponent(Module.GetType());
            m.Enabled = !m.Enabled;

            UpdateState();
        }

        public void UpdateState()
        {
            if (Module.Enabled)
            {
                GetComponent<RawImage>().color = new Color32(49, 49, 49, 255);
                TenacityModSystem.EnableModule(Module);
            }
            if (!Module.Enabled)
            {
                GetComponent<RawImage>().color = new Color32(39, 39, 39, 255);
                TenacityModSystem.DisableModule(Module);
            }
        }
    }
}
