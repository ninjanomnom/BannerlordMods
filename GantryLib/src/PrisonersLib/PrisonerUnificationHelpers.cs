using System;
using GantryLibInterface.Interfaces;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace GantryLib.PrisonerUnification
{
    public class PrisonerUnificationHelpers : IPrisonerHelpers
    {
        public int GetNewRecruitables(MobileParty party, CharacterObject prisoner, float chanceModifier = 1) {
            var behavior = Campaign.Current.GetCampaignBehavior<RecruitPrisonersCampaignBehavior>();

            var recruitable = party == MobileParty.MainParty
                ? behavior.GetRecruitableNumber(prisoner)
                : party.PrisonRoster.GetElementNumber(party.PrisonRoster.FindIndexOfTroop(prisoner));

            if (recruitable <= 0) {
                return 0;
            }

            var dailyRecruitedPrisoners =
                Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetDailyRecruitedPrisoners(party);

            if (dailyRecruitedPrisoners.Length <= prisoner.Tier) {
                return 0;
            }

            var chance = dailyRecruitedPrisoners[prisoner.Tier] * chanceModifier;

            var newRecruited = 0;
            for (var i = 0; i < recruitable; i++) {
                if (MBRandom.RandomFloat >= chance) {
                    continue;
                }

                newRecruited++;
                chance *= 0.5f;
            }

            return newRecruited;
        }

        public void AddNewRecruits(MobileParty party, CharacterObject prisoner, int amount) {
            var behavior = Campaign.Current.GetCampaignBehavior<RecruitPrisonersCampaignBehavior>();
            var recruitable = behavior.GetRecruitableNumber(prisoner);
            var newTotal = Math.Min(recruitable + amount, party.PrisonRoster.GetTroopCount(prisoner));

            behavior.SetRecruitableNumber(prisoner, newTotal);
        }
    }
}
