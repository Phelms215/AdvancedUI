using System;
using System.Linq;
using AdvancedUI.Components;
using AdvancedUI.Definitions;
using AdvancedUI.Patches; 
using Kitchen; 
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace AdvancedUI
{
    public class AdvancedUI : GenericSystemBase, IModSystem
    {
        private const string ModVersion = "1.1.0";
        public const string ModID = "AdvancedUI";
        private static AdvancedUI _ui;

        protected override void Initialise()
        {
            Utility.Log("Initializing Version " + ModVersion);
            if (GameObject.FindObjectOfType<PatchInitializer>() != null) return;
            var patchObject = new GameObject("AdvancedUI").AddComponent<PatchInitializer>();
            GameObject.DontDestroyOnLoad(patchObject);
            _ui = this;
        }

        protected override void OnUpdate() { }

        private static EntityQuery _draggableEntityQuery;

        public static void MarkElementDraggable(UIElements thisElement, string positionKey)
        {
            ClearDraggable();
            switch (thisElement)
            {
                case UIElements.MoneyDisplay:
                    _draggableEntityQuery =
                        _ui.GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CMoneyDisplay)));
                    break;
                case UIElements.GroupDisplay:
                    _draggableEntityQuery =
                        _ui.GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CParametersDisplay)));
                    break;
                case UIElements.MenuDisplay:
                    _draggableEntityQuery =
                        _ui.GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CTwitchOrderOption)));
                    break;
                case UIElements.TimeDisplay:
                    _draggableEntityQuery =
                        _ui.GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CTimeDisplay)));
                    break;
                case UIElements.DayDisplay:
                    _draggableEntityQuery =
                        _ui.GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CDayDisplay)));
                    break;
                case UIElements.PlayerListDisplay:
                    _draggableEntityQuery =
                        _ui.GetEntityQuery(new QueryHelper().All((ComponentType)typeof(SPlayerInfoManager)));
                    break;
                case UIElements.VersionTextDisplay:
                    HandleVersionDraggable();
                    return; 
                default:
                    throw new ArgumentOutOfRangeException(nameof(thisElement), thisElement, null);
            }

            if (_draggableEntityQuery.IsEmpty) return;
            var thisEntity = _draggableEntityQuery.First();
            _ui.EntityManager.AddComponentData<CElementDraggable>(thisEntity, new CElementDraggable()
            {
                PositionKey = positionKey,
                Element = thisElement
            });
        }

        private static void ClearDraggable()
        {
            var thisQuery = _ui.GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CElementDraggable)));
            if (thisQuery.IsEmpty) return;
            _ui.EntityManager.RemoveComponent<CElementDraggable>(thisQuery.First());
        }

        private static void HandleVersionDraggable()
        {
            Entity? versionInfoEntity = null;
            var versionInfoQuery = _ui.GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CRequiresView)));
            foreach (var queryItem in versionInfoQuery.ToEntityArray(Allocator.Temp).Where(queryItem =>
                         _ui.EntityManager.GetComponentData<CRequiresView>(queryItem).Type == ViewType.VersionOverlay))
            {
                versionInfoEntity = queryItem;
            }

            if (!versionInfoEntity.HasValue) return;
            _ui.EntityManager.AddComponentData<CElementDraggable>(versionInfoEntity.Value, new CElementDraggable()
            {
                PositionKey = Keys.Preferences.InfoPosition,
                Element = UIElements.VersionTextDisplay
            });
        }
        
        public static bool IsVersionHidden()
        {
            Entity? versionInfoEntity = null;
            var versionInfoQuery = _ui.GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CRequiresView)));
            foreach (var queryItem in versionInfoQuery.ToEntityArray(Allocator.Temp).Where(queryItem =>
                         _ui.EntityManager.GetComponentData<CRequiresView>(queryItem).Type == ViewType.VersionOverlay))
            {
                versionInfoEntity = queryItem;
            }

            if (!versionInfoEntity.HasValue) return false;
            if (!_ui.EntityManager.HasComponent<CPosition>(versionInfoEntity.Value)) return false;
            return (_ui.EntityManager.GetComponentData<CPosition>(versionInfoEntity.Value).Position == CPosition.Hidden);
        }

        public static bool IsHidden(UIElements thisElement)
        {
            var thisEntity = GetElementEntity(thisElement);
            if (!thisEntity.HasValue) return true;
            return (_ui.EntityManager.GetComponentData<CPosition>(thisEntity.Value).Position == CPosition.Hidden);
        }

        private static EntityQuery _thisQuery;

        private static Entity? GetElementEntity(UIElements thisElement)
        {
            _thisQuery = thisElement switch
            {
                UIElements.MoneyDisplay => CreateQuery(typeof(CMoneyDisplay)),
                UIElements.DayDisplay => CreateQuery(typeof(CDayDisplay)),
                UIElements.TimeDisplay => CreateQuery(typeof(CTimeDisplay)),
                UIElements.GroupDisplay => CreateQuery(typeof(CParametersDisplay)),
                UIElements.PlayerListDisplay => CreateQuery(typeof(SPlayerInfoManager)), 
                _ => _thisQuery
            };

            if (_thisQuery.IsEmpty) return null;
            return _thisQuery.First();
        }

        private static EntityQuery CreateQuery(Type componentType)
        {
            var query = new EntityQueryDesc
            { 
                All = new ComponentType[] { componentType }
            };
            return _ui.GetEntityQuery(query);
        }
    }
}