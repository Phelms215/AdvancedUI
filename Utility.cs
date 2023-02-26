using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdvancedUI.Definitions;
using Controllers;
using JetBrains.Annotations;
using Kitchen;
using Unity.Serialization.Json;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace AdvancedUI;

public static class Utility
{
    private static readonly string ConfigFile = Application.persistentDataPath + "/advancedui.config";

    public static void Log(string message)
    {
        Debug.Log("[" + AdvancedUI.ModID + "] " + DateTime.Now + " - " + message);
    } 

    public static bool InKitchen()
    {
        return GameInfo.CurrentScene == SceneType.Kitchen;
    }

    public static bool InLobby()
    {
        return GameInfo.CurrentScene == SceneType.Franchise;
    }

    public static bool EnteredLobby(AdvancedUI instance)
    {
        if (!instance.HasSingleton<SCreateScene>()) return false;
        return instance.GetSingleton<SCreateScene>().Type == SceneType.Franchise;
    }

    public static ControllerType PlayerControlType()
    {
        var thisPlayer = Players.Main.All().First(i => i.IsLocalUser);
        return Session.InputSource.GetCurrentController(thisPlayer.ID);
    }

    public static string FirstLetterToUpper(string str)
    {
        if (str == null)
            return null;

        if (str.Length > 1)
            return char.ToUpper(str[0]) + str.Substring(1);

        return str.ToUpper();
    }
    
    public static GamepadButton GetButton(ButtonControl thisButton)
    { 
        return thisButton.name switch
        {
            "buttonNorth" => GamepadButton.North,
            "buttonSouth" => GamepadButton.South,
            "buttonEast" => GamepadButton.East,
            "buttonWest" => GamepadButton.West,
            "leftTrigger" => GamepadButton.LeftTrigger,
            "leftShoulder" => GamepadButton.LeftShoulder,
            "rightTrigger" => GamepadButton.RightTrigger,
            "rightShoulder" => GamepadButton.RightShoulder,
            "leftStick" => GamepadButton.LeftStick,
            "rightStick" => GamepadButton.RightStick,
            "select" => GamepadButton.Select,
            "start" => GamepadButton.Start,
            "dpadUp" => GamepadButton.DpadUp,
            "dpadDown" => GamepadButton.DpadDown,
            "dpadLeft" => GamepadButton.DpadLeft,
            "dpadRight" => GamepadButton.DpadRight,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}