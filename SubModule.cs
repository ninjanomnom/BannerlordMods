using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.TwoDimension;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using TaleWorlds.Library;

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
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            harmony.UnpatchAll(harmonyId);
        }
    }
}