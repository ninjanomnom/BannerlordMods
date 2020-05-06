using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaleWorlds.Core;

namespace GantryLib.Extensions
{
    static class EquipmentExtensions
    {
        // For reference, here's the existing function signature
        // public void GetInitialWeaponIndicesToEquip(out EquipmentIndex mainHandWeaponIndex, out EquipmentIndex offHandWeaponIndex, out bool isMainHandNotUsableWithOneHand) {}

        // This is only here to be consistent with the other function, why wouldn't you just use a list??
        /// <summary>
        /// The armor version of <see cref="Equipment.GetInitialWeaponIndicesToEquip"/>
        /// </summary>
        /// <param name="src">The <see cref="Equipment"/> this method is extending</param>
        /// <param name="head">Is either the head slot or none</param>
        /// <param name="body">Is either the body slot or none</param>
        /// <param name="leg">Is either the leg slot or none</param>
        /// <param name="gloves">Is either the gloves slot or none</param>
        /// <param name="cape">Is either the cape slot or none</param>
        public static void GetInitialArmorIndicesToEquip(
            this Equipment src,
            out EquipmentIndex head,
            out EquipmentIndex body,
            out EquipmentIndex leg,
            out EquipmentIndex gloves,
            out EquipmentIndex cape
        )
        {
            head = EquipmentIndex.None;
            body = EquipmentIndex.None;
            leg = EquipmentIndex.None;
            gloves = EquipmentIndex.None;
            cape = EquipmentIndex.None;

            if(!src[EquipmentIndex.Head].IsEmpty)
            {
                head = EquipmentIndex.Head;
            }
            if (!src[EquipmentIndex.Body].IsEmpty)
            {
                body = EquipmentIndex.Body;
            }
            if (!src[EquipmentIndex.Leg].IsEmpty)
            {
                leg = EquipmentIndex.Leg;
            }
            if (!src[EquipmentIndex.Gloves].IsEmpty)
            {
                gloves = EquipmentIndex.Gloves;
            }
            if (!src[EquipmentIndex.Cape].IsEmpty)
            {
                cape = EquipmentIndex.Cape;
            }
        }

        /// <summary>
        /// A version of <see cref="GetInitialArmorIndicesToEquip"/> that's more sensible to use
        /// </summary>
        /// <param name="src">The <see cref="Equipment"/> this method is extending</param>
        /// <returns>All of the indices that have armor</returns>
        public static IEnumerable<EquipmentIndex> GetArmorIndices(this Equipment src)
        {
            src.GetInitialArmorIndicesToEquip(out var head, out var body, out var leg, out var gloves, out var cape);
            return new EquipmentIndex[] { head, body, leg, gloves, cape }.Where(s => s != EquipmentIndex.None);
        }

        /// <summary>
        /// A version of <see cref="Equipment.GetInitialWeaponIndicesToEquip"/> that's more sensible to use
        /// </summary>
        /// <param name="src">The <see cref="Equipment"/> this method is extending</param>
        /// <returns>All of the indices that have weapons</returns>
        public static IEnumerable<EquipmentIndex> GetWeaponIndices(this Equipment src)
        {
            src.GetInitialWeaponIndicesToEquip(out var mainHand, out var offHand, out var twohanded);
            return new EquipmentIndex[] { mainHand, offHand }.Where(s => s != EquipmentIndex.None);
        }

        /// <summary>
        /// Gets all equipment indices in the class
        /// </summary>
        /// <param name="src">The <see cref="Equipment"/> this method is extending</param>
        /// <returns>All of the indices that have equipment</returns>
        public static IEnumerable<EquipmentIndex> GetEquipmentIndices(this Equipment src)
        {
            return src.GetWeaponIndices().Intersect(src.GetArmorIndices());
        }
    }
}
