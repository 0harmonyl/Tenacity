using HarmonyLib;
using Photon.Pun;
using TenacityMain.ModSystem;
using TenacityMain.TenacityMods.Settings;

namespace TenacityMain.UserInterface.NotificationLib.Notification
{
    [HarmonyPatch(typeof(GorillaNot), "SendReport")]
    public class AntiCheatNotification
    {
        private static bool Prefix(string susReason, string susId, string susNick)
        {
            if (TenacityModSystem.AntiCheatNotifType == "Self" && susId == PhotonNetwork.LocalPlayer.UserId)
            {
                Notifications.SendOnScreenNotification("Flagged By AntiCheat", 3f, NotificationType.AntiCheat);
                Notifications.SendInGameNotification("<color=\"red\">[GORILLANOT] You were flagged by GorillaNot for " + susReason);
                susNick.Remove(PhotonNetwork.LocalPlayer.NickName.Length);
                susId.Remove(PhotonNetwork.LocalPlayer.UserId.Length);
            }
            else if (TenacityModSystem.AntiCheatNotifType == "All")
            {
                if (susId == PhotonNetwork.LocalPlayer.UserId)
                {
                    Notifications.SendOnScreenNotification("Flagged By AntiCheat", 3f, NotificationType.AntiCheat);
                    Notifications.SendInGameNotification("<color=\"red\">[GORILLANOT] You were flagged by GorillaNot for " + susReason);
                    susNick.Remove(PhotonNetwork.LocalPlayer.NickName.Length);
                    susId.Remove(PhotonNetwork.LocalPlayer.UserId.Length);
                }
                else
                {
                    Notifications.SendOnScreenNotification(susNick + " was Flagged", 3f, NotificationType.AntiCheat);
                    Notifications.SendInGameNotification("<color=\"red\">[GORILLANOT] flagged " + susNick + " for " + susReason);   
                }
            }

            return false;
        }
    }
}