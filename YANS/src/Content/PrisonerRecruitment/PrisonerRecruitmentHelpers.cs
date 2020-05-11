using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace YANS.Content.PrisonerRecruitment
{
    public class PrisonerRecruitmentHelpers
    {
        private struct Willingness
        {
            public float generic;
            public float bandit;
            public float unskilled;
            public float skilled;
            public float ownCulture;
            public float otherCulture;
        }

        internal static float GetPrisonerRecruitChance(MobileParty party, int tier)
        {
            if (tier == 0)
            {
                return float.PositiveInfinity;
            }

            // Basicaly, each higher tier is half as likely:
            // 0.4, 0.2, 0.1 etc
            return (0.4f / (float)Math.Pow(2, tier - 1));
        }

        internal static void TryRecruitingPrisoners(MobileParty party)
        {
            // Leaderless just checks if they're in a garrison apparently??
            if (party.IsLeaderless || party.Leader is null || party?.Party?.Owner is null)
            {
                return;
            }
            Willingness willingness = GetPartyWillingness(party);

            var behavior = Campaign.Current.GetCampaignBehavior<RecruitPrisonersCampaignBehavior>();
            var prisonRoster = party.PrisonRoster;

            int offset = MBRandom.RandomInt(prisonRoster.Count);
            for (var i = 0; i < prisonRoster.Count; i++)
            {
                int index = (i + offset) % prisonRoster.Count;

                CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(index);
                if (characterAtIndex.IsHero)
                {
                    continue;
                }

                var troopWillingness = GetWillingnessForTroop(willingness, party, characterAtIndex);

                var newRecruits = SubModule.GantryLib.PrisonerHelpers.GetNewRecruitables(party, characterAtIndex, troopWillingness);

                if (newRecruits <= 0)
                {
                    continue;
                }

                if (party == MobileParty.MainParty)
                {
                    SubModule.GantryLib.PrisonerHelpers.AddNewRecruits(party, characterAtIndex, newRecruits);
                }
                else
                {
                    party.PrisonRoster.AddToCounts(characterAtIndex, -newRecruits);
                    party.MemberRoster.AddToCounts(characterAtIndex, newRecruits);
                }
            }
        }

        private static float GetWillingnessForTroop(Willingness willingness, MobileParty party, CharacterObject troop)
        {
            var troopWillingness = willingness.generic;

            if (troop.Culture.IsBandit)
            {
                troopWillingness += willingness.bandit;
            }

            if (troop.Tier > 4)
            {
                troopWillingness += willingness.skilled;
            }
            else
            {
                troopWillingness += willingness.unskilled;
            }

            if (troop.Culture == party.Leader.Culture)
            {
                troopWillingness += willingness.ownCulture;
            }
            else
            {
                troopWillingness += willingness.otherCulture;
            }

            return troopWillingness;
        }

        private static Willingness GetPartyWillingness(MobileParty party)
        {
            var leader = party.Leader;
            var willingness = new Willingness
            {
                generic = 0f,
                bandit = 0f,
                unskilled = 0f,
                skilled = 0f,
                ownCulture = 1f,
                otherCulture = 0f
            };

            if (party == MobileParty.MainParty)
            {
                willingness.generic += 1;
            }

            // Cautious leaders don't trust former enemies
            var valor = leader.GetTraitLevel(DefaultTraits.Valor);
            if (valor < 0)
            {
                willingness.generic += valor;
                willingness.bandit += valor;
                willingness.unskilled += valor;
                willingness.skilled += valor;
                willingness.ownCulture += valor;
                willingness.otherCulture += valor;
            }

            // Merciful leaders are willing to let anyone redeem themselves
            var mercy = leader.GetTraitLevel(DefaultTraits.Mercy);
            if (mercy > 0)
            {
                willingness.generic += mercy;
                willingness.bandit -= mercy * 0.1f;
            }
            else
            {
                willingness.otherCulture += mercy;
            }

            // Honorable leaders like to free people who fought well
            // Dishonorable ones are perfectly fine hiring bandits but others of their own culture wont like them
            var honor = leader.GetTraitLevel(DefaultTraits.Honor);
            if (honor > 0)
            {
                willingness.skilled += honor * 0.5f;
                willingness.bandit -= honor;
            }
            else
            {
                willingness.bandit -= honor;
                willingness.ownCulture += honor * -0.5f;
            }

            // Generous leaders are willing to help those less fortunate than themselves
            var generosity = leader.GetTraitLevel(DefaultTraits.Generosity);
            if (generosity > 0)
            {
                willingness.unskilled += generosity * 0.5f;
                willingness.bandit += generosity * 0.25f;
            }

            return willingness;
        }
    }
}
