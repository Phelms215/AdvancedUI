using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedUI.Definitions;
using Kitchen;
using Kitchen.Modules;
using UnityEngine;

namespace AdvancedUI.UI;

public class UIStateMenu<T> : UIOptionsMenu<T> {

    public UIStateMenu(Transform container, ModuleList moduleList) : base(container, moduleList)
    {
    } 
    
    public override void Setup(int playerID) {
        AddLabel(Keys.Headings.UIStateMenu);
        New<SpacerElement>();
        UIStateOption(Keys.Preferences.CoinState, Keys.Labels.CoinState);
        UIStateOption(Keys.Preferences.GroupState, Keys.Labels.GroupState);
        UIStateOption(Keys.Preferences.MenuState, Keys.Labels.MenuState);
        UIStateOption(Keys.Preferences.TimeState, Keys.Labels.TimeState);
        UIStateOption(Keys.Preferences.DayState, Keys.Labels.DayState);
        UIStateOption(Keys.Preferences.PlayerListState, Keys.Labels.PlayerListState);
        UIStateOption(Keys.Preferences.InfoState, Keys.Labels.InfoState);
        New<SpacerElement>();
        AddButton(this.Localisation["MENU_BACK_SETTINGS"], (Action<int>) (i => this.RequestPreviousMenu()));
    }
    
    private void UIStateOption(string preferenceKey, string label)
    {
        var option =
            new Option<UIState>(
                new List<UIState>()
                { 
                    UIState.Toggle,
                    UIState.HiddenAlways,
                    UIState.HiddenNever
                },
                UIPreferences.GetOrCreate<UIState>(preferenceKey, UIState.Toggle),
                new List<string>()
                {
                    Keys.Labels.Toggle,
                    Keys.Labels.HiddenAlways,
                    Keys.Labels.HiddenNever,
                },
                null);

        option.OnChanged += (EventHandler<UIState>)((_, f) => UIPreferences.SetValue(preferenceKey, f));
        this.AddLabel(label);
        this.AddSelect<UIState>(option);
    }
    
}