using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using Unity.Serialization.Json;
using UnityEngine;

namespace AdvancedUI.Definitions;

public static class UIPreferences
{ 
    public static event EventHandler SettingsUpdated = delegate {};

    private static readonly string ConfigFile = Application.persistentDataPath + "/advancedui.properties"; 
    private static readonly Dictionary<string, object> PrefValues = new(); 

    public static T GetOrCreate<T>(string key, T defaultValue = default(T))
    {
        if (PrefValues.ContainsKey(key)) return (T)PrefValues[key];
        PrefValues.Add(key, defaultValue);
        LoadSettingFromConfig<T>(key, defaultValue);
        var thisValueResponse = GetValue<T>(key);
        SaveConfig();
        return thisValueResponse;
    }

    public static void SetValue<T>(string key, T value)
    {
        if (PrefValues.ContainsKey(key)) 
            PrefValues[key] = value;
        else 
            PrefValues.Add(key, value);
        SaveConfig();
        TriggerSettingUpdateEvent();
    }

    private static T GetValue<T>(string key) {
        return (T) PrefValues[key] ?? default(T);
    }

    private static void SaveConfig()
    {
        foreach (var preference in PrefValues)
        { 
           var jsonObj = JsonSerialization
                .FromJson<Dictionary<string, object>>(File.ReadAllText(ConfigFile));
            jsonObj[preference.Key] = preference.Value;
            var output = JsonSerialization.ToJson(jsonObj);
            File.WriteAllText(ConfigFile, output);
        }
    }
     
    private static void LoadSettingFromConfig<T>(string el, T defaultValue = default(T))
    {
        if (!File.Exists(ConfigFile)) File.WriteAllText(ConfigFile, "{}");
        var count = JsonSerialization
            .FromJson<Dictionary<string, object>>(File.ReadAllText(ConfigFile))
            .Count(i => i.Key == el);
        if (count == 0) return;
        var configSetting = JsonSerialization.FromJson<Dictionary<string, dynamic>>(File.ReadAllText(ConfigFile))
            .First(i => i.Key == el);
        if (!PrefValues.ContainsKey(el))
            PrefValues.Add(el, (T) configSetting.Value);
        else
            PrefValues[el] = (T) configSetting.Value;
    }
    
    private static void TriggerSettingUpdateEvent()
    {
        SettingsUpdated?.Invoke(null, EventArgs.Empty);
    }
 



}