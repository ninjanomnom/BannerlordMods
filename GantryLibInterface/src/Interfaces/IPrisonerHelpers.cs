using JetBrains.Annotations;
using TaleWorlds.CampaignSystem;

namespace GantryLibInterface.Interfaces
{
    [PublicAPI]
    public interface IPrisonerHelpers
    {
        [PublicAPI]
        int GetNewRecruitables(MobileParty party, CharacterObject prisoner, float chanceModifier = 1);

        [PublicAPI]
        void AddNewRecruits(MobileParty party, CharacterObject prisoner, int amount);
    }
}
