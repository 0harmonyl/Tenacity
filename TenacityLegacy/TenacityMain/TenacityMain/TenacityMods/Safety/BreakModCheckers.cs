using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TenacityLib;
using UnityEngine;
using UnityEngine.Networking;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace TenacityMain.TenacityMods.Safety
{
    internal class BreakModCheckers : MonoBehaviour, ITenacityModule
    {
        public string Name
        {
            get { return "BMC"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public string Tab
        {
            get { return "Safety Tab"; }
        }

        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Mode",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions = new List<string>
                    {
                        "Show Legal",
                        "Show None"
                    },
                    SelectedDropdown = "Show Legal"
                }
            };
            Enabled = true;
        }

        public void Start() { }

        public void Update()
        {
            if (!Enabled) return;

            if (PhotonNetwork.InRoom)
            {
                if (PhotonNetwork.LocalPlayer.CustomProperties["mods"] != null)
                {
                    HideMods();
                }
            }
        }

        private void HideMods()
        {
            if (Options[0].SelectedDropdown == "Show Legal")
                StartCoroutine(GetLegalText());
            if (Options[0].SelectedDropdown == "Show None")
                StartCoroutine(GetNoneText());
        }

        IEnumerator GetLegalText()
        {
            using (UnityWebRequest url = UnityWebRequest.Get("https://pastebin.com/raw/knhRssSU"))
            {
                yield return url.SendWebRequest();
                if (url.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError("Failed to retrieve mod text: " + url.error);
                    yield break;
                }
                string modText = url.downloadHandler.text;
                Hashtable hashtable = new Hashtable();
                hashtable.Add("mods", modText);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable, null, null);
                modText = null;
                hashtable = null;
            }
            yield break;
        }

        IEnumerator GetNoneText()
        {
            string modText = "";
            Hashtable hashtable = new Hashtable();
            hashtable.Add("mods", modText);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable, null, null);
            modText = null;
            hashtable = null;
            yield break;
        }

        public void Cleanup() { }
    }
}
