using System.Threading.Tasks;
using TenacityLib;
using UnityEngine;
using UnityEngine.UI;

namespace TenacityMain.UserInterface.MainMenu
{
    internal class TenacityMainMenu : MonoBehaviour
    {
        bool animationStarted = false;
        float timePassed;
        Canvas coverMapLoading;
        RawImage image;

        void Start()
        {
            GameObject obj = new GameObject("CoverMapLoading");
            coverMapLoading = obj.AddComponent<Canvas>();
            obj.AddComponent<RectTransform>();
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
            coverMapLoading.renderMode = RenderMode.ScreenSpaceCamera;
            var mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            coverMapLoading.worldCamera = mainCamera;
            coverMapLoading.planeDistance = mainCamera.nearClipPlane + 0.1f;
            image = obj.AddComponent<RawImage>();
            image.color = new Color32(0, 0, 0, 0);
        }

        async void Update()
        {
            if (ControllerInputManager.rightTrigger && !animationStarted || ControllerInputManager.leftTrigger && !animationStarted)
            {
                StartAnimation();
            }

            if (animationStarted)
            {
                timePassed += Time.deltaTime;

                if (timePassed < 3.0f)
                {
                    float alpha = Mathf.Lerp(0f, 255f, timePassed / 3.0f);
                    image.color = new Color32(0, 0, 0, (byte)alpha);

                    ParticleSystem MenuParticles = GameObject.Find("MainMenuParticleSystem").GetComponent<ParticleSystem>();
                    #pragma warning disable CS0618
                    MenuParticles.playbackSpeed += 0.15f;
                }
                else if (timePassed >= 3.0f && timePassed < 4.5f)
                {
                    image.color = new Color32(0, 0, 0, 255);
                    GorillaTagger.Instance.transform.position = Plugin.StartPosition;
                    GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;

                    await Task.Delay(35);
                }
                else if (timePassed >= 4.5f)
                {
                    float alpha = Mathf.Lerp(255f, 0f, (timePassed - 4.5f) / 1.5f);
                    image.color = new Color32(0, 0, 0, (byte)alpha);
                }

                if (timePassed >= 3.0f)
                {
                    Plugin.TheWorld.SetActive(true);
                }

                if (timePassed >= 6.0f)
                {
                    Destroy(GameObject.Find("CoverMapLoading"));
                    Destroy(GameObject.Find("Tenacity Main Menu"));
                }
            }
        }

        void StartAnimation()
        {
            animationStarted = true;
        }
    }
}
