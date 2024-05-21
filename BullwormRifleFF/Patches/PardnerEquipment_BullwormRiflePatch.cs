using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace BullwormRifleFF.Patches;

[HarmonyPatch(typeof(PardnerEquipment_BullwormRifle))]
public static class PardnerEquipment_BullwormRiflePatch {
    [HarmonyPatch(nameof(PardnerEquipment_BullwormRifle.OnUse))] [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> InsertNewLayerCase(IEnumerable<CodeInstruction> codeInstructions)
    {
        var codeMatcher =  new CodeMatcher(codeInstructions);
        codeMatcher.MatchForward(false, 
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldloca || instruction.opcode == OpCodes.Ldloca_S),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Call || instruction.opcode == OpCodes.Callvirt),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Call || instruction.opcode == OpCodes.Callvirt),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Call || instruction.opcode == OpCodes.Callvirt),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldc_I4 || instruction.opcode == OpCodes.Ldc_I4_S),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Beq || instruction.opcode == OpCodes.Beq_S))
            .ThrowIfInvalid("Failure to find patch location in PardnerEquipment_BullwormRifle::OnUse!");
        codeMatcher.Insert(
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(OpCodes.Ldloca_S, codeMatcher.Operand),
            new CodeInstruction(OpCodes.Call, typeof(PardnerEquipment_BullwormRiflePatch).GetMethod(nameof(HitPlayerCase))));
            
        return codeMatcher.Instructions();
    }

    [HarmonyPatch("Awake")] [HarmonyPrefix]
    public static void AddToBulletLayerMask(PardnerEquipment_BullwormRifle __instance)
    {
        BullwormRifleFriendlyFire.Logger?.LogDebug("Bullworm Rifle Awake");
        __instance.bulletLayerMask |= 1 << LayerMask.NameToLayer("Player");
    }

    public static void HitPlayerCase(PardnerEquipment_BullwormRifle self, ref RaycastHit raycastHit)
    {
        BullwormRifleFriendlyFire.Logger?.LogDebug($"Hit {LayerMask.LayerToName(raycastHit.collider.gameObject.layer)}");

        if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var player = raycastHit.collider.gameObject.GetComponentInParent<Player>();
            player.DamagePardner_ServerRPC(self.damageToWorm);
            BullwormRifleFriendlyFire.Logger?.LogDebug("Dealing damage to player!");
        }
    }
}