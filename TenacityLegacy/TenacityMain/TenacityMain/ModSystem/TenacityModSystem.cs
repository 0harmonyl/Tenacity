using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TenacityLib;
using TenacityMain.UserInterface.InGameUi;
using TenacityMain.UserInterface.InGameUi.UiComponents;
using TenacityMain.UserInterface.NotificationLib;
using TenacityMain.UserInterface.OnScreenUi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenacityMain.ModSystem
{
    internal class TenacityModSystem : MonoBehaviour
    {
        public static List<Type> RegisteredModules = new List<Type>();
        public static List<string> EnabledModules = new List<string>();
        private bool NextIsRightSide = true;
        public static TenacityModSystem instance;
        public static bool NoFingers;
        public static bool TextLogo;
        public static bool Projctiles = true;
        public static Color TextLogoColor = Color.white;
        public static bool AlorNotifications;
        public static string AntiCheatNotifType;

        public void RegisterModule(Type type) 
        {
            RegisteredModules.Add(type);
        }

        void Start()
        {
            instance = this;
            try
            {
                Assembly mainAssembly = Assembly.GetExecutingAssembly();

                List<Type> moduleTypes = mainAssembly.GetTypes()
                    .Where(type => type.IsClass && typeof(ITenacityModule).IsAssignableFrom(type))
                    .ToList();

                string ArrayString = "Module Array: ";
                foreach (Type type in moduleTypes)
                {
                    ArrayString += type.Name + ", ";
                }
                
                Debug.Log(ArrayString);
                
                foreach (Type type in moduleTypes)
                {
                    Debug.Log("Registering: " + type.Name);
                    RegisterModule(type);
                }

                foreach (Type type in RegisteredModules)
                {
                    Debug.Log("Adding " + type.Name + " as Component to Self");
                    ITenacityModule module = (ITenacityModule)GorillaTagger.Instance.gameObject.AddComponent(type);
                    if (module != null)
                    {
                        CreateModuleFromInterface(module); // Create Ui With Controller Components
                    }
                }

                UpdateAllControllerStates();
            } catch (Exception e)
            {
                Debug.Log(e.StackTrace);
            }
        }

        public static void EnableModule(ITenacityModule Module)
        {
            Module.Start();

            if (EnabledModules.Contains(Module.Name) || Module.Name == "Logo Settings") return;

            EnabledModules.Add(Module.Name);

            Notifications.SendNotification("<color=\"white\">" + Module.Name + " was <color=\"green\">enabled", 3f, NotificationType.ModuleEnabled);
        }

        public static void DisableModule(ITenacityModule Module)
        {
            Module.Cleanup();

            if(!EnabledModules.Contains(Module.Name)) return;

            EnabledModules.Remove(Module.Name);

            Notifications.SendNotification("<color=\"white\">" + Module.Name + " was <color=\"red\">disabled", 3f, NotificationType.ModuleDisabled);
        }

        public static void UpdateAllControllerStates()
        {
            foreach(TenacityModuleController c in Resources.FindObjectsOfTypeAll(typeof(TenacityModuleController)))
            {
                c.UpdateState();
            }

            foreach(RegularOptionController o in Resources.FindObjectsOfTypeAll<RegularOptionController>())
            {
                o.UpdateState();
            }

            foreach(DropdownOptionController d in Resources.FindObjectsOfTypeAll<DropdownOptionController>())
            {
                d.UpdateState();
            }
        }

        void CreateModuleFromInterface(ITenacityModule Module)
        {
            Module.Setup();

            GameObject Tab = GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/TenacityInGameUi(Clone)/TenacityRSScrollRect/Viewport/" + Module.Tab);

            GameObject RightSide = Tab.transform.Find("Right Side").gameObject;
            GameObject LeftSide = Tab.transform.Find("Left Side").gameObject;

            GameObject CurrentSide = RightSide;

            if (!NextIsRightSide)
            {
                CurrentSide = LeftSide;
                NextIsRightSide = true;
            } 
            else if (NextIsRightSide)
            {
                CurrentSide = RightSide;
                NextIsRightSide = false;
            }

            GameObject moduleObject = new GameObject();
            moduleObject.AddComponent<RectTransform>();
            moduleObject.GetComponent<RectTransform>().sizeDelta = new Vector2(655, Module.Options.Count * 100 + 100);
            moduleObject.AddComponent<VerticalLayoutGroup>();
            moduleObject.GetComponent<VerticalLayoutGroup>().childControlWidth = true;
            moduleObject.transform.SetParent(CurrentSide.transform, false);
            moduleObject.name = Module.Name;

            GameObject top = new GameObject();
            top.name = "Top";
            top.transform.SetParent(moduleObject.transform, false);
            top.AddComponent<RectTransform>();
            top.GetComponent<RectTransform>().sizeDelta = new Vector2(top.GetComponent<RectTransform>().sizeDelta.x, 100);
            top.AddComponent<HorizontalLayoutGroup>();
            top.AddComponent<RawImage>();
            top.GetComponent<RawImage>().color = new Color32(39, 39, 39, 255);
            top.AddComponent<BoxCollider>().size = new Vector3(655, 100, 1);
            top.GetComponent<BoxCollider>().isTrigger = true;

            GameObject moduleName = new GameObject();
            moduleName.transform.SetParent(top.transform, false);
            moduleName.AddComponent<RectTransform>();
            moduleName.GetComponent<RectTransform>().sizeDelta = new Vector2(440, 100);
            moduleName.name = Module.Name + " Text";

            moduleName.AddComponent<TextMeshProUGUI>();
            moduleName.GetComponent<TextMeshProUGUI>().verticalAlignment = VerticalAlignmentOptions.Middle;
            moduleName.GetComponent<TextMeshProUGUI>().text = "  " + Module.Name;
            moduleName.GetComponent<TextMeshProUGUI>().font = InGameUi.font;
            moduleName.GetComponent<TextMeshProUGUI>().fontSize = 55;

            top.AddComponent<TenacityModuleController>();
            top.GetComponent<TenacityModuleController>().Module = Module;

            GameObject bottom = new GameObject();
            bottom.name = "Bottom";
            bottom.transform.SetParent(moduleObject.transform, false);
            bottom.AddComponent<RectTransform>();
            bottom.GetComponent<RectTransform>().sizeDelta = new Vector2(bottom.GetComponent<RectTransform>().sizeDelta.x, Module.Options.Count * 100);
            bottom.AddComponent<VerticalLayoutGroup>().childControlHeight = true;

            int index = 0;
            foreach (TenacityOption option in Module.Options)
            {
                GameObject row = new GameObject();
                row.transform.SetParent(bottom.transform, false);
                row.AddComponent<RectTransform>().sizeDelta = new Vector2(655, row.GetComponent<RectTransform>().sizeDelta.y);
                row.name = "Row " + index;
                row.AddComponent<HorizontalLayoutGroup>().childControlHeight = true;
                row.AddComponent<RawImage>().color = new Color32(35, 35, 35, 255);
                row.AddComponent<BoxCollider>().size = new Vector3(655, row.GetComponent<RectTransform>().sizeDelta.y, 1);
                row.GetComponent<BoxCollider>().isTrigger = true;

                GameObject optionText = new GameObject();
                optionText.transform.SetParent(row.transform, false);
                optionText.AddComponent<RectTransform>().sizeDelta = new Vector2(328, optionText.GetComponent<RectTransform>().sizeDelta.y);
                optionText.name = Module.Name + ": " + option.Name;
                optionText.AddComponent<TextMeshProUGUI>().text = "  " + option.Name;
                optionText.GetComponent<TextMeshProUGUI>().font = InGameUi.font;
                optionText.GetComponent<TextMeshProUGUI>().verticalAlignment = VerticalAlignmentOptions.Middle;
                optionText.GetComponent<TextMeshProUGUI>().fontSize = 40;

                if (option.OptionType == TenacityOption.TenacityOptionType.Toggle)
                {
                    row.AddComponent<RegularOptionController>().OptionIndex = index;
                    row.GetComponent<RegularOptionController>().Module = Module;
                }

                if (option.OptionType == TenacityOption.TenacityOptionType.Dropdown)
                {
                    GameObject dropdownText = new GameObject();
                    dropdownText.transform.SetParent(row.transform, false);
                    dropdownText.AddComponent<RectTransform>().sizeDelta = new Vector2(327, dropdownText.GetComponent<RectTransform>().sizeDelta.y);
                    dropdownText.AddComponent<TextMeshProUGUI>().font = InGameUi.font;
                    dropdownText.GetComponent<TextMeshProUGUI>().fontSize = 37;
                    dropdownText.GetComponent<TextMeshProUGUI>().verticalAlignment = VerticalAlignmentOptions.Middle;
                    dropdownText.GetComponent<TextMeshProUGUI>().horizontalAlignment = HorizontalAlignmentOptions.Right;
                    dropdownText.GetComponent<TextMeshProUGUI>().margin = new Vector4(0, 0, 20, 0);
                    dropdownText.GetComponent<TextMeshProUGUI>().text = option.SelectedDropdown + "   ";
                    row.AddComponent<DropdownOptionController>().Module = Module;
                    row.GetComponent<DropdownOptionController>().OptionIndex = index;
                    row.GetComponent<DropdownOptionController>().text = dropdownText.GetComponent<TextMeshProUGUI>();
                }

                index++;
            }
        }
    }
}
