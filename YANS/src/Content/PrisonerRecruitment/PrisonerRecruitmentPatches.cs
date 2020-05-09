using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;
using GantryLib.Extensions;
using System;
using System.Collections.Generic;

namespace YANS.Content.PrisonerRecruitment
{
    [HarmonyPatch]
    internal class PrisonerRecruitmentPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RecruitPrisonersCampaignBehavior), "DailyTick")]
        internal static void DailyTick(RecruitPrisonersCampaignBehavior __instance)
        {
            MobileParty party = MobileParty.MainParty;
            TroopRoster prisonRoster = party.PrisonRoster;

            int offset = MBRandom.RandomInt(prisonRoster.Count);
            for (var i = 0; i < prisonRoster.Count; i++)
            {
                int index = (i + offset) % prisonRoster.Count;
                
                CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(index);
                var newRecruits = __instance.GetNewRecruitables(party, characterAtIndex);

                __instance.AddNewRecruits(party, characterAtIndex, newRecruits);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DefaultPrisonerRecruitmentCalculationModel), "GetDailyRecruitedPrisoners")]
        internal static bool GetDailyRecruitedPrisoners(ref float[] __result)
        {
            MobileParty mainParty = MobileParty.MainParty;

            const int maxTier = 6;

            var results = new float[maxTier];

            for(var i = 0; i < maxTier; i++)
            {
                results[i] = PrisonerRecruitmentHelpers.GetPrisonerRecruitChance(mainParty, i);
            }

            __result = new float[] { float.PositiveInfinity, 0.4f, 0.2f, 0.1f, 0.05f, 0.025f, 0.0125f };
            return true;
        }
    }
}
