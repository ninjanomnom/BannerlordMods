using System;
using System.Collections.Generic;
using GantryLib.ExperienceLib.Defaults;
using GantryLibInterface.Interfaces;
using TaleWorlds.CampaignSystem;

namespace GantryLib.ExpUnification
{
    internal class ExpController : IExpController
    {
        private Dictionary<Type, IDailyExpBehavior> Controllers { get; }

        internal ExpController() {
            Controllers = new Dictionary<Type, IDailyExpBehavior>();
        }

        internal void DailyTick(Town town) {
            foreach (var controller in Controllers) {
                controller.Value.DailyTick(town);
            }
        }

        internal void DailyTick(MobileParty party) {
            var expController = SubModule.Instance.InternalExpController;
            foreach (var controller in expController.Controllers) {
                controller.Value.DailyTick(party);
            }
        }

        public void Register(IDailyExpSource source) {
            Register<DefaultDailyExpBehavior>(source);
        }

        public void Register<TController>(IDailyExpSource source, bool replaceDefault = false)
            where TController : IDailyExpBehavior, new() {
            var controller = Controllers[typeof(TController)];
            if (controller is null) {
                Controllers.Add(typeof(TController), new TController());
                controller = Controllers[typeof(TController)];
            }

            controller.Register(source);
        }

        public void ReplaceDefaultBehavior<TNew>(bool keepSources = true)
            where TNew : IDailyExpBehavior, new() {
            ReplaceBehavior<DefaultDailyExpBehavior, TNew>(keepSources);
        }

        public void ReplaceBehavior<TOld, TNew>(bool keepSources = true)
            where TOld : IDailyExpBehavior, new()
            where TNew : IDailyExpBehavior, new() {
            var newBehavior = new TNew();
            if (Controllers.TryGetValue(typeof(TOld), out var oldBehavior) && keepSources) {
                foreach (var behavior in oldBehavior.Sources) {
                    newBehavior.Register(behavior);
                }
            }

            Controllers.Add(typeof(TNew), newBehavior);
        }
    }
}
