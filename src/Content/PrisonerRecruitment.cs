using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;
using tomelib.ReflectionHelpers;

namespace YANS.Content
{
    [HarmonyPatch]
    internal class PrisonerRecruitment
    {
        // We use these private methods to change as little as possible when we replace the DailyTick function
        private static readonly PrivateFunc<RecruitPrisonersCampaignBehavior, bool> IsPrisonerRecruitable =
            new PrivateFunc<RecruitPrisonersCampaignBehavior, bool>("IsPrisonerRecruitable");
        private static readonly PrivateFunc<RecruitPrisonersCampaignBehavior, int> GetRecruitableNumberInternal =
            new PrivateFunc<RecruitPrisonersCampaignBehavior, int>("GetRecruitableNumberInternal");
        private static readonly PrivateFunc<RecruitPrisonersCampaignBehavior, int> SetRecruitableNumberInternal =
            new PrivateFunc<RecruitPrisonersCampaignBehavior, int>("SetRecruitableNumberInternal");

        [HarmonyPostfix]
        [HarmonyPatch(typeof(RecruitPrisonersCampaignBehavior), "DailyTick")]
        internal static void DailyTick(RecruitPrisonersCampaignBehavior __instance)
        {
            float[] dailyRecruitedPrisoners = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetDailyRecruitedPrisoners(MobileParty.MainParty);

            TroopRoster prisonRoster = MobileParty.MainParty.PrisonRoster;
            int offset = MBRandom.RandomInt(prisonRoster.Count);
            for (var i = 0; i < prisonRoster.Count; i++)
            {
                int index = (i + offset) % prisonRoster.Count;
                CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(index);

                if (!IsPrisonerRecruitable.Execute(__instance, new object[] { characterAtIndex }))
                {
                    continue;
                }

                int tier = characterAtIndex.Tier;
                if (tier >= dailyRecruitedPrisoners.Length)
                {
                    continue;
                }

                int totalAmount = prisonRoster.GetElementNumber(index);
                int recruitable = GetRecruitableNumberInternal.Execute(__instance, new object[] { characterAtIndex });
                for (var k = recruitable + 1; k <= totalAmount; k++)
                {
                    if (MBRandom.RandomFloat >= dailyRecruitedPrisoners[tier])
                    {
                        continue;
                    }

                    recruitable++;
                    dailyRecruitedPrisoners[tier] /= 2;
                }

                SetRecruitableNumberInternal.Execute(__instance, new object[] { characterAtIndex, recruitable });
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(DefaultPrisonerRecruitmentCalculationModel), "GetDailyRecruitedPrisoners")]
        internal static float[] GetDailyRecruitedPrisoners(float[] parentResult, MobileParty mainParty)
        {
            return new float[] { float.PositiveInfinity, 0.4f, 0.2f, 0.1f, 0.05f, 0.025f, 0.0125f };
        }
    }
}
