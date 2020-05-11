using HarmonyLib;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.VillageBehaviors;

namespace YAS.KingdomBuilder.HarmonyPatches
{
    [HarmonyPatch]
    class VillageTradeBehavior
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(VillageGoodProductionCampaignBehavior), "AssignTradeBounds")]
        internal bool AssignTradeBounds()
        {
            foreach (Village current in Village.All.Where(s => !s.TradeBound.IsTown))
            {
                // Refresh village trade bounds
            }

            return false;
        }
    }
}
