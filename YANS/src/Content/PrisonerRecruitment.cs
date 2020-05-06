using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;
using GantryLib.Extensions;
using System;
using System.Collections.Generic;

namespace YANS.Content
{
    [HarmonyPatch]
    internal class PrisonerRecruitment
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RecruitPrisonersCampaignBehavior), "DailyTick")]
        internal static void DailyTick(RecruitPrisonersCampaignBehavior src, MobileParty party)
        {
            TroopRoster prisonRoster = party.PrisonRoster;

            int offset = MBRandom.RandomInt(prisonRoster.Count);
            for (var i = 0; i < prisonRoster.Count; i++)
            {
                int index = (i + offset) % prisonRoster.Count;
                
                CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(index);
                var newRecruits = src.GetNewRecruitables(party, characterAtIndex);

                src.AddNewRecruits(party, characterAtIndex, newRecruits);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PrisonerRecruitmentCalculationModel), "GetDailyRecruitedPrisoners")]
        internal static float[] GetDailyRecruitedPrisoners(float[] parentResult, MobileParty mainParty)
        {
            const int maxTier = 6;

            var results = new float[maxTier];

            for(var i = 0; i < maxTier; i++)
            {
                results[i] = GetPrisonerRecruitChance(mainParty, i);
            }

            return new float[] { float.PositiveInfinity, 0.4f, 0.2f, 0.1f, 0.05f, 0.025f, 0.0125f };
        }

        // Use harmony to mess with this function instead of the normal one above
        private static float GetPrisonerRecruitChance(MobileParty party, int tier)
        {
            if(tier == 0)
            {
                return float.PositiveInfinity;
            }

            // Basicaly, each higher tier is half as likely:
            // 0.4, 0.2, 0.1 etc
            return (0.4f / (float)Math.Pow(2, tier-1));
        }
    }
}
