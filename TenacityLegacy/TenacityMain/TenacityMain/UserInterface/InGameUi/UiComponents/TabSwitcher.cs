using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    GameObject currentlyPressed;
    GameObject currentTab;

    private void Start()
    {
        currentlyPressed = GameObject.Find("Blatant");
        currentTab = GameObject.Find("Blatant Tab");
    }

    public void ButtonPressed(Image image)
    {
        if (currentlyPressed != image.gameObject)
        {
            currentlyPressed.GetComponent<Image>().color = new Color32(22, 22, 22, 0);
            currentlyPressed = image.gameObject;
        }

        if (image.color.a == 0)
        {
            image.color = new Color32(22, 22, 22, 255);
            return;
        }
        else if (image.color.a == 255)
        {
            image.color = new Color32(22, 22, 22, 0);
        }
    }

    public void SwitchTab(GameObject Tab, Image image)
    {
        if (Tab == null) return;
        if (Tab == currentTab) return;
        if (Tab != currentTab)
        {
            currentTab.SetActive(false);
            currentTab = Tab;
            Tab.SetActive(true);
            GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect").GetComponent<ScrollRect>().content = Tab.GetComponent<RectTransform>();
            ButtonPressed(image);
        }
    }
}