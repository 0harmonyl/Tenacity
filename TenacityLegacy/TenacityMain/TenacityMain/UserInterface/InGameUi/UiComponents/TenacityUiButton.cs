using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TenacityMain.UserInterface.InGameUi.UiComponents
{
    internal class TenacityUiButton : MonoBehaviour
    {
        public TabSwitcher buttonPress;
        string newName;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != GameObject.Find("TenacityPlayerUi").GetComponent<InGameUi>().rightHandCube) return;

            if (gameObject == GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/Left Side/Scroll View/Viewport/Content/TabButtonContainer/Blatant"))
                newName = "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect/Viewport/Blatant Tab";
            if (gameObject == GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/Left Side/Scroll View/Viewport/Content/TabButtonContainer/Misc"))
                newName = "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect/Viewport/Misc Tab";
            if (gameObject == GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/Left Side/Scroll View/Viewport/Content/TabButtonContainer/Troll"))
                newName = "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect/Viewport/Troll Tab";
            if (gameObject == GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/Left Side/Scroll View/Viewport/Content/TabButtonContainer/Safety"))
                newName = "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect/Viewport/Safety Tab";
            if (gameObject == GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/Left Side/Scroll View/Viewport/Content/TabButtonContainer/Visuals"))
                newName = "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect/Viewport/TenacityInGameUiVisualsTab";
            if (gameObject == GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/Left Side/Scroll View/Viewport/Content/TabButtonContainer/Settings"))
                newName = "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect/Viewport/Settings Tab";
            if (gameObject == GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/Left Side/Scroll View/Viewport/Content/TabButtonContainer/Configs"))
                newName = "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect/Viewport/Configs Tab";
            if (gameObject == GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/Left Side/Scroll View/Viewport/Content/TabButtonContainer/Custom"))
                newName = "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect/Viewport/Custom Tab";

            buttonPress.SwitchTab(GameObject.Find(newName), GetComponent<Image>());
        }
    }
}
