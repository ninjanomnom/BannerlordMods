using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace GantryLibInterface.Interfaces
{
    public interface IDailyExpHelpers
    {
        List<IDailyExpSource> Sources { get; }

        void Register(IDailyExpSource source);

        void GiveExpToGroup(MobileParty party, float activeExp, float passiveExp);
    }
}