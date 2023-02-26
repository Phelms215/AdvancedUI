using System;
using System.Linq;
using AdvancedUI.Definitions;
using HarmonyLib;
using Kitchen;
using Kitchen.Modules;
using KitchenData; 
using Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;


namespace AdvancedUI.Patches
{
    [HarmonyPatch(typeof(ControlRebindElement), "Setup")]
    internal class RebindPatch
    {

        [HarmonyPostfix]
        private static void Postfix(ControlRebindElement __instance)
        {
            if (!GameData.Main.GlobalLocalisation.Text.ContainsKey(Keys.Labels.ToggleUI))
                GameData.Main.GlobalLocalisation.Text.Add(Keys.Labels.ToggleUI, Keys.Headings.ToggleUI);
            AccessTools.Method(__instance.GetType(), "AddRebindOption").Invoke(__instance, new object[]
            {
                Keys.Labels.ToggleUI,
                Keys.Labels.ToggleUI
            });
            AccessTools.Method(__instance.GetType(), "EndRebind").Invoke(__instance, null);
        }
    }

    [HarmonyPatch(typeof(ControlRebindElement), "TriggerRebind")]
    internal class RebindActionPatch
    {
        private static bool Prefix(ControlRebindElement __instance, ref string action)
        {
            if (action != Keys.Labels.ToggleUI) return true;
            Utility.Log(action);
            UpdateKeyBind(__instance);
            return false;
        }

        private static void UpdateKeyBind(ControlRebindElement instance)
        {
            if (Utility.PlayerControlType() is ControllerType.Playstation or ControllerType.Xbox)
            {
                InputSystem.onEvent
                    .ForDevice<Gamepad>()
                    .Where(e => e.HasButtonPress())
                    .CallOnce(eventPtr =>
                    {
                        var thisButton = (ButtonControl)eventPtr.EnumerateChangedControls().First();
                        var currentButton = UIPreferences.GetOrCreate<GamepadButton>(Keys.Preferences.ButtonKeyBind, GamepadButton.Select);
                        if(currentButton != Utility.GetButton(thisButton))
                            UIPreferences.SetValue(Keys.Preferences.ButtonKeyBind, Utility.GetButton(thisButton));
                        
                        AccessTools.Method(instance.GetType(), "EndRebind").Invoke(instance, null);
                    });
            }

            if (Utility.PlayerControlType() is ControllerType.Keyboard)
            {
                InputSystem.onEvent
                    .ForDevice<Keyboard>()
                    .Where(e => e.HasButtonPress())
                    .CallOnce(eventPtr =>
                    {
                        var thisButton = (KeyControl)eventPtr.EnumerateChangedControls().First();
                        var value = (Key)Enum.Parse(typeof(Key), Utility.FirstLetterToUpper(thisButton.name));
                        UIPreferences.GetOrCreate<Key>(Keys.Preferences.KeyboardKeyBind, value);

                        var currentButton = UIPreferences.GetOrCreate<Key>(Keys.Preferences.KeyboardKeyBind, Key.Tab);
                        if (currentButton != value)
                            UIPreferences.SetValue(Keys.Preferences.KeyboardKeyBind, value);

                        AccessTools.Method(instance.GetType(), "EndRebind").Invoke(instance, null);
                    });
            }

            if (Utility.PlayerControlType() is ControllerType.None)
                AccessTools.Method(instance.GetType(), "EndRebind").Invoke(instance, null);
        }


    }
}