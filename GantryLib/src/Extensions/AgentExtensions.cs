using GantryLib.Extensions;
using TaleWorlds.MountAndBlade;

namespace GantryLib.src.Extensions
{
    public static class AgentExtensions
    {
        /// <summary>
        /// Same as <see cref="Agent.WieldInitialWeapons"/> but for armor
        /// </summary>
        /// <param name="src">The agent instance this method is extending</param>
        public static void WieldInitialArmor(this Agent src)
        {
            foreach (var index in src.SpawnEquipment.GetArmorIndices())
            {
                src.Character.Equipment[index] = src.SpawnEquipment[index];
            }
        }
    }
}
