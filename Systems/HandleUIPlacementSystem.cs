using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedUI.Components;
using AdvancedUI.Definitions;
using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace AdvancedUI.Systems;
 
[UpdateAfter(typeof(AssignUIDetailsSystem))]
public class HandleUIPlacementSystem : GenericSystemBase, IModSystem
{
    private EntityQuery _toggleQuery;
    private EntityQuery _elementQuery;

    protected override void Initialise()
    {
        _toggleQuery = GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CToggleState)));
        _elementQuery = GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CElementUIOption),(ComponentType)typeof(CElementUpdated))); 
    }

    protected override void OnUpdate()
    {
        if (!Utility.InKitchen()) return;
        if (_elementQuery.IsEmpty) return; 
        foreach (var element in _elementQuery.ToEntityArray(Allocator.Temp))
            HandleElementPosition(element);
    }

    private void HandleElementPosition(Entity element)
    {
        var thisElementData = EntityManager.GetComponentData<CElementUIOption>(element); 
        switch (thisElementData.State)
        {
            case UIState.HiddenAlways:
                if (IsHidden(element)) break;
                HideEntity(element);
                break; 
            case UIState.Toggle:
                if (EntityManager.HasComponent<CElementToggles>(element)) HandleToggle(element);
                break;
            case UIState.HiddenNever:
                SetPosition(element, thisElementData.Position);
                break;
        }
        EntityManager.RemoveComponent<CElementUpdated>(element);

    }

    private void HandleToggle(Entity element)
    {
        if (_toggleQuery.IsEmpty) return;
        var toggleState = EntityManager.GetComponentData<CToggleState>(_toggleQuery.First());
        var thisElementData = EntityManager.GetComponentData<CElementUIOption>(element);
        if (toggleState.IsVisible)
            SetPosition(element, thisElementData.Position);
        else
            HideEntity(element);

    }

    private void HideEntity(Entity thisEntity)
    {
        EntityManager.SetComponentData<CPosition>(thisEntity, CPosition.Hidden); 
    }

    private void SetPosition(Entity thisEntity, Vector3 thisPosition)
    {
        EntityManager.SetComponentData<CPosition>(thisEntity, new CPosition(thisPosition));
    }
    
    private Vector3 GetPosition(Entity thisEntity)
    {
        return EntityManager.GetComponentData<CPosition>(thisEntity).Position;
    }
    private bool IsHidden(Entity thisEntity)
    {
        return (EntityManager.GetComponentData<CPosition>(thisEntity).Position == CPosition.Hidden);
    }

}