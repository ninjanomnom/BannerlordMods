using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace GantryLib.Extensions
{
    public static class VillageExtension
    {
        public static void Initialize(this Village src)
        {
            src.RefreshTradeBounds();
        }

        /// <summary>
        /// This was originaly in the function AssignTradeBounds inlined
        /// It's been split off here to allow us to call it more easily without refreshing every village at once
        /// </summary>
        public static void RefreshTradeBounds(this Village src)
        {
            float currentDistance = float.PositiveInfinity;
            Settlement tradeBound = null;

            foreach (Settlement candidate in Settlement.All)
            {
                if (!candidate.IsTown)
                {
                    continue;
                }
                if (src.Settlement.MapFaction != candidate.MapFaction)
                {
                    continue;
                }
                var distance = candidate.Position2D.DistanceSquared(src.Settlement.Position2D);
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    tradeBound = candidate;
                }
            }

            src.TradeBound = tradeBound;
        }
    }
}
