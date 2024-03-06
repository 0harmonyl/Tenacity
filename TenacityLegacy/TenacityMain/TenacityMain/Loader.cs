using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TenacityMain
{
    public class Loader
    {
        public static void Init()
        {
            Loader.Load = new UnityEngine.GameObject();
            Loader.Load.AddComponent<Plugin>();
            UnityEngine.Object.DontDestroyOnLoad(Loader.Load);
        }

        private static GameObject Load;
    }
}
