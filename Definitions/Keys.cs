using UnityEngine;

namespace AdvancedUI.Definitions;

public static class Keys
{
    
    public static class Headings
    {
        public const string UIMenu = "UI Options";
        public const string UIPositionMenu = "Element Positions";
        public const string UIStateMenu = "Element Settings";
        public const string ToggleUI = "Toggle UI";

        
        public const string UIAdjustPosition = "Adjust UI Positions";
        public const string InstructionsLabelUI = "Left Click (Mouse) to Stop";  
    }
    
    public static class Labels
    {
        public const string ToggleUI = "TOGGLE_UI";
        
        public const string DefaultPosition = "Default Position";
        public const string DraggablePosition = "Draggable";
        
        public const string PositionMenuButton = "Element Position";
        public const string StateMenuButton = "Element Settings";
        public const string AdjustPositionButton = "Adjust Position";

        public const string CoinPosition = "Coin Position";
        public const string GroupPosition = "Group Position";
        public const string MenuPosition = "Menu Position";
        public const string TimePosition = "Time Position";
        public const string DayPosition = "Day Position";
        public const string PlayerListPosition = "Player List Position"; 
        public const string InfoPosition = "Version Text Position";
        
        public const string CoinState = "Coin Settings";
        public const string GroupState = "Group Settings";
        public const string MenuState = "Menu Settings";
        public const string TimeState = "Time Settings";
        public const string DayState = "Day Settings";
        public const string PlayerListState = "Player List Settings";
        public const string InfoState = "Version Settings";

        public const string Toggle = "Toggle Visiblity";
        public const string HiddenAlways = "Always Hidden";
        public const string HiddenNever = "Never Hidden"; 
    }

    public static class Preferences
    {
        public const string CoinPosition = "CoinPosition";
        public const string GroupPosition = "GroupPosition";
        public const string MenuPosition = "MenuPosition";
        public const string TimePosition = "TimePosition";
        public const string DayPosition = "DayPosition";
        public const string PlayerListPosition = "PlayerListPosition"; 
        public const string InfoPosition = "InfoPosition"; 

        public const string ButtonKeyBind = "ButtonKeybind";
        public const string KeyboardKeyBind = "ButtonKeybind";
         
        public const string CoinState = "CoinState";
        public const string GroupState = "GroupState";
        public const string MenuState = "MenuState";
        public const string TimeState = "TimeState";
        public const string DayState = "DayState";
        public const string PlayerListState = "PlayerListState";
        public const string InfoState = "InfoState";
        
        public const string CoinSettingPosition = "CoinSettingPosition";
        public const string GroupSettingPosition = "GroupSettingPosition";
        public const string MenuSettingPosition = "MenuSettingPosition";
        public const string TimeSettingPosition = "TimeSettingPosition";
        public const string DaySettingPosition = "DaySettingPosition";
        public const string PlayerListSettingPosition = "PlayerListSettingPosition"; 
        public const string InfoSettingPosition = "InfoSettingPosition"; 
        
    }

    public static class Positions
    {
        public static readonly Vector3 CoinPosition = new Vector3(0.0f, 1f, 0.0f);
        public static readonly Vector3 GroupPosition = new Vector3(0.0f, 1f, 0.0f);
        public static readonly Vector3 MenuPosition = new Vector3(0.0f, 1f, 0.0f);
        public static readonly Vector3 TimePosition = new Vector3(0.5f, 1f, 0.0f);
        public static readonly Vector3 DayPosition = new Vector3(1f, 1f, 0.0f);
        public static readonly Vector3 PlayerListPosition = new Vector3(0.5f, 0.0f, 0.0f);
        public static readonly Vector3 VersionTextPosition = new Vector3(1f, 0.0f, 0.0f);
    }
    
}