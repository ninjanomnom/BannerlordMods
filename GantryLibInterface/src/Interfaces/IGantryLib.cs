using JetBrains.Annotations;

namespace GantryLibInterface.Interfaces
{
    [PublicAPI]
    public interface IGantryLib
    {
        [PublicAPI] IPrisonerHelpers PrisonerHelpers { get; }

        [PublicAPI] IExpController ExpController { get; }
    }
}
