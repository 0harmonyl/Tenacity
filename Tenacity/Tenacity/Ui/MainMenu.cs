using System;
using System.Threading.Tasks;
using Tenacity.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Tenacity.Ui
{
    public class MainMenu : MonoBehaviour
    {
        private bool animationStarted = false;
        private float timePassed;
        private Canvas mapCover;
        private RawImage img;

        private void Start()
        {
            var gameObj = new GameObject("MapCover");
            mapCover = gameObj.AddComponent<Canvas>();
            gameObj.AddComponent<RectTransform>();
            gameObj.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
            mapCover.renderMode = RenderMode.ScreenSpaceCamera;
            var mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            mapCover.worldCamera = mainCamera;
            mapCover.planeDistance = mainCamera.nearClipPlane + 0.1f;
            img = gameObj.AddComponent<RawImage>();
            img.color = new Color32(0, 0, 0, 0);
        }

        [Obsolete("Obsolete")]
        private async void Update()
        {
            if (ControllerInputManager.RightTrigger && !animationStarted ||
                ControllerInputManager.LeftTrigger && !animationStarted)
            {
                animationStarted = true;
            }

            if (!animationStarted) return;
            
            timePassed += Time.deltaTime;

            if (timePassed < 3.0f)
            {
                var alpha = Mathf.Lerp(0f, 255f, timePassed / 3.0f);
                img.color = new Color32(0, 0, 0, (byte)alpha);

                var menuParticles = GameObject.Find("MainMenuParticleSystem").GetComponent<ParticleSystem>();
                menuParticles.playbackSpeed += 0.15f;
            }
            else if (timePassed >= 3.0f && timePassed < 4.5f)
            {
                img.color = new Color32(0, 0, 0, 255);
                GorillaTagger.Instance.transform.position = Loader.Instance.startPosition;
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;

                await Task.Delay(35);
            }
            else if (timePassed >= 4.5f)
            {
                var alpha = Mathf.Lerp(255f, 0f, (timePassed - 4.5f) / 1.5f);
                img.color = new Color32(0, 0, 0, (byte)alpha);
            }

            if (timePassed >= 3.0f)
            {
                Loader.Instance.theWorld.SetActive(true);
            }

            if (!(timePassed >= 6.0f)) return;
            
            Destroy(GameObject.Find("MapCover"));
            Destroy(GameObject.Find("Tenacity Main Menu"));
        }
    }
}