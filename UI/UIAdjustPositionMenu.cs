using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedUI.Definitions;
using Kitchen;
using Kitchen.Modules;
using UnityEngine;

namespace AdvancedUI.UI;

public class UIAdjustPositionMenu<T> : OptionsMenu<PauseMenuAction>
{

    public UIAdjustPositionMenu(Transform container, ModuleList moduleList) : base(container, moduleList)
    {
    }

    public override void Setup(int playerID)
    {
        AddLabel(Keys.Headings.UIAdjustPosition);
        New<SpacerElement>();

        if (ElementMoveAble(Keys.Preferences.CoinSettingPosition) && !AdvancedUI.IsHidden(UIElements.MoneyDisplay))
            UIPositionOption(UIElements.MoneyDisplay, Keys.Labels.CoinPosition, Keys.Preferences.CoinPosition);
        
        if (ElementMoveAble(Keys.Preferences.GroupSettingPosition) && !AdvancedUI.IsHidden(UIElements.GroupDisplay))
            UIPositionOption(UIElements.GroupDisplay, Keys.Labels.GroupPosition, Keys.Preferences.GroupPosition);
        
        if (ElementMoveAble(Keys.Preferences.MenuSettingPosition) && !AdvancedUI.IsHidden(UIElements.MenuDisplay))
            UIPositionOption(UIElements.MenuDisplay, Keys.Labels.MenuPosition, Keys.Preferences.MenuPosition);
        
        if (ElementMoveAble(Keys.Preferences.TimeSettingPosition) && !AdvancedUI.IsHidden(UIElements.TimeDisplay))
            UIPositionOption(UIElements.TimeDisplay, Keys.Labels.TimePosition, Keys.Preferences.TimePosition);
        
        if (ElementMoveAble(Keys.Preferences.DaySettingPosition) && !AdvancedUI.IsHidden(UIElements.DayDisplay))
            UIPositionOption(UIElements.DayDisplay, Keys.Labels.DayPosition, Keys.Preferences.DayPosition);
        
        if (ElementMoveAble(Keys.Preferences.PlayerListSettingPosition) && !AdvancedUI.IsHidden(UIElements.PlayerListDisplay))
            UIPositionOption(UIElements.PlayerListDisplay, Keys.Labels.PlayerListPosition,
                Keys.Preferences.PlayerListPosition);
        
        if (ElementMoveAble(Keys.Preferences.InfoSettingPosition) && !AdvancedUI.IsVersionHidden())
            UIPositionOption(UIElements.VersionTextDisplay, Keys.Labels.InfoPosition, Keys.Preferences.InfoPosition);
        
        New<SpacerElement>();
        AddLabel(Keys.Headings.InstructionsLabelUI); 
        New<SpacerElement>();
        AddButton(this.Localisation["MENU_BACK_SETTINGS"], (Action<int>)(i => this.RequestPreviousMenu()));
    }

    private void UIPositionOption(UIElements thisElementType, string label, string positionKey)
    {
        AddButton(label, (Action<int>)(i => HandleDraggable(thisElementType, positionKey)));
    }

    private void HandleDraggable(UIElements thisElement, string positionKey)
    {
        AdvancedUI.MarkElementDraggable(thisElement, positionKey);
        RequestAction(PauseMenuAction.CloseMenu);
    }

    private bool ElementMoveAble(string settingKey)
    {
        var settingValue = UIPreferences.GetOrCreate<UIPositionTypes>(settingKey, UIPositionTypes.Default);
        return (settingValue == UIPositionTypes.Draggable);
    }

}