using System;
using AdvancedUI.Definitions;
using Unity.Entities;
using UnityEngine;

namespace AdvancedUI.Components;

public class CElementUIOption : IComponentData {
    public Vector3 Position;
    public UIState State;
    public UIPositionTypes PositionSetting; 
}