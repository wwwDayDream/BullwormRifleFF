using System;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using CessilCellsCeaChells.CeaChore;
using HarmonyLib;

[assembly: RequiresMethod(typeof(PardnerEquipment_BullwormRifle), "Awake", typeof(void))]

namespace BullwormRifleFF;

[AttributeUsage(AttributeTargets.Class)]
internal class MMReqVersion : Attribute {}

[MMReqVersion]
[BepInDependency("wwwDayDream.MultiplayerModRestrictor")]
[BepInAutoPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public partial class BullwormRifleFriendlyFire : BaseUnityPlugin {
    public new static ManualLogSource? Logger { get; private set; }
    public static Harmony Patcher { get; } = new(MyPluginInfo.PLUGIN_GUID); 
    public void Awake()
    {
        Logger = base.Logger;

        Patcher.PatchAll();
        
        int patchedMethodCount;
        Logger.LogDebug($"Successfully patched {(patchedMethodCount = Patcher.GetPatchedMethods().Count())} method{(patchedMethodCount == 1 ? "" : "s")}.");
    }
}