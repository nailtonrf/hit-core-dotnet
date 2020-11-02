namespace Hitmu.Abstractions.Core.Results
{
    public interface IResult
    {
        bool IsValid { get; }
        ErrorMessage ErrorMessage { get; }
    }
}