using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Tenacity.ModuleSystem
{
    public class ModuleManager : MonoBehaviour
    {
        public static ModuleManager Instance;
        public Dictionary<Type, bool> Modules;

        public void Start()
        {
            Instance = this;
            Modules = new Dictionary<Type, bool>();

            var mainAssembly = Assembly.GetExecutingAssembly();

            var moduleTypes = mainAssembly.GetTypes()
                .Where(type => type.IsClass && typeof(ITenacityModule).IsAssignableFrom(type))
                .ToList();

            foreach (var type in moduleTypes)
            {
                Modules.Add(type, false);
            }
        }

        public void EnableModule(Type type)
        {
            var component = (ITenacityModule)GorillaTagger.Instance.gameObject.AddComponent(type);
            Modules[type] = true;
            component?.OnEnable();
        }

        public void DisableModule(Type type)
        {
            var component = (ITenacityModule)GorillaTagger.Instance.gameObject.GetComponent(type);
            Modules[type] = false;
            component?.OnDisable();
        }
    }
}