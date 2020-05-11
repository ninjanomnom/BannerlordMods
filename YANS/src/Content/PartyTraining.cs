using GantryLibInterface.AbstractClasses;
using TaleWorlds.CampaignSystem;

namespace YANS.Content
{
    internal class PartyTraining : DailyExpSource
    {
        public override void Generic(MobileParty party, out float active, out float passive)
        {
            base.Generic(party, out active, out passive);
            if (party.HasPerk(DefaultPerks.Leadership.CombatTips))
            {
                var baseExp = Campaign.Current.Models.PartyTrainingModel.GetTroopPerksXp(DefaultPerks.Leadership.CombatTips);
                active += baseExp;
                passive += 1;
            }
        }

        public override void Town(MobileParty party, Town town, out float active, out float passive)
        {
            base.Town(party, town, out active, out passive);
            var dailyTroopXpBonus = Campaign.Current.Models.DailyTroopXpBonusModel.CalculateDailyTroopXpBonus(town);
            if (dailyTroopXpBonus > 0)
            {
                var xpBonusMultiplier = Campaign.Current.Models.DailyTroopXpBonusModel.CalculateGarrisonXpBonusMultiplier(town);
                active = dailyTroopXpBonus * xpBonusMultiplier;
                passive = 1;
            }
        }

        public override void Character(MobileParty party, int characterIndex, out float active, out float passive)
        {
            base.Character(party, characterIndex, out active, out passive);
            if (party.HasPerk(DefaultPerks.Leadership.CombatTips) && party.MemberRoster.GetCharacterAtIndex(characterIndex).Tier < 4)
            {
                var baseExp = Campaign.Current.Models.PartyTrainingModel.GetTroopPerksXp(DefaultPerks.Leadership.CombatTips);
                active += baseExp;
                passive += 1;
            }
        }
    }
}
