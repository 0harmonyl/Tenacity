using System;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace TenacityMain.Utils
{
    public class TenacityProjectileTracker : MonoBehaviourPunCallbacks
    {
        private static int _localPlayerProjectileCount = 0;

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            ClearProjectiles();
        }

        public static int IncrementLocalPlayerProjectileCount()
        {
            _localPlayerProjectileCount++;
            return _localPlayerProjectileCount;
         }
 
         private static void ClearProjectiles()
         {
             _localPlayerProjectileCount = 0;
         }
    }
}