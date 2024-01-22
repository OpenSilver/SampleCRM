using System;
using System.Threading.Tasks;

namespace SampleCRM.Web.Views
{
    public class AsyncHelper
    {
        public static IBusyCapablePage ContentPage { get; set; }

        public static async Task RunAsync<TParam>(Func<TParam, Task> action, TParam parameter)
        {
            await RunAsync(async () => await action(parameter));
        }

        public static async Task RunAsync(Func<Task> action)
        {
            try
            {
                if (ContentPage != null)
                    ContentPage.MakeBusy(true);

                await action();

                if (ContentPage != null)
                    ContentPage.MakeBusy(false);
            }
            catch (Exception ex)
            {
                if (ContentPage != null)
                    ContentPage.MakeBusy(false);

                throw ex;
            }
        }
    }
}