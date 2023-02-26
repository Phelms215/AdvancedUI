using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedUI.Definitions;
using Kitchen;
using Kitchen.Modules;
using UnityEngine;

namespace AdvancedUI.UI;

public class UIOptionsMenu<T> : OptionsMenu<T>
{
    
    public UIOptionsMenu(Transform container, ModuleList moduleList) : base(container, moduleList)
    {
    } 
    
    public override void Setup(int playerID)
    {
        AddLabel(Keys.Headings.UIMenu); 
        if(Utility.InKitchen())
            AddButton(Keys.Labels.AdjustPositionButton,
                (i => this.RequestSubMenu(typeof (UIAdjustPositionMenu<T>)))); 
        AddButton(Keys.Labels.PositionMenuButton,
          (i => this.RequestSubMenu(typeof (UIPositionMenu<T>))));
        AddButton(Keys.Labels.StateMenuButton,
            (i => this.RequestSubMenu(typeof (UIStateMenu<T>))));
        New<SpacerElement>();
        AddButton(this.Localisation["MENU_BACK_SETTINGS"], (Action<int>) (i => this.RequestPreviousMenu()));


    }


 
    
}