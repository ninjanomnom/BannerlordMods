using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace GantryLib.ExpUnification
{
    public class DailyExpHelpers
    {
        internal static List<DailyExpSource> _sources = new List<DailyExpSource>();

        /// <summary>
        /// Registers a new daily exp source to be queried for xp assignments
        /// </summary>
        /// <param name="source">The new source</param>
        public static void Register(DailyExpSource source)
        {
            _sources.Add(source);
        }

        public static void GiveExpToGroup(MobileParty party, float activeExp, float passiveExp)
        {
            List<CharacterObject> trainees = party.MemberRoster.Troops.ToList();
            for (var i = 0; i < trainees.Count(); i++)
            {
                float troopActive = 0;
                float troopPassive = 0;
                foreach (var source in _sources)
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
