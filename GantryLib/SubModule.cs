using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace GantryLib
{
    public class SubModule : MBSubModuleBase
    {
        private const string harmonyId = "mod.ninjanomnom.GantryLib";
        private readonly Harmony harmony;

        public SubModule()
        {
            harmony = new Harmony(harmonyId);
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            harmony.PatchAll();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            harmony.UnpatchAll(harmonyId);
        }
    }
}