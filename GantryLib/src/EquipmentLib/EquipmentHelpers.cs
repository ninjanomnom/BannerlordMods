using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace GantryLib.EquipmentLib
{
    public class EquipmentHelpers : IEquipmentHelpers
    {
        public void WieldInitialArmor(Agent src)
        {
            foreach (var index in GetArmorIndices(src.SpawnEquipment))
            {
                src.Character.Equipment[index] = src.SpawnEquipment[index];
            }
        }

        public void GetInitialArmorIndicesToEquip(
            Equipment src,
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

            if (!src[EquipmentIndex.Head].IsEmpty)
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

        public IEnumerable<EquipmentIndex> GetArmorIndices(Equipment src)
        {
            GetInitialArmorIndicesToEquip(src, out var head, out var body, out var leg, out var gloves, out var cape);
            return new EquipmentIndex[] { head, body, leg, gloves, cape }.Where(s => s != EquipmentIndex.None);
        }

        public IEnumerable<EquipmentIndex> GetWeaponIndices(Equipment src)
        {
            src.GetInitialWeaponIndicesToEquip(out var mainHand, out var offHand, out var twohanded);
            return new EquipmentIndex[] { mainHand, offHand }.Where(s => s != EquipmentIndex.None);
        }

        public IEnumerable<EquipmentIndex> GetEquipmentIndices(Equipment src)
        {
            return GetWeaponIndices(src).Intersect(GetArmorIndices(src));
        }
    }
}
