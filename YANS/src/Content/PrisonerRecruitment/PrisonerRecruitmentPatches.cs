using System;
using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;

namespace YANS.Content.PrisonerRecruitment
{
    [UsedImplicitly]
    [HarmonyPatch]
    internal class PrisonerRecruitmentPatches
    {
        [UsedImplicitly]
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(RecruitPrisonersCampaignBehavior), "DailyTick")]
        internal static IEnumerable<CodeInstruction> DailyTick(IEnumerable<CodeInstruction> instructions) {
            var prisonersRecruitMethod =
                typeof(DefaultPrisonerRecruitmentCalculationModel).GetMethod("GetDailyRecruitedPrisoners");
            var noop = new Func<MobileParty, float[]>((party) => Array.Empty<float>()).Method;

            return instructions.MethodReplacer(prisonersRecruitMethod, noop);
        }

        [UsedImplicitly]
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RecruitPrisonersCampaignBehavior), "DailyTick")]
        internal static void DailyTick() {
            foreach (var party in MobileParty.All) {
                PrisonerRecruitmentHelpers.TryRecruitingPrisoners(party);
            }
        }

        [UsedImplicitly]
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DefaultPrisonerRecruitmentCalculationModel), "GetDailyRecruitedPrisoners")]
        internal static bool GetDailyRecruitedPrisoners(ref float[] __result) {
            var mainParty = MobileParty.MainParty;

            const int maxTier = 6;

            var results = new float[maxTier];

            for (var i = 0; i < maxTier; i++) {
                results[i] = PrisonerRecruitmentHelpers.GetPrisonerRecruitChance(i);
            }

            __result = new float[] {float.PositiveInfinity, 0.4f, 0.2f, 0.1f, 0.05f, 0.025f, 0.0125f};
            return true;
        }
    }
}
