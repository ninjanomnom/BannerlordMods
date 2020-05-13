using GantryLibInterface.Interfaces;
using JetBrains.Annotations;
using TaleWorlds.CampaignSystem;

namespace GantryLibInterface.AbstractClasses
{
    [PublicAPI]
    public abstract class DailyExpSource : IDailyExpSource
    {
        public virtual float Generic(MobileParty party) {
            return 0;
        }

        public virtual float Town(Town town) {
            return 0;
        }

        public virtual float Party(MobileParty party) {
            return 0;
        }

        public virtual float Character(MobileParty party, int characterIndex) {
            return 0;
        }
    }
}
