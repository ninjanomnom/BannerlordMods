using GantryLibInterface.Interfaces;
using HarmonyLib;
using JetBrains.Annotations;
using TaleWorlds.MountAndBlade;
using YANS.Content;
using YANS.Content.ScalingExp;

namespace YANS
{
    [UsedImplicitly]
    public class SubModule : MBSubModuleBase
    {
        internal static IGantryLib GantryLib => GantryLibInterface.GantryLibInterface.GantryLib;

        private const string HarmonyId = "mod.ninjanomnom.yans";
        private readonly Harmony _harmony;

        public SubModule() {
            _harmony = new Harmony(HarmonyId);
        }

        protected override void OnSubModuleLoad() {
            base.OnSubModuleLoad();
            _harmony.PatchAll();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot() {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            GantryLib.ExpController.ReplaceDefaultBehavior<ScalingBehavior>();
        }
    }
}
