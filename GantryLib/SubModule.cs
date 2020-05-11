using GantryLib.ExpUnification;
using GantryLib.PrisonerUnification;
using GantryLibInterface.Interfaces;
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace GantryLib
{
    public class SubModule : MBSubModuleBase, IGantryLib
    {
        public IDailyExpHelpers ExpHelpers { get; private set; }
        public IPrisonerHelpers PrisonerHelpers { get; private set; }

        internal static IGantryLib Instance { get; private set; }

        private const string harmonyId = "mod.ninjanomnom.GantryLib";
        private readonly Harmony harmony;

        public SubModule()
        {
            Instance = this;
            harmony = new Harmony(harmonyId);
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            ExpHelpers = new DailyExpHelpers();
            PrisonerHelpers = new PrisonerUnificationHelpers();

            harmony.PatchAll();
        }
    }
}