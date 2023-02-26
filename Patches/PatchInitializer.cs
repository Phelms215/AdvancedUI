using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace AdvancedUI.Patches;

public class PatchInitializer : MonoBehaviour
{
    private readonly Harmony _harmony = new("com.networkglitch.advancedui");

    private void Awake()
    {
        _harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}