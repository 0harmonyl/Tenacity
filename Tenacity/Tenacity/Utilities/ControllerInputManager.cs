using GorillaNetworking;
using HarmonyLib;
using UnityEngine;
using Valve.VR;
using CommonUsages = UnityEngine.XR.CommonUsages;

namespace Tenacity.Utilities
{
    public class ControllerInputManager : MonoBehaviour
    {
        public static bool LeftGrip, RightGrip, LeftTrigger, RightTrigger, LeftPrimary, RightPrimary, RightAxis2DClick, LeftAxis2DClick;
        public static Vector2 LeftAxis2D, RightAxis2D;

        void Update()
        {
            LeftGrip = ControllerInputPoller.instance.leftGrab;
            RightGrip = ControllerInputPoller.instance.rightGrab;
            LeftPrimary = ControllerInputPoller.instance.leftControllerPrimaryButton;
            RightPrimary = ControllerInputPoller.instance.rightControllerPrimaryButton;
            RightAxis2D = ControllerInputPoller.instance.rightControllerPrimary2DAxis;

            var isSteamVR = Traverse.Create(PlayFabAuthenticator.instance).Field("platform").GetValue().ToString().ToLower() == "steam";

            if (isSteamVR)
            {
                LeftAxis2D = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.GetAxis(SteamVR_Input_Sources.LeftHand);
                RightAxis2DClick = SteamVR_Actions.gorillaTag_RightJoystickClick.GetState(SteamVR_Input_Sources.RightHand);
                LeftAxis2DClick = SteamVR_Actions.gorillaTag_LeftJoystickClick.GetState(SteamVR_Input_Sources.LeftHand);
                LeftTrigger = SteamVR_Actions.gorillaTag_LeftTriggerClick.GetState(SteamVR_Input_Sources.LeftHand);
                RightTrigger = SteamVR_Actions.gorillaTag_RightTriggerClick.GetState(SteamVR_Input_Sources.RightHand);
            }
            else
            {
                ControllerInputPoller.instance.leftControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out LeftAxis2D);
                ControllerInputPoller.instance.rightControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out RightAxis2DClick);
                ControllerInputPoller.instance.leftControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out LeftAxis2DClick);
                ControllerInputPoller.instance.rightControllerDevice.TryGetFeatureValue(CommonUsages.triggerButton, out RightTrigger);
                ControllerInputPoller.instance.leftControllerDevice.TryGetFeatureValue(CommonUsages.triggerButton, out LeftTrigger);
            }
        }
    }
}