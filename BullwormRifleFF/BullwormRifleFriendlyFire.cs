using System.Linq;
using BepInEx;
using BepInEx.Logging;
using CessilCellsCeaChells.CeaChore;
using HarmonyLib;
using UnityEngine;

[assembly: RequiresMethod(typeof(PardnerEquipment_BullwormRifle), nameof(PardnerEquipment_BullwormRifle.Awake), typeof(void))]
[assembly: RequiresField(typeof(ConnectionPayload), nameof(ConnectionPayload.versionBullworkRifleFF), typeof(string), true)]

namespace BullwormRifleFF;

[BepInAutoPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public partial class BullwormRifleFriendlyFire : BaseUnityPlugin {
    public static new ManualLogSource? Logger { get; private set; }
    public static Harmony Patcher { get; } = new(MyPluginInfo.PLUGIN_GUID); 
    public void Awake()
    {
        Logger = base.Logger;

        Patcher.PatchAll();
        
        int patchedMethodCount;
        Logger.LogDebug($"Successfully patched {(patchedMethodCount = Patcher.GetPatchedMethods().Count())} method{(patchedMethodCount == 1 ? "" : "s")}.");
    }
}