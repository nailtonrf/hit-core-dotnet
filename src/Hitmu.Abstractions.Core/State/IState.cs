namespace Hitmu.Abstractions.Core.State
{
    /// <summary>
    ///     An persistent object.
    /// </summary>
    /// <typeparam name="TIdType"></typeparam>
    public interface IState<out TIdType> where TIdType : struct
    {
        TIdType Id { get; }
    }
}