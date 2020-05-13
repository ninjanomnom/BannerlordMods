using GantryLib.ExpUnification;
using GantryLib.PrisonerUnification;
using GantryLibInterface.Interfaces;
using HarmonyLib;
using JetBrains.Annotations;
using TaleWorlds.MountAndBlade;

namespace GantryLib
{
    [UsedImplicitly]
    public class SubModule : MBSubModuleBase, IGantryLib
    {
        public IPrisonerHelpers PrisonerHelpers { get; private set; }

        public IExpController ExpController => InternalExpController;
        internal ExpController InternalExpController { get; private set; }

        internal static SubModule Instance { get; private set; }

        private const string HarmonyId = "mod.ninjanomnom.GantryLib";
        private readonly Harmony _harmony;

        public SubModule() {
            Instance = this;
            _harmony = new Harmony(HarmonyId);
        }

        protected override void OnSubModuleLoad() {
            base.OnSubModuleLoad();

            PrisonerHelpers = new PrisonerUnificationHelpers();

            InternalExpController = new ExpController();

            _harmony.PatchAll();
        }
    }
}
