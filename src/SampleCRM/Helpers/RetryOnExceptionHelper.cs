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
        public static IBusyCapablePage ContentPage { get; set; }

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
                    if (ContentPage != null)
                        ContentPage.MakeBusy(true);

                    await action();
                    
                    if (ContentPage != null)
                        ContentPage.MakeBusy(false);
                    
                    return;
                }
                catch (Exception ex)
                {
                    retryCount++;

                    if (ContentPage != null)
                        ContentPage.MakeBusy(false);

                    if (retryCount >= maxRetryCount)
                        throw ex;
                }
            } while (retryCount < maxRetryCount);
        }
    }

    public interface IBusyCapablePage
    {
        void MakeBusy(bool isBusy);
    }
}