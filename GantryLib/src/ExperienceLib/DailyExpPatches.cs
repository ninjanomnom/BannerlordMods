using System;
using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace GantryLib.ExperienceLib
{
    [HarmonyPatch]
    internal static class DailyExpUnification
    {
        // We're replacing all exp handling in these functions so lets remove anything that grants exp
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(MobileParty), "DailyTick")]
        [HarmonyPatch(typeof(Town), "DailyTick")]
        internal static IEnumerable<CodeInstruction> DailyTick(IEnumerable<CodeInstruction> instructions) {
            var expMethod = typeof(TroopRoster).GetMethod("AddXpToTroop");
            var noop = new Func<int, CharacterObject, int>((xp, troop) => 0).Method;

            return instructions.MethodReplacer(expMethod, noop);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Town), "DailyTick")]
        internal static void TownDailyTick(Town __instance) {
            var garrison = __instance.GarrisonParty;

            if (garrison == null || !garrison.IsActive || garrison.MapEvent != null ||
                garrison.CurrentSettlement == null) {
                return;
            }

            SubModule.Instance.InternalExpController.DailyTick(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MobileParty), "DailyTick")]
        internal static void PartyDailyTick(MobileParty __instance) {
            if (!__instance.IsActive) {
                return;
            }

            SubModule.Instance.InternalExpController.DailyTick(__instance);
        }
    }
}
