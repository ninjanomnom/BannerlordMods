using System;
using System.Linq;
using GantryLibInterface.AbstractClasses;
using TaleWorlds.CampaignSystem;

namespace GantryLib.ExperienceLib.Defaults
{
    public class DefaultDailyExpBehavior : DailyExpBehavior
    {
        public override void DailyTick(MobileParty party) {
            float exp = 0;
            foreach (var source in Sources) {
                exp += source.Generic(party);
                exp += source.Party(party);
            }

            GiveExpToCharacterGroups(party, exp);
        }

        public override void DailyTick(Town town) {
            var party = town.GarrisonParty;

            float exp = 0;
            foreach (var source in Sources) {
                exp += source.Generic(party);
                exp += source.Town(town);
            }

            GiveExpToCharacterGroups(party, exp);
        }

        private void GiveExpToCharacterGroups(MobileParty party, float exp) {
            var troopRoster = party.MemberRoster.Troops;
            for (var i = 0; i < troopRoster.Count(); i++) {
                var characterExp = exp + Sources.Sum(source => source.Character(party, i));

                party.MemberRoster.AddXpToTroopAtIndex((int) Math.Round(characterExp), i);
            }
        }
    }
}
