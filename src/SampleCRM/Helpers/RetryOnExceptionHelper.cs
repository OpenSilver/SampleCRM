using System;
using System.Threading.Tasks;

namespace SampleCRM.Web.Views
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class RetryOnExceptionAttribute : Attribute
    {
        public int MaxRetryCount { get; }
        public Type ExceptionType { get; }

        public RetryOnExceptionAttribute(int maxRetryCount, Type exceptionType)
        {
            MaxRetryCount = maxRetryCount;
            ExceptionType = exceptionType;
        }
    }

    public class RetryOnExceptionHelper
    {
        public static async Task RetryAsync<TParam>(Func<TParam, Task> action, TParam parameter, int maxRetryCount = 3)
        {
            await RetryAsync(async () => await action(parameter), maxRetryCount);
        }

        public static async Task RetryAsync(Func<Task> action, int maxRetryCount = 3)
        {
            int retryCount = 0;
            do
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception)
                {
                    retryCount++;
                }
            } while (retryCount < maxRetryCount);
        }
    }
}