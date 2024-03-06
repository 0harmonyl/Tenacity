using System.Collections.Generic;
using Photon.Pun;
using TenacityLib;
using TenacityMain.Utils;
using UnityEngine;

namespace TenacityMain.TenacityMods.Blatant
{
    public class ProjectileSpammerTest : MonoBehaviour, ITenacityModule
    {
        public string Name => "ProjSpamTest";
        public string Tab => "Blatant Tab";
        public string Description => "test";
        
        public List<TenacityOption> Options { get; set; }
        public bool Enabled { get; set; }

        public void Setup()
        {
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Delay",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "0.5",
                        "0.25",
                        "0.2",
                        "0.1",
                    },
                    SelectedDropdown = "0.1",
                },
                new TenacityOption()
                {
                    Name = "Projectile",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Slingshot",
                        "Snowball",
                        "WaterBalloon",
                        "LavaRock",
                        "Horns Slingshot",
                        "Cloud Slingshot",
                        "Cupid Bow",
                        "Ice Slingshot",
                        "Elf Bow",
                        "Molten Slingshot",
                        "Spider Bow",
                        "Mentos"
                    },
                    SelectedDropdown = "Snowball"
                },
                new TenacityOption()
                {
                    Name = "Color",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Black",
                        "White",
                        "Red",
                        "Blue",
                        "Yellow",
                        "Magenta",
                        "Cyan",
                        "Green"
                    }
                },
                new TenacityOption()
                {
                    Name = "Random Projectile",
                    OptionType = TenacityOption.TenacityOptionType.Toggle,
                    Enabled = false
                },
                new TenacityOption()
                {
                    Name = "Random Color",
                    OptionType = TenacityOption.TenacityOptionType.Toggle,
                    Enabled = false,
                }
            };
        } 
        
        public void Start() { }
        
        public void Cleanup() { }
        
        ///////////////////////////////////////////

        private float projDebounce = 0f;
        private float projDebounceType = 0.05f;

        private string[] projectiles = 
        {
            "SlingshotProjectile",
            "SnowballProjectile",
            "WaterBalloonProjectile",
            "LavaRockProjectile",
            "HornsSlingshotProjectile",
            "CloudSlingshot_Projectile",
            "CupidBow_Projectile",
            "IceSlingshot_Projectile",
            "ElfBow_Projectile",
            "MoltenSlingshot_Projectile",
            "SpiderBow_Projectile",
            "ScienceCandyProjectile"
        };

        private Color[] colors =
        {
            Color.black,
            Color.white,
            Color.red,
            Color.blue,
            Color.yellow,
            Color.magenta,
            Color.cyan,
            Color.green,
        };

        private void Update()
        {
            if (!Enabled) return;
            if (!ControllerInputManager.rightGrip) return;

            if (Options[0].SelectedDropdown == "0.5")
                projDebounceType = 0.5f;
            if (Options[0].SelectedDropdown == "0.25")
                projDebounceType = 0.25f;
            if (Options[0].SelectedDropdown == "0.2")
                projDebounceType = 0.2f;
            if (Options[0].SelectedDropdown == "0.1")
                projDebounceType = 0.1f;

            var color = Options[4].Enabled ? colors[Random.Range(0, 7)] : GetCurrentColor();
            var projectileName = Options[3].Enabled ? projectiles[Random.Range(0, 11)] : GetCurrentProjectile();
            
            BetaFireProjectile(projectileName, GorillaTagger.Instance.rightHandTransform.position,
                -GorillaTagger.Instance.rightHandTransform.up * Time.deltaTime * 3000f, color);
        } 

        public void BetaFireProjectile(string projectileName, Vector3 position, Vector3 velocity, Color color,
            bool noDelay = false)
        {
            if (Time.time > projDebounce)
            {
                Vector3 oldVel = GorillaTagger.Instance.GetComponent<Rigidbody>().velocity;
                GorillaTagger.Instance.GetComponent<Rigidbody>().velocity = velocity;
                SnowballThrowable fart = GameObject
                    .Find(
                        "Player Objects/Local VRRig/Local Gorilla Player/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/TransferrableItemRightHand/SnowballRightAnchor")
                    .transform.Find("LMACF.").GetComponent<SnowballThrowable>();
                Vector3 oldPos = fart.transform.position;
                GorillaTagger.Instance.offlineVRRig.SetThrowableProjectileColor(false, color);
                fart.transform.position = position;
                fart.projectilePrefab.tag = projectileName;
                fart.OnRelease(null, null);
                fart.transform.position = oldPos;
                GorillaTagger.Instance.GetComponent<Rigidbody>().velocity = oldVel;
                fart.randomizeColor = false;
                if (projDebounceType > 0f && !noDelay)
                {
                    projDebounce = Time.time + projDebounceType;
                }
            }
        }

        Color GetCurrentColor()
        {
            switch (Options[2].SelectedDropdown)
            {
                case "Black":
                    return Color.black;
                case "White":
                    return Color.white;
                case "Red":
                    return Color.red;
                case "Blue":
                    return Color.blue;
                case "Yellow":
                    return Color.yellow;
                case "Magenta":
                    return Color.magenta;
                case "Cyan":
                    return Color.cyan;
                case "Green":
                    return Color.green;
            }

            return Color.white;
        }

        string GetCurrentProjectile()
        {
            switch (Options[1].SelectedDropdown)
            {
                case "Slingshot":
                    return "SlingshotProjectile";
                case "Snowball":
                    return "SnowballProjectile";
                case "WaterBalloon":
                    return "WaterBalloonProjectile";
                case "LavaRock":
                    return "LavaRockProjectile";
                case "Horns Slingshot":
                    return "HornsSlingshotProjectile";
                case "Cloud Slingshot":
                    return "CloudSlingshot_Projectile";
                case "Cupid Bow":
                    return "CupidBow_Projectile";
                case "Ice Slingshot":
                    return "IceSlingshot_Projectile";
                case "Elf Bow":
                    return "ElfBow_Projectile";
                case "Molten Slingshot":
                    return "MoltenSlingshot_Projectile";
                case "Spider Bow":
                    return "SpiderBow_Projectile";
                case "Mentos":
                    return "ScienceCandyProjectile";
            }

            return "";
        }
    }
}