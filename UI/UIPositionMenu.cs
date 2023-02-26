using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedUI.Definitions;
using Kitchen;
using Kitchen.Modules;
using UnityEngine;

namespace AdvancedUI.UI;

public class UIPositionMenu<T> : OptionsMenu<T> {

    public UIPositionMenu(Transform container, ModuleList moduleList) : base(container, moduleList)
    {
    } 
    
    public override void Setup(int playerID)
    {
        AddLabel(Keys.Headings.UIPositionMenu);
        New<SpacerElement>();
        UIPositionOption(Keys.Preferences.CoinSettingPosition, Keys.Labels.CoinPosition);
        UIPositionOption(Keys.Preferences.GroupSettingPosition, Keys.Labels.GroupPosition);
        UIPositionOption(Keys.Preferences.MenuSettingPosition, Keys.Labels.MenuPosition);
        UIPositionOption(Keys.Preferences.TimeSettingPosition, Keys.Labels.TimePosition);
        UIPositionOption(Keys.Preferences.DaySettingPosition, Keys.Labels.DayPosition);
        UIPositionOption(Keys.Preferences.PlayerListSettingPosition, Keys.Labels.PlayerListPosition);
        UIPositionOption(Keys.Preferences.InfoSettingPosition, Keys.Labels.InfoPosition);
        New<SpacerElement>();
        AddButton(this.Localisation["MENU_BACK_SETTINGS"], (Action<int>) (i => this.RequestPreviousMenu()));
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
                UIPreferences.GetOrCreate<UIPositionTypes>(preferenceKey, UIPositionTypes.Default),
                new List<string>()
                {
                    Keys.Labels.DefaultPosition,
                    Keys.Labels.DraggablePosition,
                },
                null);

        option.OnChanged += (EventHandler<UIPositionTypes>)((_, f) => UIPreferences.SetValue(preferenceKey, f));
        this.AddLabel(label);
        this.AddSelect<UIPositionTypes>(option);
    }
 
}