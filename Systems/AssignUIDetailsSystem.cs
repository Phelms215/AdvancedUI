using System;
using System.Linq;
using AdvancedUI.Components;
using AdvancedUI.Definitions;
using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine; 

namespace AdvancedUI.Systems;

public class AssignUIDetailsSystem : GenericSystemBase, IModSystem
{
    private EntityQuery _toggleQuery;
    private EntityQuery _moneyDisplayQuery;
    private EntityQuery _groupDisplayQuery;
    private EntityQuery _timeDisplayQuery;
    private EntityQuery _dayDisplayQuery;
    private EntityQuery _playerInfoDisplayQuery;
    private EntityQuery _menuDisplayQuery;

    protected override void Initialise()
    {
        _moneyDisplayQuery = CreateQuery(typeof(CMoneyDisplay));
        _groupDisplayQuery = CreateQuery(typeof(CParametersDisplay));
        _timeDisplayQuery = CreateQuery(typeof(CTimeDisplay));
        _dayDisplayQuery = CreateQuery(typeof(CDayDisplay));
        _menuDisplayQuery = CreateQuery(typeof(CTwitchOrderOption));
        _playerInfoDisplayQuery = CreateQuery(typeof(SPlayerInfoManager));
        _toggleQuery = GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CToggleState)));
        
        UIPreferences.SettingsUpdated += new EventHandler(SettingUpdatedNotification);
    }

    protected override void OnUpdate()
    {
        if (!Utility.InKitchen()) return;
        HandleQuery(_moneyDisplayQuery, new QueryParams()
        {
            PositionSettingKey = Keys.Preferences.CoinSettingPosition,
            PositionVectorKey = Keys.Preferences.CoinPosition,
            DefaultPosition = Keys.Positions.CoinPosition,
            StateKey = Keys.Preferences.CoinState,
        });
        HandleQuery(_groupDisplayQuery, new QueryParams()
        {
            PositionSettingKey = Keys.Preferences.GroupSettingPosition,
            PositionVectorKey = Keys.Preferences.GroupPosition,
            DefaultPosition = Keys.Positions.GroupPosition,
            StateKey = Keys.Preferences.GroupState,
        });
        HandleQuery(_timeDisplayQuery, new QueryParams()
        {
            PositionSettingKey = Keys.Preferences.TimeSettingPosition,
            PositionVectorKey = Keys.Preferences.TimePosition,
            DefaultPosition = Keys.Positions.TimePosition,
            StateKey = Keys.Preferences.TimeState,
        });
        HandleQuery(_dayDisplayQuery, new QueryParams()
        {
            PositionSettingKey = Keys.Preferences.DaySettingPosition,
            PositionVectorKey = Keys.Preferences.DayPosition,
            DefaultPosition = Keys.Positions.DayPosition,
            StateKey = Keys.Preferences.DayState,
        });
        HandleQuery(_playerInfoDisplayQuery, new QueryParams()
        {
            PositionSettingKey = Keys.Preferences.PlayerListSettingPosition,
            PositionVectorKey = Keys.Preferences.PlayerListPosition,
            DefaultPosition = Keys.Positions.PlayerListPosition,
            StateKey = Keys.Preferences.PlayerListState,
        });
        HandleQuery(_menuDisplayQuery, new QueryParams()
        {
            PositionSettingKey = Keys.Preferences.MenuSettingPosition,
            PositionVectorKey = Keys.Preferences.MenuPosition,
            DefaultPosition = Keys.Positions.MenuPosition,
            StateKey = Keys.Preferences.MenuState,
        });
        
        //
        // Version Info Is Annoying... handle it on its own 
        HandleVersionInfo();

    }

    private void HandleVersionInfo(bool update = false)
    {
        Entity? versionInfoEntity = null;
        var versionInfoQuery = GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CRequiresView)).None((ComponentType)typeof(CElementUIOption)));
        if(update)
            versionInfoQuery = GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CRequiresView)));
        foreach (var queryItem in versionInfoQuery.ToEntityArray(Allocator.Temp).Where(queryItem =>
                     EntityManager.GetComponentData<CRequiresView>(queryItem).Type == ViewType.VersionOverlay))
        {
            versionInfoEntity = queryItem;
        }
        if (!versionInfoEntity.HasValue) return;


        var currentPosition = UIPreferences.GetOrCreate<Vector3>(Keys.Preferences.InfoPosition, Keys.Positions.VersionTextPosition);
        var currentSettings = UIPreferences.GetOrCreate<UIPositionTypes>(Keys.Preferences.InfoSettingPosition, UIPositionTypes.Default);
       
        HandleUIDataUpdate(versionInfoEntity.Value, new QueryParams() {
            PositionSettingKey = Keys.Preferences.InfoSettingPosition,
            PositionVectorKey = Keys.Preferences.InfoPosition,
            DefaultPosition = Keys.Positions.VersionTextPosition,
            StateKey = Keys.Preferences.InfoState,
        });
        HandleStateUpdate(Keys.Preferences.InfoState,versionInfoEntity.Value); 
        MarkUpdated(versionInfoEntity.Value); 
    }
    private void SettingUpdatedNotification(object thisObj, EventArgs eventArgs)
    {
        if (!Utility.InKitchen()) return;
        HandleQuery(CreateQuery(typeof(CMoneyDisplay), true),
            new QueryParams()
            {
                PositionSettingKey = Keys.Preferences.CoinSettingPosition,
                PositionVectorKey = Keys.Preferences.CoinPosition,
                DefaultPosition = Keys.Positions.CoinPosition,
                StateKey = Keys.Preferences.CoinState,
            });
        HandleQuery(CreateQuery(typeof(CParametersDisplay), true),
            new QueryParams()
            {
                PositionSettingKey = Keys.Preferences.GroupSettingPosition,
                PositionVectorKey = Keys.Preferences.GroupPosition,
                DefaultPosition = Keys.Positions.GroupPosition,
                StateKey = Keys.Preferences.GroupState,
            });
        HandleQuery(CreateQuery(typeof(CTimeDisplay), true),
            new QueryParams()
            {
                PositionSettingKey = Keys.Preferences.TimeSettingPosition,
                PositionVectorKey = Keys.Preferences.TimePosition,
                DefaultPosition = Keys.Positions.TimePosition,
                StateKey = Keys.Preferences.TimeState,
            });
        HandleQuery(CreateQuery(typeof(CDayDisplay), true),
            new QueryParams()
            {
                PositionSettingKey = Keys.Preferences.DaySettingPosition,
                PositionVectorKey = Keys.Preferences.DayPosition,
                DefaultPosition = Keys.Positions.DayPosition,
                StateKey = Keys.Preferences.DayState,
            });
        HandleQuery(CreateQuery(typeof(CTwitchOrderOption), true),
            new QueryParams()
            {
                PositionSettingKey = Keys.Preferences.MenuSettingPosition,
                PositionVectorKey = Keys.Preferences.MenuPosition,
                DefaultPosition = Keys.Positions.MenuPosition,
                StateKey = Keys.Preferences.MenuState,
            });
        HandleQuery(CreateQuery(typeof(SPlayerInfoManager), true),
            new QueryParams()
            {
                PositionSettingKey = Keys.Preferences.PlayerListSettingPosition,
                PositionVectorKey = Keys.Preferences.PlayerListPosition,
                DefaultPosition = Keys.Positions.PlayerListPosition,
                StateKey = Keys.Preferences.PlayerListState,
            });
        HandleVersionInfo(true);
    }

    private void HandleQuery(EntityQuery entityQuery, QueryParams queryParams)
    {
        if (entityQuery.IsEmpty) return;
        foreach (var thisQuery in entityQuery.ToEntityArray(Allocator.Temp))
        {
            HandleUIDataUpdate(thisQuery, queryParams);
            HandleStateUpdate(queryParams.StateKey, thisQuery);
            MarkUpdated(thisQuery);
        }
    }

    private void HandleUIDataUpdate(Entity thisEntity, QueryParams thisQuery)
    {
        var currentPosition =
            UIPreferences.GetOrCreate<Vector3>(thisQuery.PositionVectorKey, thisQuery.DefaultPosition);

        var thisPosition = currentPosition;
        if (EntityManager.HasComponent<CElementUIOption>(thisEntity))
        {
            var currentSettings = EntityManager.GetComponentData<CElementUIOption>(thisEntity);
            thisPosition = currentSettings.PositionSetting is UIPositionTypes.Default
                ? thisQuery.DefaultPosition
                : currentPosition;
        }
        else
        {
            EntityManager.AddComponent<CElementUIOption>(thisEntity);
        }

        EntityManager.SetComponentData<CElementUIOption>(thisEntity, new CElementUIOption()
        {
            State = UIPreferences.GetOrCreate<UIState>(thisQuery.StateKey, UIState.Toggle),
            Position = thisPosition,
            PositionSetting =
                UIPreferences.GetOrCreate<UIPositionTypes>(thisQuery.PositionSettingKey, UIPositionTypes.Default),
        });
    }

    private EntityQuery CreateQuery(Type componentType, bool includeAll = false)
    {
        var query = new EntityQueryDesc
        {
            None = (includeAll ? new ComponentType[] { } : new ComponentType[] { typeof(CElementUIOption) }),
            All = new ComponentType[] { componentType }
        };
        return GetEntityQuery(query);
    }

    private void MarkUpdated(Entity thisEntity) {
        // Everyone got updated
        EntityManager.AddComponentData<CElementUpdated>(thisEntity, new CElementUpdated());
    }
    
    
    private void HandleStateUpdate(string stateKey, Entity thisEntity)
    {

        if (UIPreferences.GetOrCreate<UIState>(stateKey, UIState.Toggle) == UIState.Toggle)
        {
            EntityManager.AddComponentData<CElementToggles>(thisEntity, new CElementToggles());
        }
        else
        {
            if (EntityManager.HasComponent<CElementToggles>(thisEntity))
                EntityManager.RemoveComponent<CElementToggles>(thisEntity);
        }
    }

    private struct QueryParams
    {
        public string PositionSettingKey;
        public string PositionVectorKey;
        public Vector3 DefaultPosition;
        public string StateKey; 

    }
}