using System; 
using AdvancedUI.Components;
using AdvancedUI.Definitions;
using Kitchen;
using KitchenMods; 
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace AdvancedUI.Systems;

public class HandleUIDraggableSystem : GenericSystemBase, IModSystem
{
    private EntityQuery _elementQuery;  
    
    protected override void Initialise()
    {
        _elementQuery = GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CElementDraggable),
            (ComponentType)typeof(CElementUIOption)));
        RequireForUpdate(_elementQuery);

    }

    protected override void OnUpdate()
    {
        if (_elementQuery.IsEmpty) return;
        var entity = _elementQuery.First();
        
        var elementData = EntityManager.GetComponentData<CElementUIOption>(entity);
        var draggableData = EntityManager.GetComponentData<CElementDraggable>(entity); 
        Vector3 worldPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition); 
        
        elementData.Position = worldPosition;
        EntityManager.SetComponentData<CPosition>(entity, new CPosition(worldPosition));
        EntityManager.SetComponentData<CElementUIOption>(entity, elementData);

        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        EntityManager.RemoveComponent<CElementDraggable>(entity);  
        UIPreferences.SetValue<Vector3>(draggableData.PositionKey, worldPosition);

    }
}