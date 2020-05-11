using GantryLibInterface.Interfaces;
using HarmonyLib;
using TaleWorlds.MountAndBlade;
using YANS.Content;

namespace YANS
{
    public class SubModule : MBSubModuleBase
    {
        internal static SubModule Instance { get; private set; }
        internal static IGantryLib GantryLib => GantryLibInterface.GantryLibInterface.GantryLib;

        private const string harmonyId = "mod.ninjanomnom.yans";
        private readonly Harmony harmony;

        public SubModule()
        {
            Instance = this;
            harmony = new Harmony(harmonyId);
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            harmony.PatchAll();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            GantryLib.ExpHelpers.Register(new PartyTraining());
        }
    }
}