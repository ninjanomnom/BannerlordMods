using GantryLib.ReflectionHelpers;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace GantryLib.Extensions
{
    public static class RecruitPrisonersCampaignBehaviorExtensions
    {
        // We use these private methods to change as little as possible when we replace the DailyTick function
        private static readonly PrivateFunc<RecruitPrisonersCampaignBehavior, Func<CharacterObject, bool>> IsPrisonerRecruitable =
            new PrivateFunc<RecruitPrisonersCampaignBehavior, Func<CharacterObject, bool>>("IsPrisonerRecruitable");
        private static readonly PrivateFunc<RecruitPrisonersCampaignBehavior, Func<CharacterObject, int>> GetRecruitableNumberInternal =
            new PrivateFunc<RecruitPrisonersCampaignBehavior, Func<CharacterObject, int>>("GetRecruitableNumberInternal");
        private static readonly PrivateFunc<RecruitPrisonersCampaignBehavior, Action<CharacterObject, int>> SetRecruitableNumberInternal =
            new PrivateFunc<RecruitPrisonersCampaignBehavior, Action<CharacterObject, int>>("SetRecruitableNumberInternal");

        public static int GetNewRecruitables(this RecruitPrisonersCampaignBehavior src, MobileParty party, CharacterObject prisoner)
        {
            if(!IsPrisonerRecruitable.Get(src).Invoke(prisoner))
            {
                return 0;
            }

            float[] dailyRecruitedPrisoners = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetDailyRecruitedPrisoners(party);

            if(dailyRecruitedPrisoners.Length <= prisoner.Tier)
            {
                return 0;
            }

            float chance = dailyRecruitedPrisoners[prisoner.Tier];
            int recruitable = GetRecruitableNumberInternal.Get(src).Invoke(prisoner);

            int newRecruited = 0;
            for (var i = 0; i < recruitable; i++ )
            {
                if(MBRandom.RandomFloat >= chance)
                {
                    continue;
                }

                newRecruited++;
                chance /= 2;
            }

            return newRecruited;
        }

        public static void AddNewRecruits(this RecruitPrisonersCampaignBehavior src, MobileParty party, CharacterObject prisoner, int amount)
        {
            var recruitable = GetRecruitableNumberInternal.Get(src).Invoke(prisoner);
            var newTotal = Math.Min(recruitable + amount, party.PrisonRoster.GetTroopCount(prisoner));

            SetRecruitableNumberInternal.Get(src).Invoke(prisoner, newTotal);
        }
    }
}
