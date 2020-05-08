using GantryLib.ExpUnification;
using HarmonyLib;
using TaleWorlds.MountAndBlade;
using YANS.Content;

namespace YANS
{
    public class SubModule : MBSubModuleBase
    {
        private const string harmonyId = "mod.ninjanomnom.yans";
        private readonly Harmony harmony;

        public SubModule()
        {
            harmony = new Harmony(harmonyId);
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            harmony.PatchAll();

            DailyExpUnification.Register(new PartyTraining());
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            harmony.UnpatchAll(harmonyId);
        }
    }
}