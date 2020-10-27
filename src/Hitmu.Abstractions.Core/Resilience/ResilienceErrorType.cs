namespace Hitmu.Abstractions.Core.Resilience
{
    /// <summary>
    ///     Classification of an error.
    /// </summary>
    public enum ResilienceErrorType
    {
        /// <summary>
        ///     Retryable.
        /// </summary>
        Transient = 1,

        /// <summary>
        ///     Not retryable.
        /// </summary>
        Permanent = 2
    }
}