using GantryLibInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;

namespace GantryLib.ExpUnification
{
    internal class DailyExpHelpers : IDailyExpHelpers
    {
        public List<IDailyExpSource> Sources { get; private set; }

        internal DailyExpHelpers()
        {
            Sources = new List<IDailyExpSource>();
        }

        public void Register(IDailyExpSource source)
        {
            Sources.Add(source);
        }

        public void GiveExpToGroup(MobileParty party, float activeExp, float passiveExp)
        {
            List<CharacterObject> trainees = party.MemberRoster.Troops.ToList();
            for (var i = 0; i < trainees.Count(); i++)
            {
                float troopActive = 0;
                float troopPassive = 0;
                foreach (var source in Sources)
                {
                    source.Character(party, i, out var characterA, out var characterB);
                    troopActive += characterA;
                    troopPassive += characterB;
                }

                var total = party.MemberRoster.GetElementNumber(i);
                var active = total / 2;
                var passive = total - active;

                var totalActiveExp = active * (activeExp + troopActive);
                var totalPassiveExp = passive * (passiveExp + troopPassive);
                var totalExp = (int)Math.Round(totalActiveExp + totalPassiveExp);

                party.MemberRoster.AddXpToTroopAtIndex(totalExp, i);
            }

            if (party != MobileParty.MainParty)
            {
                Campaign.Current.PartyUpgrader.UpgradeReadyTroops(party.Party);
            }
        }
    }
}
