using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using TenacityLib;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TenacityMain.Effects;
using TenacityMain.ModSystem;
using TenacityMain.Utils;

namespace TenacityMain.TenacityMods.Blatant
{
    public class ProjectileSpammerOld : MonoBehaviour
    {
        public string Name
        {
            get { return "Projectile Spammer"; }
        }

        public string Description 
        { 
            get { return "An advanced configurable projectile spammer module"; }
        }

        public string Tab
        {
            get { return "Blatant Tab"; }
        }

        public bool Enabled { get; set; }
        public List<TenacityOption> Options { get; set; }

        public static ProjectileSpammerOld instance;

        public void Start()
        {
            instance = this;
            spammerController.SetActive(true);
        }

        public void Setup()
        {
            GameObject spammerObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            spammerObject.transform.SetParent(GorillaTagger.Instance.transform, true);
            spammerObject.GetComponent<Renderer>().enabled = false;
            spammerObject.GetComponent<Collider>().enabled = false;

            spammerController = spammerObject;
            
            Options = new List<TenacityOption>()
            {
                new TenacityOption()
                {
                    Name = "Amount",
                    DropdownOptions =
                    {
                        "1",
                        "2",
                        "3",
                        "4",
                        "5",
                        "6",
                        "7",
                    },
                    SelectedDropdown = "1",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown
                },
                new TenacityOption()
                {
                    Name = "Different Projectiles?",
                    Enabled = false,
                    OptionType = TenacityOption.TenacityOptionType.Toggle
                },
                new TenacityOption()
                {
                    Name = "Main Projectile",
                    DropdownOptions = new List<string>()
                    {
                      "Elfbow",
                      "Rock",
                      "Rainbow",
                      "Snowball",
                      "Waterballon",
                      "Slingshot"
                    },
                    SelectedDropdown = "Slingshot",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown
                },
                new TenacityOption()
                {
                    Name = "Anti Lag",
                    Enabled = false,
                    OptionType = TenacityOption.TenacityOptionType.Toggle
                },
                new TenacityOption()
                {
                    Name = "Force",
                    DropdownOptions =
                    {
                        "0",
                        "1000",
                        "2000",
                        "3000"
                    },
                    SelectedDropdown = "3000",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown
                },
                new TenacityOption()
                {
                    Name = "Full Lag Clear",
                    Enabled = false,
                    OptionType = TenacityOption.TenacityOptionType.Toggle
                },
                new TenacityOption()
                {
                    Name = "Rainbow Spam",
                    Enabled = true,
                    OptionType = TenacityOption.TenacityOptionType.Toggle
                },
                new TenacityOption()
                {
                    Name = "Color",
                    OptionType = TenacityOption.TenacityOptionType.Dropdown,
                    DropdownOptions =
                    {
                        "Tenacity",
                        "Dark Red",
                        "Red",
                        "Orange",
                        "Mint Green",
                        "Green",
                        "Blue",
                        "Pink",
                        "Purple",
                    },
                    SelectedDropdown = "Tenacity",
                }
            };
        }

        public void Cleanup()
        {
            spammerController.SetActive(false);
        }
        
        ///////////////////////////////////////////////////////////////////

        private int SpammerAmount = 1;
        private bool DifferentProjectiles;
        private GameObject spammerController;
        private float SetForce = 0f;

        private List<string> Projectiles = new List<string>()
        {
            "SlingshotProjectile",
            "SnowballProjectile",
            "WaterBalloonProjectile",
            "LavaRockProjectile",
            "CloudSlingshot_Projectile",
            "ElfBow_Projectile"
        };

        public void Update()
        {
            if (!Enabled) return;

            if (Options[5].Enabled)
            {
                FullLagClear();
                Options[5].Enabled = !Options[5].Enabled;
            }

            TenacityModSystem.Projctiles = !Options[3].Enabled;

            switch (Options[0].SelectedDropdown)
            {
                case "1":
                    SpammerAmount = 1;
                    break;
                case "2":
                    SpammerAmount = 2;
                    break;
                case "3":
                    SpammerAmount = 3;
                    break;
                case "4":
                    SpammerAmount = 4;
                    break;
                case "5":
                    SpammerAmount = 5;
                    break;
                case "6":
                    SpammerAmount = 6;
                    break;
                case "7":
                    SpammerAmount = 7;
                    break;
            }

            switch (Options[4].SelectedDropdown)
            {
                case "0":
                    SetForce = 0f;
                    break;
                case "1000":
                    SetForce = 1000f;
                    break;
                case "2000":
                    SetForce = 2000f;
                    break;
                case "3000":
                    SetForce = 3000f;
                    break;
            }

            DifferentProjectiles = Options[1].Enabled;

            if (ControllerInputManager.rightGrip)
            {
                Kebab();
            }
        }

        void Kebab()
        {
            if (!DifferentProjectiles)
            {
                SpammerControllerAmountCheck();
                
                foreach (TenacityProjectileSpammer spammer in spammerController
                             .GetComponents<TenacityProjectileSpammer>())
                {
                    spammer.ProjectileString = MainProjectile();
                    spammer.Force = SetForce;
                }
            }
            else
            {
                SpammerControllerAmountCheck();

                int index = 0;
                
                foreach (TenacityProjectileSpammer spammer in spammerController
                             .GetComponents<TenacityProjectileSpammer>())
                {
                    spammer.ProjectileString = Projectiles[index];
                    spammer.Force = SetForce;
                    if (!Options[6].Enabled)
                    {
                        spammer.projectileColor = GetProjectileColor();
                    }
                    else
                    {
                        spammer.projectileColor = HardRainbowColor();
                    }
                    index++;
                } 
            }
        }

        async void SpammerControllerAmountCheck()
        {
            await Task.Delay(100);
            
            if (spammerController.GetComponents<TenacityProjectileSpammer>().Length > SpammerAmount)
            {
                Destroy(spammerController.GetComponent<TenacityProjectileSpammer>());
            }
            else if (spammerController.GetComponents<TenacityProjectileSpammer>().Length < SpammerAmount)
            {
                TenacityProjectileSpammer spammer = spammerController.AddComponent<TenacityProjectileSpammer>();
                spammer.Force = 3000f;
                spammer.ProjectileString = MainProjectile();
                if (!Options[6].Enabled)
                {
                    spammer.projectileColor = GetProjectileColor();
                }
                else
                {
                    spammer.projectileColor = HardRainbowColor();
                }
            }

            await Task.Delay(100);
        }

        Color32 HardRainbowColor()
        {
            float h = (float)Time.frameCount / 100f % 1f;
            Color newColor = Color.HSVToRGB(h, 1f, 1f);
            float r = Mathf.Floor(newColor.r * 2f) / 2f * 255f * 100f;
            float g = Mathf.Floor(newColor.r * 2f) / 2f * 255f * 100f;
            float b = Mathf.Floor(newColor.r * 2f) / 2f * 255f * 100f;

            return new Color32((byte)r, (byte)g, (byte)b, byte.MaxValue);
        }

        Color GetProjectileColor()
        {
            Color newColor = new Color();

            if (Options[7].SelectedDropdown == "Tenacity")
                newColor = TenacityColorManager.TenacityColor;
            if (Options[7].SelectedDropdown == "Dark Red")
                newColor = new Color32(145, 15, 0, 255);
            if (Options[7].SelectedDropdown == "Red")
                newColor = Color.red;
            if (Options[7].SelectedDropdown == "Orange")
                newColor = new Color32(255, 190, 11, 255);
            if (Options[7].SelectedDropdown == "Mint Green")
                newColor = new Color32(128, 255, 114, 255);
            if (Options[7].SelectedDropdown == "Green")
                newColor = new Color32(53, 191, 29, 255);
            if (Options[7].SelectedDropdown == "Blue")
                newColor = new Color32(32, 164, 243, 255);
            if (Options[7].SelectedDropdown == "Pink")
                newColor = new Color32(255, 159, 243, 255);
            if (Options[7].SelectedDropdown == "Purple")
                newColor = new Color32(98, 71, 170, 255);

            return newColor;
        }

        private string MainProjectile()
        {
            switch (Options[2].SelectedDropdown)
            {
                case "Elfbow":
                    return "ElfBow_Projectile";
                case "Rainbow":
                    return
                        "CloudSlingshot_Projectile";
                case "Snowball":
                    return "SnowballProjectile";
                case "Waterballon":
                    return
                        "WaterBalloonProjectile";
                case "Slingshot":
                    return "SlingshotProjectile";
                case "Rock":
                    return "LavaRockProjectile";
            }
            return "";
        }

        private bool DeleteMode;
        
        async void FullLagClear()
        {
            await Task.Delay(100);

            int index = 0;
            
            foreach (Transform obj in GameObject.Find("Environment Objects/PersistentObjects_Prefab/GlobalObjectPools").transform)
            {
                if (index > 9000)
                {
                    DeleteMode = true;
                }
                else
                {
                    DeleteMode = false;
                }
                
                if (DeleteMode)
                {
                    Destroy(obj.gameObject);
                }
                else
                {
                    obj.gameObject.SetActive(false);
                }

                index++;
            }

            if (index > 9000)
            {
                DeleteMode = true;
            }
            else
            {
                DeleteMode = false;
            }
        }
    }

    public class TenacityProjectileSpammer : MonoBehaviour
    {
        public string ProjectileString;
        public float Force;
        public Color projectileColor;
        private float debounceTime = 0f;
        private SnowballThrowable snowball;

        void Update()
        {
            if (snowball == null)
            {
                foreach (SnowballThrowable snowballThrowable in Resources.FindObjectsOfTypeAll<SnowballThrowable>())
                {
                    if (snowballThrowable != null)
                    {
                        snowball = snowballThrowable;
                        break;
                    }
                }
            }
            
            if (!ControllerInputManager.rightGrip) return;

            try
            {
                BetaFireProjectile(ProjectileString, GorillaTagger.Instance.rightHandTransform.position,
                    -GorillaTagger.Instance.rightHandTransform.up * Time.deltaTime * Force, projectileColor);
            }
            catch (Exception e)
            {
                Debug.Log("Error Message: " + e.Message + "\nStacktrace: " + e.StackTrace);
            }
        }

        void BetaFireProjectile(string projectileName, Vector3 position, Vector3 velocity, Color color)
        {
            if (Time.time > debounceTime)
            {
                projectileColor = color;
                Vector3 velocity2 = GorillaTagger.Instance.GetComponent<Rigidbody>().velocity;
                GorillaTagger.Instance.GetComponent<Rigidbody>().velocity = velocity;
                SnowballThrowable component = snowball;
                Vector3 position2 = component.transform.position;
                component.randomizeColor = true;
                component.transform.position = position;
                component.projectilePrefab.tag = projectileName;
                component.OnRelease(null, null);
                component.transform.position = position2;
                GorillaTagger.Instance.GetComponent<Rigidbody>().velocity = velocity2;
                
                if (debounceTime > 0f)
                {
                    debounceTime = Time.time + debounceTime;
                }
            }
        }
    }
}