using JetBrains.Annotations;
using TaleWorlds.CampaignSystem;

namespace GantryLibInterface.Interfaces
{
    [PublicAPI]
    public interface IDailyExpSource
    {
        /// <summary>
        /// Called in all situations for all troops
        /// </summary>
        /// <param name="party">The party holding the troops</param>
        float Generic(MobileParty party);

        /// <summary>
        /// Called on parties in towns
        /// </summary>
        /// <param name="town">The town they are in</param>
        float Town(Town town);

        /// <summary>
        /// Called on moving parties on the map
        /// </summary>
        /// <param name="party">The party moving and holding the troops</param>
        float Party(MobileParty party);

        /// <summary>
        /// Called on individual troops in all situations
        /// </summary>
        /// <param name="party">The party moving and holding the troops</param>
        /// <param name="characterIndex">The index of the character in the party roster</param>
        float Character(MobileParty party, int characterIndex);
    }
}
