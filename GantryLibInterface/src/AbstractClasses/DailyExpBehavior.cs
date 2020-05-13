using System;
using System.Collections.Generic;
using GantryLibInterface.Interfaces;
using JetBrains.Annotations;
using TaleWorlds.CampaignSystem;

namespace GantryLibInterface.AbstractClasses
{
    [PublicAPI]
    public abstract class DailyExpBehavior : IDailyExpBehavior
    {
        public IEnumerable<IDailyExpSource> Sources => _sources;

        private readonly List<IDailyExpSource> _sources;

        protected DailyExpBehavior() {
            _sources = new List<IDailyExpSource>();
        }

        public virtual void Register(IDailyExpSource source) {
            _sources.Add(source);
        }

        public virtual void DailyTick(MobileParty party) {
            return;
        }

        public virtual void DailyTick(Town town) {
            return;
        }
    }
}
