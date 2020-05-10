using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace GantryLib.Extensions
{
    public static class RecruitPrisonersCampaignBehaviorExtensions
    {
        public static int GetNewRecruitables(this RecruitPrisonersCampaignBehavior src, MobileParty party, CharacterObject prisoner, float chanceModifier = 1)
        {
            int recruitable = party == MobileParty.MainParty
                ? src.GetRecruitableNumber(prisoner)
                : party.PrisonRoster.GetElementNumber(party.PrisonRoster.FindIndexOfTroop(prisoner));
            
            if (recruitable <= 0)
            {
                return 0;
            }

            float[] dailyRecruitedPrisoners = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetDailyRecruitedPrisoners(party);

            if(dailyRecruitedPrisoners.Length <= prisoner.Tier)
            {
                return 0;
            }

            float chance = dailyRecruitedPrisoners[prisoner.Tier] * chanceModifier;

            int newRecruited = 0;
            for (var i = 0; i < recruitable; i++ )
            {
                if(MBRandom.RandomFloat >= chance)
                {
                    continue;
                }

                newRecruited++;
                chance *= 0.5f;
            }

            return newRecruited;
        }

        public static void AddNewRecruits(this RecruitPrisonersCampaignBehavior src, MobileParty party, CharacterObject prisoner, int amount)
        {
            var recruitable = src.GetRecruitableNumber(prisoner);
            var newTotal = Math.Min(recruitable + amount, party.PrisonRoster.GetTroopCount(prisoner));

            src.SetRecruitableNumber(prisoner, newTotal);
        }
    }
}
