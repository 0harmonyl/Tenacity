using TenacityLib;
using UnityEngine;
using UnityEngine.UI;

namespace TenacityMain.UserInterface.InGameUi
{
    internal class UiInteractor : MonoBehaviour
    {
        public LayerMask mask = 1;
        string scrollname1 = "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect";
        string scrollname2 = "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/Left Side/Scroll View";

        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, Mathf.Infinity, mask))
            {
                GameObject.Find("TenacityPlayerUi").GetComponent<InGameUi>().rightHandCube.transform.position = hit.point;
            }

            Vector2 axis = ControllerInputManager.rightAxis2D;
            if(axis.y < -0.2)
            {
                GameObject.Find(scrollname1).GetComponent<ScrollRect>().verticalNormalizedPosition -= GameObject.Find(scrollname1).GetComponent<ScrollRect>().content.gameObject.GetComponent<RectTransform>().sizeDelta.y / 25000;
            }

            if (axis.y > 0.2)
            {
                GameObject.Find(scrollname1).GetComponent<ScrollRect>().verticalNormalizedPosition += GameObject.Find(scrollname1).GetComponent<ScrollRect>().content.gameObject.GetComponent<RectTransform>().sizeDelta.y / 25000;
            }

            Vector2 axis2 = ControllerInputManager.leftAxis2D;
            if (axis2.y < -0.2)
            {
                GameObject.Find(scrollname2).GetComponent<ScrollRect>().verticalNormalizedPosition -= GameObject.Find(scrollname2).GetComponent<ScrollRect>().content.gameObject.GetComponent<RectTransform>().sizeDelta.y / 25000;
            }

            if (axis2.y > 0.2)
            {
                GameObject.Find(scrollname2).GetComponent<ScrollRect>().verticalNormalizedPosition += GameObject.Find(scrollname2).GetComponent<ScrollRect>().content.gameObject.GetComponent<RectTransform>().sizeDelta.y / 25000;
            }
        }
    }
}
