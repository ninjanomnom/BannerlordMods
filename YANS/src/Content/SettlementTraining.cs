using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace YANS.Content
{
    [HarmonyPatch]
    internal class SettlementTraining
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Town), "DailyTick")]
        internal static void DailyTick(Town __instance)
        {   
            MobileParty garrison = __instance.GarrisonParty;

            if(garrison == null || !garrison.IsActive || garrison.MapEvent != null || garrison.CurrentSettlement == null)
            {
                return;
            }

            var dailyTroopXpBonusModel = Campaign.Current.Models.DailyTroopXpBonusModel;

            var exp = dailyTroopXpBonusModel.CalculateDailyTroopXpBonus(__instance, null);
            var multiplier = dailyTroopXpBonusModel.CalculateGarrisonXpBonusMultiplier(__instance, null);
            exp = MathF.Round(exp * multiplier);
            if (exp <= 0)
            {
                return;
            }
            List<CharacterObject> trainees = garrison.MemberRoster.Troops.Where(t => !t.IsHero).ToList();

            var maximumActive = MathF.Round(2 * exp);
            var currentActive = 0;
            while(trainees.Count > 0)
            {
                var index = MBRandom.RandomInt(trainees.Count) % trainees.Count;
                CharacterObject trainee = trainees[index];
                trainees.RemoveAt(index);

                // The base game handles exp as if there's only 1 troop in each stack
                // That means we now need to only handle the exp for everyone else in the stack
                var traineeCount = garrison.MemberRoster.GetElementNumber(index) - 1;
                if(traineeCount <= 0)
                {
                    continue;
                }

                var realExp = 0;

                // This handles the active trainees
                var newActive = Math.Min(traineeCount, maximumActive - currentActive);
                currentActive += newActive;
                realExp += exp * newActive;

                // If any trainees are left over that can't be active they can be spectators
                var leftover = traineeCount - newActive;
                realExp += leftover;

                // Give the actual exp assignment back to the main game
                garrison.MemberRoster.AddXpToTroop(realExp, trainee);
            }

            Campaign.Current.PartyUpgrader.UpgradeReadyTroops(__instance.GarrisonParty.Party);
        }
    }
}
