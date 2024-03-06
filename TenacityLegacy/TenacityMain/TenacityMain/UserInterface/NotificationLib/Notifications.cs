using System;
using System.Linq;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TenacityMain.UserInterface.NotificationLib
{
    public enum NotificationType
    {
        ModuleEnabled,
        ModuleDisabled,
        Reported,
        AntiCheat
    }
    
    public static class Notifications
    {
        private static int _currentNotificationCount = 0;
        public const int InGameDecayTime = 600;
        public static int inGameNotificationDecayTimeCounter = 0;
        public static string[] inGameNotificationLines;
        public static string inGameNewText;
        public static string inGamePreviousNotification;
        
        public static void SendNotification(string message, float decayTime, NotificationType type)
        {
            SendInGameNotification(message);
            SendOnScreenNotification(message, decayTime, type);
        }

        public static void SendInGameNotification(string message)
        {
            if (!message.Contains(Environment.NewLine)) message += Environment.NewLine;
            InGameUi.InGameUi.CenterTopText.text += message;
            inGamePreviousNotification = message;
        }

        public static void ClearPastInGameNotifications(int amount)
        {
            inGameNotificationLines = null;
            inGameNewText = "";
            inGameNotificationLines = InGameUi.InGameUi.CenterTopText.text.Split(Environment.NewLine.ToCharArray())
                .Skip(amount).ToArray();
            foreach (string line in inGameNotificationLines)
            {
                if (line != "")
                {
                    inGameNewText += line + "\n";
                }
            }
        }

        public static void ClearAllInGameNotifications()
        {
            InGameUi.InGameUi.CenterTopText.text = "";
        }

        public static void SendOnScreenNotification(string message, float decayTime, NotificationType type)
        {
            _currentNotificationCount++;
            var isGreen = type == NotificationType.ModuleEnabled;

            var onScreenNotificationContainer = new GameObject();
            onScreenNotificationContainer.transform.SetParent(OnScreenUi.OnScreenUi.Canvas.transform);

            var notificationObject = Object.Instantiate(isGreen ? Plugin.MainAssetBundle.LoadAsset<GameObject>("GreenNotification.prefab") : Plugin.MainAssetBundle.LoadAsset<GameObject>("RedNotification.prefab"), onScreenNotificationContainer.transform);
            notificationObject.transform.Find("Title").GetComponent<TMP_Text>().enableAutoSizing = true;
            notificationObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            notificationObject.transform.position = new Vector3(2000f, 120 * (_currentNotificationCount - 1) + 120f, 0f);

            var interpolationScript = onScreenNotificationContainer.AddComponent<OnScreenNotificationInterpolation>();
            interpolationScript.Init(notificationObject.transform, decayTime);
            
            notificationObject.transform.Find("Description").GetComponent<TMP_Text>().text = message;

            if (type == NotificationType.ModuleDisabled || type == NotificationType.ModuleEnabled)
            {
                notificationObject.transform.Find("Title").GetComponent<TMP_Text>().text = "Module Toggled";
            }
            else if (type == NotificationType.Reported)
            {
                notificationObject.transform.Find("Title").GetComponent<TMP_Text>().text = "Reported";
            }
            else if (type == NotificationType.AntiCheat)
            {
                notificationObject.transform.Find("Title").GetComponent<TMP_Text>().text = "AntiCheat";
            }
            
            
        }

        public static void NotificationDisappearing()
        {
            _currentNotificationCount -= 1;
        }
     }

    public class OnScreenNotificationInterpolation : MonoBehaviour
    {
        private Transform notificationTransform;
        private float fadeInTime = 0.5f;
        private float decayTime;
        private float elapsedTime;
        private bool nonce;

        public void Init(Transform transform, float _decayTime)
        {
            notificationTransform = transform;
            decayTime = _decayTime;
            elapsedTime = 0f;
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < fadeInTime)
            {
                float t = Mathf.Clamp01(elapsedTime / fadeInTime);

                float currentX = Mathf.Lerp(2000f, 1770f, t);

                Vector3 newPos = notificationTransform.position;
                newPos.x = currentX;
                notificationTransform.position = newPos;
            }
            else
            {
                float tDecay = Mathf.Clamp01((elapsedTime - fadeInTime) / decayTime);

                if (tDecay >= 1 && !nonce)
                {
                    nonce = true;
                    Notifications.NotificationDisappearing();
                    Destroy(gameObject);
                }
            }
        }
    }

    public class InGameNotificationCounter : MonoBehaviour
    {
        private void FixedUpdate()
        {
            Notifications.inGameNotificationDecayTimeCounter++;
            if (Notifications.inGameNotificationDecayTimeCounter > Notifications.InGameDecayTime)
            {
                Notifications.inGameNotificationLines = null;
                Notifications.inGameNewText = "";
                Notifications.inGameNotificationDecayTimeCounter = 0;
                Notifications.inGameNotificationLines = InGameUi.InGameUi.CenterTopText.text 
                    .Split(Environment.NewLine.ToCharArray()).Skip(1).ToArray();
                foreach (string line in Notifications.inGameNotificationLines)
                {
                    if (line != "")
                    {
                        Notifications.inGameNewText += line + "\n";
                    }
                }

                InGameUi.InGameUi.CenterTopText.text = Notifications.inGameNewText;
            } 
        }
    }
}