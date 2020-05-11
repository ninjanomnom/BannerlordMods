using GantryLibInterface.Interfaces;
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace YAS
{
    public class SubModule : MBSubModuleBase
    {
        internal static SubModule Instance { get; private set; }
        internal static IGantryLib GantryLib { get; private set; }

        private const string harmonyId = "mod.ninjanomnom.yas";
        private readonly Harmony harmony;

        public SubModule()
        {
            Instance = this;
            harmony = new Harmony(harmonyId);
        }
    }
}