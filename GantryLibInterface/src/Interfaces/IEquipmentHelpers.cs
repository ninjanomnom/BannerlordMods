using System.Collections.Generic;
using JetBrains.Annotations;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace GantryLib.EquipmentLib
{
    [PublicAPI]
    public interface IEquipmentHelpers
    {
        /// <summary>
        /// Same as <see cref="Agent.WieldInitialWeapons"/> but for armor
        /// </summary>
        /// <param name="src">The agent instance this method is extending</param>
        [PublicAPI]
        void WieldInitialArmor([NotNull] Agent src);

        /// <summary>
        /// The armor version of <see cref="Equipment.GetInitialWeaponIndicesToEquip"/>
        /// </summary>
        /// <param name="src">The <see cref="Equipment"/> this method is extending</param>
        /// <param name="head">Is either the head slot or none</param>
        /// <param name="body">Is either the body slot or none</param>
        /// <param name="leg">Is either the leg slot or none</param>
        /// <param name="gloves">Is either the gloves slot or none</param>
        /// <param name="cape">Is either the cape slot or none</param>
        [PublicAPI]
        void GetInitialArmorIndicesToEquip(
            [NotNull] Equipment src, out EquipmentIndex head, out EquipmentIndex body,
            out EquipmentIndex leg, out EquipmentIndex gloves, out EquipmentIndex cape
        );

        /// <summary>
        /// A version of <see cref="GetInitialArmorIndicesToEquip"/> that's more sensible to use
        /// </summary>
        /// <param name="src">The <see cref="Equipment"/> this method is extending</param>
        /// <returns>All of the indices that have armor</returns>
        [PublicAPI]
        IEnumerable<EquipmentIndex> GetArmorIndices([NotNull] Equipment src);

        /// <summary>
        /// A version of <see cref="Equipment.GetInitialWeaponIndicesToEquip"/> that's more sensible to use
        /// </summary>
        /// <param name="src">The <see cref="Equipment"/> this method is extending</param>
        /// <returns>All of the indices that have weapons</returns>
        [PublicAPI]
        IEnumerable<EquipmentIndex> GetWeaponIndices([NotNull] Equipment src);

        /// <summary>
        /// Gets all equipment indices in the class
        /// </summary>
        /// <param name="src">The <see cref="Equipment"/> this method is extending</param>
        /// <returns>All of the indices that have equipment</returns>
        [PublicAPI]
        IEnumerable<EquipmentIndex> GetEquipmentIndices([NotNull] Equipment src);
    }
}
