using HarmonyLib;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace GantryLib.ExpUnification
{
    [HarmonyPatch]
    public static class DailyExpUnification
    {
        // We're replacing all exp handling in these functions so lets remove anything that grants exp
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(MobileParty), "DailyTick")]
        [HarmonyPatch(typeof(Town), "DailyTick")]
        internal static IEnumerable<CodeInstruction> DailyTick(IEnumerable<CodeInstruction> instructions)
        {
            var expMethod = typeof(TroopRoster).GetMethod("AddXpToTroop");
            var noop = new Func<int, CharacterObject, int>((xp, troop) => 0).Method;

            return instructions.MethodReplacer(expMethod, noop);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Town), "DailyTick")]
        internal static void TownDailyTick(Town __instance)
        {
            MobileParty garrison = __instance.GarrisonParty;

            if (garrison == null || !garrison.IsActive || garrison.MapEvent != null || garrison.CurrentSettlement == null)
            {
                return;
            }

            float activeExp = 0;
            float passiveExp = 0;
            foreach (var source in DailyExpHelpers._sources)
            {
                source.Generic(garrison, out var genericA, out var genericP);
                source.Town(garrison, __instance, out var townA, out var townB);
                activeExp += genericA + townA;
                passiveExp += genericP + townB;
            }

            DailyExpHelpers.GiveExpToGroup(garrison, activeExp, passiveExp);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MobileParty), "DailyTick")]
        internal static void PartyDailyTick(MobileParty __instance)
        {
            if(!__instance.IsActive)
            {
                return;
            }

            float activeExp = 0;
            float passiveExp = 0;
            foreach(var source in DailyExpHelpers._sources)
            {
                source.Generic(__instance, out var genericA, out var genericP);
                source.Party(__instance, out var partyA, out var partyP);
                activeExp += genericA + partyA;
                passiveExp += genericP + partyP; 
            }

            DailyExpHelpers.GiveExpToGroup(__instance, activeExp, passiveExp);
        }
    }
}
