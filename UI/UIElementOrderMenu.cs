using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedUI.Definitions;
using Kitchen;
using Kitchen.Modules;
using UnityEngine;

namespace AdvancedUI.UI;

public class UIElementOrderMenu<T> : UIOptionsMenu<T> {

    public UIElementOrderMenu(Transform container, ModuleList moduleList) : base(container, moduleList)
    {
    } 
    
    public override void Setup(int playerID) {
        
        UIPositionOption("CoinSettingPosition", "Coin Position");
        UIPositionOption("GroupSettingPosition", "Group Position");
        UIPositionOption("MenuSettingPosition", "Menu Position");
        UIPositionOption("TimeSettingPosition", "Time Position");
        UIPositionOption("DaySettingPosition", "Day Position");
        UIPositionOption("PlayerListSettingPosition", "Player List Position");
        UIPositionOption("InfoSettingPosition", "Info Position");
    }
    
    private void UIPositionOption(string preferenceKey, string label)
    {
        var option =
            new Option<UIPositionTypes>(
                new List<UIPositionTypes>()
                {
                    UIPositionTypes.Default,
                    UIPositionTypes.Draggable
                },
                UIPreferences.GetOrCreate<UIPositionTypes>(preferenceKey),
                new List<string>()
                {
                    Keys.Labels.DefaultPosition,
                    Keys.Labels.DraggablePosition
                },
                null);

        option.OnChanged += (EventHandler<UIPositionTypes>)((_, f) => UIPreferences.SetValue(preferenceKey, f));
        this.AddLabel(label);
        this.AddSelect<UIPositionTypes>(option);
    }
    
}