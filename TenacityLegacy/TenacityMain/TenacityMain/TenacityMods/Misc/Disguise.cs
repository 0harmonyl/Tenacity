using System.Collections.Generic;
using System.Threading.Tasks;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using TenacityLib;
using TenacityMain.ModSystem;
using UnityEngine;

namespace TenacityMain.TenacityMods.Misc
{
    public class Disguise : MonoBehaviour, ITenacityModule
    {
        public string Name => "Disguise";
        public string Description => "Disguise Mod";
        public string Tab => "Misc Tab";
        
        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>();
        }
        
        public void Start() { }
        
        public void Cleanup() { }
        
        //////////////////////////////////////////////////////

        public async void Update()
        {
            if (!Enabled) return;
            
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-66.7989f, 12.5422f, -82.6815f);
            
            if (!GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(PhotonNetwork
                    .LocalPlayer.UserId))
            {
                Enabled = false;
                TenacityModSystem.UpdateAllControllerStates();
                return;
            } 

            DisguisePlayer();

            await Task.Delay(10);

            GorillaTagger.Instance.offlineVRRig.enabled = true;
            
            Enabled = false;
            TenacityModSystem.UpdateAllControllerStates();
        }

        void DisguisePlayer()
        {
            ChangeName(RandomName());
            ChangeColor(RandomColor());
        }

        private Color RandomColor()
        {
            return new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255),
                byte.MaxValue);
        }

        private string RandomName()
        {
            string[] adjectives = { "Gorilla", "Monkey", "1234567", "Jman", "Jmancurly", "Vmt", "Banana", "Boo", "nej", "Gamingchair", "Gorillafeet", "Dogs", "Cats", "Fortnite", "Valorant", "Gorillatag", "Jmanfan", "Jmaniscurly", "Jaymancurry", "Kebab", "Skittle", "Yourcheating" };
            return adjectives[Random.Range(0, adjectives.Length - 1)];
        }

        private void ChangeName(string name)
        {
            GorillaComputer.instance.currentName = name;
            PhotonNetwork.LocalPlayer.NickName = name;
            GorillaComputer.instance.offlineVRRigNametagText.text = name;
            GorillaComputer.instance.savedName = name;
            PlayerPrefs.SetString("playerName", name);
            PlayerPrefs.Save();
        }

        private void ChangeColor(Color color)
        {
            PlayerPrefs.SetFloat("redValue", Mathf.Clamp(color.r, 0f, 1f));
            PlayerPrefs.SetFloat("greenValue", Mathf.Clamp(color.g, 0f, 1f));
            PlayerPrefs.SetFloat("blueValue", Mathf.Clamp(color.b, 0f, 1f));
            GorillaTagger.Instance.UpdateColor(color.r, color.g, color.g);
            PlayerPrefs.Save();
            GorillaTagger.Instance.myVRRig.RPC("InitializeNoobMaterial", RpcTarget.All, new object[]
            {
                color.r,
                color.g,
                color.b,
                false
            });
        }
    }
}