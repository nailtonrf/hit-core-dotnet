using System;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Core.Results
{
    public static class ResultDeadEndExtensions
    {
        public static Result<T> Do<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsValid) action(result.Value);
            return result;
        }

        public static async Task<Result<T>> DoAsync<T>(this Result<T> result, Func<T, Task> asyncTask)
        {
            if (result.IsValid) await asyncTask(result.Value).ConfigureAwait(false);
            return result;
        }

        public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<T, Task> asyncTask)
        {
            var resultValue = await result;
            if (resultValue.IsValid) await asyncTask(resultValue.Value).ConfigureAwait(false);
            return resultValue;
        }

        public static Result<T> DoWhenError<T>(this Result<T> result, Action<ErrorMessage> action)
        {
            if (result.IsInvalid) action(result.ErrorMessage);
            return result;
        }

        public static async Task<Result<T>> DoWhenErrorAsync<T>(this Task<Result<T>> result, Func<T, Task> asyncTask)
        {
            var resultValue = await result.ConfigureAwait(false);
            if (resultValue.IsInvalid) await asyncTask(resultValue.Value).ConfigureAwait(false);
            return resultValue;
        }

        public static async Task<Result<T>> DoWhenErrorAsync<T>(this Result<T> result, Func<T, Task> asyncTask)
        {
            if (result.IsInvalid) await asyncTask(result.Value).ConfigureAwait(false);
            return result;
        }
    }
}
