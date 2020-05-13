using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TaleWorlds.CampaignSystem;

namespace GantryLibInterface.Interfaces
{
    [PublicAPI]
    public interface IDailyExpBehavior
    {
        IEnumerable<IDailyExpSource> Sources { get; }

        void Register(IDailyExpSource source);

        void DailyTick(MobileParty party);

        void DailyTick(Town party);
    }
}
