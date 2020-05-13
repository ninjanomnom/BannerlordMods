using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TaleWorlds.CampaignSystem;

namespace GantryLibInterface.Interfaces
{
    [PublicAPI]
    public interface IExpController
    {
        [PublicAPI]
        void Register(IDailyExpSource source);

        [PublicAPI]
        void Register<TController>(IDailyExpSource source, bool replaceDefault = false)
            where TController : IDailyExpBehavior, new();

        [PublicAPI]
        void ReplaceDefaultBehavior<TNew>(bool keepSources = true)
            where TNew : IDailyExpBehavior, new();

        [PublicAPI]
        void ReplaceBehavior<TOld, TNew>(bool keepSources = true)
            where TOld : IDailyExpBehavior, new()
            where TNew : IDailyExpBehavior, new();
    }
}
