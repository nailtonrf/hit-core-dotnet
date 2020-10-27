using System;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Results
{
    public static class ResultAdaptersExtensions
    {
        public static T GetOrDefault<T>(this Result<T> result, T whenError = default)
        {
            return result.IsValid ? result.Value : whenError;
        }

        public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> binder)
        {
            return result.IsValid ? binder(result.Value) : Result<TOut>.Error<TOut>(result.ErrorMessage);
        }

        public static async Task<Result<TOut>> ThenAsync<TIn, TOut>(this Task<Result<TIn>> result,
            Func<TIn, Result<TOut>> binder)
        {
            var resultValue = await result.ConfigureAwait(false);
            return resultValue.Then(binder);
        }

        public static async Task<Result<TOut>> ThenAsync<TIn, TOut>(this Result<TIn> result,
            Func<TIn, Task<Result<TOut>>> binder)
        {
            return result.IsValid
                ? await binder(result.Value).ConfigureAwait(false)
                : Result<TOut>.Error<TOut>(result.ErrorMessage);
        }

        public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper)
        {
            return result.IsValid
                ? Result<TOut>.From(mapper(result.Value))
                : Result<TOut>.Error<TOut>(result.ErrorMessage);
        }

        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Task<Result<TIn>> result,
            Func<TIn, TOut> mapper)
        {
            var resultValue = await result.ConfigureAwait(false);
            return resultValue.Map(mapper);
        }

        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result,
            Func<TIn, Task<TOut>> mapper)
        {
            return result.IsValid
                ? Result<TOut>.From(await mapper(result.Value).ConfigureAwait(false))
                : Result<TOut>.Error<TOut>(result.ErrorMessage);
        }
    }
}