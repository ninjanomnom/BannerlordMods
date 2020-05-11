using TaleWorlds.CampaignSystem;

namespace GantryLibInterface.Interfaces
{
    public interface IPrisonerHelpers
    {
        int GetNewRecruitables(MobileParty party, CharacterObject prisoner, float chanceModifier = 1);

        void AddNewRecruits(MobileParty party, CharacterObject prisoner, int amount);
    }
}