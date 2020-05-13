using GantryLibInterface.Interfaces;
using System.Linq;
using JetBrains.Annotations;
using TaleWorlds.MountAndBlade;

namespace GantryLibInterface
{
    [PublicAPI]
    public static class GantryLibInterface
    {
        private static IGantryLib _gantryLib;

        public static IGantryLib GantryLib => GetOrSetup();

        private static IGantryLib GetOrSetup() {
            if (_gantryLib is null) {
                var gantryLib = Module.CurrentModule.SubModules.Where(s => s is IGantryLib).Single();
                _gantryLib = (IGantryLib) gantryLib;
            }

            return _gantryLib;
        }
    }
}
