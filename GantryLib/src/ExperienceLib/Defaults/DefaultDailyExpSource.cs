using GantryLibInterface.AbstractClasses;
using TaleWorlds.CampaignSystem;

namespace GantryLib.ExperienceLib.Defaults
{
    internal class DefaultDailyExpSource : DailyExpSource
    {
        public override float Generic(MobileParty party) {
            base.Generic(party);

            var exp = 0f;
            if (party.HasPerk(DefaultPerks.Leadership.CombatTips)) {
                exp += Campaign.Current.Models.PartyTrainingModel.GetTroopPerksXp(DefaultPerks.Leadership.CombatTips);
            }

            return exp;
        }

        public override float Town(Town town) {
            base.Town(town);

            var exp = 0f;
            var dailyTroopXpBonus = Campaign.Current.Models.DailyTroopXpBonusModel.CalculateDailyTroopXpBonus(town);
            exp += dailyTroopXpBonus *
                   Campaign.Current.Models.DailyTroopXpBonusModel.CalculateGarrisonXpBonusMultiplier(town);

            return exp;
        }

        public override float Character(MobileParty party, int characterIndex) {
            base.Character(party, characterIndex);

            var exp = 0f;
            if (party.HasPerk(DefaultPerks.Leadership.CombatTips) &&
                party.MemberRoster.GetCharacterAtIndex(characterIndex).Tier < 4) {
                exp += Campaign.Current.Models.PartyTrainingModel.GetTroopPerksXp(DefaultPerks.Leadership.CombatTips);
            }

            return exp;
        }
    }
}
