using AdvancedUI.Definitions;
using Unity.Entities;

namespace AdvancedUI.Components;

public class CElementDraggable : IComponentData
{
    public UIElements Element;
    public string PositionKey;
}