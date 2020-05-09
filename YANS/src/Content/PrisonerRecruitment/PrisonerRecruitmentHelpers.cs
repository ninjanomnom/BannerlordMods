using System;
using TaleWorlds.CampaignSystem;

namespace YANS.Content.PrisonerRecruitment
{
    public class PrisonerRecruitmentHelpers
    {
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
    }
}
