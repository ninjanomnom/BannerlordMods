using GantryLibInterface.Interfaces;
using TaleWorlds.CampaignSystem;

namespace GantryLibInterface.AbstractClasses
{
    public abstract class DailyExpSource : IDailyExpSource
    {
        public virtual void Generic(MobileParty party, out float active, out float passive)
        {
            active = 0;
            passive = 0;
        }

        public virtual void Town(MobileParty party, Town town, out float active, out float passive)
        {
            active = 0;
            passive = 0;
        }

        public virtual void Party(MobileParty party, out float active, out float passive)
        {
            active = 0;
            passive = 0;
        }

        public virtual void Character(MobileParty party, int characterIndex, out float active, out float passive)
        {
            active = 0;
            passive = 0;
        }
    }
}
