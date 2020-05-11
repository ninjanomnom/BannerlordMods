using TaleWorlds.CampaignSystem;

namespace GantryLibInterface.Interfaces
{
    public interface IDailyExpSource
    {
        /// <summary>
        /// Called in all situations for all troops
        /// </summary>
        /// <param name="party">The party holding the troops</param>
        /// <param name="active">The active exp bonus for those troops</param>
        /// <param name="passive">The passive exp bonus for those troops</param>
        void Generic(MobileParty party, out float active, out float passive);

        /// <summary>
        /// Called on parties in towns
        /// </summary>
        /// <param name="party">The party holding the troops</param>
        /// <param name="town">The town they are in</param>
        /// <param name="active">The active exp bonus for those troops</param>
        /// <param name="passive">The passive exp bonus for those troops</param>
        void Town(MobileParty party, Town town, out float active, out float passive);

        /// <summary>
        /// Called on moving parties on the map
        /// </summary>
        /// <param name="party">The party moving and holding the troops</param>
        /// <param name="active">The active exp bonus for those troops</param>
        /// <param name="passive">The passive exp bonus for those troops</param>
        void Party(MobileParty party, out float active, out float passive);

        /// <summary>
        /// Called on individual troops in all situations
        /// </summary>
        /// <param name="party">The party moving and holding the troops</param>
        /// <param name="characterIndex">The index of the character in the party roster</param>
        /// <param name="active">The active exp bonus for the character</param>
        /// <param name="passive">The passive exp bonus for the character</param>
        void Character(MobileParty party, int characterIndex, out float active, out float passive);
    }
}