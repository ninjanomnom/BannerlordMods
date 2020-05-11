namespace GantryLibInterface.Interfaces
{
    public interface IGantryLib
    {
        IDailyExpHelpers ExpHelpers { get; }

        IPrisonerHelpers PrisonerHelpers { get; }
    }
}