using System;
using System.Linq;
using AdvancedUI.Components;
using AdvancedUI.Definitions;
using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using ComponentType = Unity.Entities.ComponentType;

namespace AdvancedUI.Systems;

public class HandleToggleRequestSystem : GenericSystemBase, IModSystem
{
    private IDisposable _gamepadListener;
    private IDisposable _keyboardListener;

    private EntityQuery _toggleQuery;
    private EntityQuery _elementToggles;
    private Entity _toggleStateEntity;


    protected override void Initialise()
    { 
        _toggleQuery = GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CToggleState)));
        _elementToggles = GetEntityQuery(new QueryHelper().All((ComponentType)typeof(CElementToggles))); 
    }

    protected override void OnUpdate()
    {
        if (!Utility.InKitchen()) return;
        if (_toggleQuery.IsEmpty)
        {
            _toggleStateEntity = EntityManager.CreateEntity((ComponentType)typeof(CToggleState));
            EntityManager.SetComponentData<CToggleState>(_toggleStateEntity, new CToggleState());
        }

        _gamepadListener ??= InputSystem.onEvent
            .ForDevice<Gamepad>()
            .Where(e => e.HasButtonPress())
            .Call(eventPtr =>
            {
                
                var thisButton = (ButtonControl)eventPtr.EnumerateChangedControls().First();
                if (Utility.GetButton(thisButton) !=
                    UIPreferences.GetOrCreate<GamepadButton>(Keys.Preferences.ButtonKeyBind, GamepadButton.Select))
                    return;
                ToggleUI();
            });

        _keyboardListener ??= InputSystem.onEvent
            .ForDevice<Keyboard>()
            .Where(e => e.HasButtonPress())
            .Call(eventPtr =>
            {
                var thisButton = (KeyControl)eventPtr.EnumerateChangedControls().First();
                var value = (Key)Enum.Parse(typeof(Key), Utility.FirstLetterToUpper(thisButton.name));
                if (value != UIPreferences.GetOrCreate<Key>(Keys.Preferences.KeyboardKeyBind, Key.Tab)) return;
                ToggleUI();
            });
    }

    private void ToggleUI()
    {
        if (_toggleQuery.IsEmpty) return;
        var currentValue = EntityManager.GetComponentData<CToggleState>(_toggleQuery.First());
        currentValue.IsVisible = !currentValue.IsVisible; 
        EntityManager.SetComponentData<CToggleState>(_toggleQuery.First(), currentValue);
        foreach (var element in _elementToggles.ToEntityArray(Allocator.Temp)) {
            EntityManager.AddComponent<CElementUpdated>(element);
        }
    }
}