using System;
using System.Collections.Generic;
using AdvancedUI.Definitions;
using HarmonyLib; 
using AdvancedUI.UI;
using Kitchen;
using Kitchen.Modules; 
using UnityEngine;

namespace AdvancedUI.Patches
{ 
    [HarmonyPatch(typeof(OptionsMenu<PauseMenuAction>), "Setup")]
    internal class OptionsMenuPatch
    {

        [HarmonyPrefix]
        private static void Prefix(OptionsMenu<PauseMenuAction> __instance)
        {

            AccessTools.Method(__instance.GetType(), "AddButton").Invoke(__instance, new object[]
            {
                "UI Options",
                (Action<int>)(i =>
                    AccessTools.Method(__instance.GetType(), "RequestSubMenu")
                        .Invoke(__instance,
                            new object[] { typeof(UIOptionsMenu<PauseMenuAction>), false })),
                0, // arg
                1, // scale
                0.2f
            });
        }
    }

    [HarmonyPatch(typeof(OptionsMenu<PauseMenuAction>), "CreateSubmenus")]
    internal class AddMenuPatch
    {
        [HarmonyPostfix]
        private static void Postfix(OptionsMenu<PauseMenuAction> __instance,
            ref Dictionary<System.Type, Menu<PauseMenuAction>> menus)
        {
            var moduleList = (ModuleList)AccessTools.Field(__instance.GetType(), "ModuleList").GetValue(__instance);
            var container = (Transform)AccessTools.Field(__instance.GetType(), "Container").GetValue(__instance); 
            menus.Add(typeof(UIOptionsMenu<PauseMenuAction>), new UIOptionsMenu<PauseMenuAction>(container, moduleList));
            menus.Add(typeof(UIPositionMenu<PauseMenuAction>), new UIPositionMenu<PauseMenuAction>(container, moduleList));
            menus.Add(typeof(UIAdjustPositionMenu<PauseMenuAction>), new UIAdjustPositionMenu<PauseMenuAction>(container, moduleList));
            menus.Add(typeof(UIStateMenu<PauseMenuAction>), new UIStateMenu<PauseMenuAction>(container, moduleList));  

        }
        
    } 
}