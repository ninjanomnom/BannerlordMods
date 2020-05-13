using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace YANS.Content.PrisonerRecruitment
{
    public static class PrisonerRecruitmentHelpers
    {
        private struct Willingness
        {
            public float Generic;
            public float Bandit;
            public float Unskilled;
            public float Skilled;
            public float OwnCulture;
            public float OtherCulture;
        }

        internal static float GetPrisonerRecruitChance(int tier) {
            if (tier == 0) {
                return float.PositiveInfinity;
            }

            // Basically, each higher tier is half as likely:
            // 0.4, 0.2, 0.1 etc
            return 0.4f / (float) Math.Pow(2, tier - 1);
        }

        internal static void TryRecruitingPrisoners(MobileParty party) {
            // Leaderless just checks if they're in a garrison apparently??
            if (party.IsLeaderless || party.Leader is null || party?.Party?.Owner is null) {
                return;
            }

            var willingness = GetPartyWillingness(party);

            var behavior = Campaign.Current.GetCampaignBehavior<RecruitPrisonersCampaignBehavior>();
            var prisonRoster = party.PrisonRoster;

            var offset = MBRandom.RandomInt(prisonRoster.Count);
            for (var i = 0; i < prisonRoster.Count; i++) {
                var index = (i + offset) % prisonRoster.Count;

                var characterAtIndex = prisonRoster.GetCharacterAtIndex(index);
                if (characterAtIndex.IsHero) {
                    continue;
                }

                var troopWillingness = GetWillingnessForTroop(willingness, party, characterAtIndex);

                var newRecruits =
                    SubModule.GantryLib.PrisonerHelpers.GetNewRecruitables(party, characterAtIndex, troopWillingness);

                if (newRecruits <= 0) {
                    continue;
                }

                if (party == MobileParty.MainParty) {
                    SubModule.GantryLib.PrisonerHelpers.AddNewRecruits(party, characterAtIndex, newRecruits);
                } else {
                    party.PrisonRoster.AddToCounts(characterAtIndex, -newRecruits);
                    party.MemberRoster.AddToCounts(characterAtIndex, newRecruits);
                }
            }
        }

        private static float GetWillingnessForTroop(Willingness willingness, MobileParty party, CharacterObject troop) {
            var troopWillingness = willingness.Generic;

            if (troop.Culture.IsBandit) {
                troopWillingness += willingness.Bandit;
            }

            if (troop.Tier > 4) {
                troopWillingness += willingness.Skilled;
            } else {
                troopWillingness += willingness.Unskilled;
            }

            if (troop.Culture == party.Leader.Culture) {
                troopWillingness += willingness.OwnCulture;
            } else {
                troopWillingness += willingness.OtherCulture;
            }

            return troopWillingness;
        }

        private static Willingness GetPartyWillingness(MobileParty party) {
            var leader = party.Leader;
            var willingness = new Willingness {
                Generic = 0f,
                Bandit = 0f,
                Unskilled = 0f,
                Skilled = 0f,
                OwnCulture = 1f,
                OtherCulture = 0f,
            };

            if (party == MobileParty.MainParty) {
                willingness.Generic += 1;
            }

            // Cautious leaders don't trust former enemies
            var valor = leader.GetTraitLevel(DefaultTraits.Valor);
            if (valor < 0) {
                willingness.Generic += valor;
                willingness.Bandit += valor;
                willingness.Unskilled += valor;
                willingness.Skilled += valor;
                willingness.OwnCulture += valor;
                willingness.OtherCulture += valor;
            }

            // Merciful leaders are willing to let anyone redeem themselves
            var mercy = leader.GetTraitLevel(DefaultTraits.Mercy);
            if (mercy > 0) {
                willingness.Generic += mercy;
                willingness.Bandit -= mercy * 0.1f;
            } else {
                willingness.OtherCulture += mercy;
            }

            // Honorable leaders like to free people who fought well
            // Dishonorable ones are perfectly fine hiring bandits but others of their own culture wont like them
            var honor = leader.GetTraitLevel(DefaultTraits.Honor);
            if (honor > 0) {
                willingness.Skilled += honor * 0.5f;
                willingness.Bandit -= honor;
            } else {
                willingness.Bandit -= honor;
                willingness.OwnCulture += honor * -0.5f;
            }

            // Generous leaders are willing to help those less fortunate than themselves
            var generosity = leader.GetTraitLevel(DefaultTraits.Generosity);
            if (generosity > 0) {
                willingness.Unskilled += generosity * 0.5f;
                willingness.Bandit += generosity * 0.25f;
            }

            return willingness;
        }
    }
}
