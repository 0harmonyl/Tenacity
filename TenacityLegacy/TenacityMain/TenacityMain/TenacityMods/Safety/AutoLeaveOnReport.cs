using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TenacityLib;
using TenacityMain.ModSystem;
using TenacityMain.TenacityMods.Settings;
using TenacityMain.UserInterface.NotificationLib;
using UnityEngine;

namespace TenacityMain.TenacityMods.Safety
{
    internal class AutoLeaveOnReport : MonoBehaviourPunCallbacks, ITenacityModule
    {
        public string Name
        {
            get { return "ALOR"; }
        }

        public string Description
        {
            get { return "Leave when a players hand is within certain range of your report button"; }
        }

        public string Tab
        {
            get { return "Safety Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        float distance = 0.42f;
        List<GorillaScoreBoard> allScoreboards = new List<GorillaScoreBoard>();
        bool inRoomNonce;

        public void Setup() 
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Distance",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "0.3",
                        "0.35",
                        "0.4",
                        "0.42",
                        "0.45",
                        "0.6",
                    },
                    SelectedDropdown = "0.42"
                }
            };
            Enabled = true;
        }

        public void Start() { }

        public async void Update()
        {
            if (!Enabled) return;

            if (Options[0].SelectedDropdown == "0.3")
                distance = 0.3f;
            if (Options[0].SelectedDropdown == "0.35")
                distance = 0.35f;
            if (Options[0].SelectedDropdown == "0.4")
                distance = 0.4f;
            if (Options[0].SelectedDropdown == "0.42")
                distance = 0.42f;
            if (Options[0].SelectedDropdown == "0.45")
                distance = 0.45f;
            if (Options[0].SelectedDropdown == "0.6")
                distance = 0.6f;

            if (!PhotonNetwork.InRoom)
            {
                inRoomNonce = false;
                allScoreboards.Clear();
                return;
            }

            if (!inRoomNonce)
            {
                foreach (GameObject gameObj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
                {
                    if (gameObj.GetComponent<GorillaScoreBoard>() != null)
                    {
                        allScoreboards.Add(gameObj.GetComponent<GorillaScoreBoard>());
                    }
                }
                inRoomNonce = true;
            };

            foreach (GorillaScoreBoard scb in allScoreboards)
            {
                foreach (GorillaPlayerScoreboardLine line in scb.lines)
                {
                    if (line.playerNameValue == PhotonNetwork.LocalPlayer.NickName.ToUpper())
                    {
                        foreach (VRRig rig in GameObject.Find("Player Objects/RigCache/Rig Parent").GetComponentsInChildren(typeof(VRRig)))
                        {
                            if (Vector3.Distance(line.reportButton.gameObject.transform.position, rig.leftHandTransform.position) < distance)
                            {
                                string playerName = rig.playerText.text;
                                string roomCode = PhotonNetwork.CurrentRoom.Name;
                                PhotonNetwork.Disconnect();
                                NotificationCheck(playerName, roomCode);
                            }
                            if (Vector3.Distance(line.reportButton.gameObject.transform.position, rig.rightHandTransform.position) < distance)
                            {
                                string playerName = rig.playerText.text;
                                string roomCode = PhotonNetwork.CurrentRoom.Name;
                                PhotonNetwork.Disconnect();
                                NotificationCheck(playerName, roomCode);
                            }
                        }
                    }
                }
            }

            await Task.Delay(25);
        }

        void NotificationCheck(string rig, string roomCode)
        {
            if (TenacityModSystem.AlorNotifications)
            {
                Notifications.SendOnScreenNotification("You got reported", 3f, NotificationType.Reported);
                Notifications.SendInGameNotification("<color=\"orange\">[ALOR] You got reported by " + rig + " Room Code: " + roomCode);
            }
        }

        public void Cleanup() { }
    }
}
