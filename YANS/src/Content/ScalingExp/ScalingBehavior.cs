using System;
using System.Linq;
using GantryLibInterface.AbstractClasses;
using TaleWorlds.CampaignSystem;

namespace YANS.Content.ScalingExp
{
    public class ScalingBehavior : DailyExpBehavior
    {
        public override void DailyTick(MobileParty party) {
            base.DailyTick(party);

            if (party is null) {
                throw new ArgumentNullException(nameof(party));
            }

            var exp = 0f;
            foreach (var source in Sources) {
                exp += source.Generic(party);
                exp += source.Party(party);
            }

            GiveExpToGroup(party, exp);
        }

        public override void DailyTick(Town town) {
            base.DailyTick(town);

            if (town is null) {
                throw new ArgumentNullException(nameof(town));
            }

            var party = town.GarrisonParty;
            var exp = 0f;
            foreach (var source in Sources) {
                exp += source.Generic(party);
                exp += source.Town(town);
            }

            GiveExpToGroup(party, exp);
        }

        private void GiveExpToGroup(MobileParty party, float exp) {
            var trainees = party.MemberRoster;
            for (var i = 0; i < trainees.Count(); i++) {
                var totalExp = Sources.Sum(source => source.Character(party, i)) + exp;
                totalExp *= trainees.GetElementNumber(i);
                totalExp *= 0.5f;

                trainees.AddXpToTroopAtIndex((int) Math.Round(totalExp), i);
            }
        }
    }
}
