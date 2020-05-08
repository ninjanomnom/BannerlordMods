using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;

namespace GantryLib.ExpUnification
{
    [HarmonyPatch]
    public static class DailyExpUnification
    {
        private static List<DailyExpSource> _sources = new List<DailyExpSource>();

        /// <summary>
        /// Registers a new daily exp source to be queried for xp assignments
        /// </summary>
        /// <param name="source">The new source</param>
        public static void Register(DailyExpSource source)
        {
            _sources.Add(source);
        }

        // We're replacing all exp handling in these functions so lets remove anything that grants exp
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(MobileParty), "DailyTick")]
        [HarmonyPatch(typeof(Town), "DailyTick")]
        internal static IEnumerable<CodeInstruction> DailyTick(IEnumerable<CodeInstruction> instructions)
        {
            var expMethod = typeof(TroopRoster).GetMethod("AddXpToTroop");
            var noop = typeof(DailyExpSource).GetMethod("DoNothing");

            return instructions.MethodReplacer(expMethod, noop);
        }

#pragma warning disable IDE0051 // Remove unused private members
        private static void DoNothing() { }
#pragma warning restore IDE0051 // Remove unused private members

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
            foreach (var source in _sources)
            {
                source.Generic(garrison, out var genericA, out var genericP);
                source.Town(garrison, __instance, out var townA, out var townB);
                activeExp += genericA + townA;
                passiveExp += genericP + townB;
            }

            GiveExpToGroup(garrison, activeExp, passiveExp);
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
            foreach(var source in _sources)
            {
                source.Generic(__instance, out var genericA, out var genericP);
                source.Party(__instance, out var partyA, out var partyP);
                activeExp += genericA + partyA;
                passiveExp += genericP + partyP; 
            }

            GiveExpToGroup(__instance, activeExp, passiveExp);
        }

        private static void GiveExpToGroup(MobileParty party, float activeExp, float passiveExp)
        {
            List<CharacterObject> trainees = party.MemberRoster.Troops.ToList();
            for (var i = 0; i < trainees.Count(); i++)
            {
                float troopActive = 0;
                float troopPassive = 0;
                foreach (var source in _sources)
                {
                    source.Character(party, i, out var characterA, out var characterB);
                    troopActive += characterA;
                    troopPassive += characterB;
                }

                var total = party.MemberRoster.GetElementNumber(i);
                var active = total / 2;
                var passive = total - active;

                var totalActiveExp = active * (activeExp + troopActive);
                var totalPassiveExp = passive * (passiveExp + troopPassive);
                var totalExp = (int)Math.Round(totalActiveExp + totalPassiveExp);

                party.MemberRoster.AddXpToTroopAtIndex(totalExp, i);
            }

            Campaign.Current.PartyUpgrader.UpgradeReadyTroops(party.Party);
        }
    }
}
