using OpenRiaServices.DomainServices.Client;
using System;
using System.Threading.Tasks;

namespace SampleCRM.Web.Views
{
    public static class AsyncHelper
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
                MakeBusy(true);

                await action();

                MakeBusy(false);
            }
            catch (Exception ex)
            {
                MakeBusy(false);

                throw ex;
            }
        }

        public static void MakeBusy(bool busy)
        {
            ContentPage?.MakeBusy(busy);
        }

        public static void Load<TEntity>(this DomainContext context, EntityQuery<TEntity> query, Action<LoadOperation<TEntity>> callback) where TEntity : Entity
        {
            MakeBusy(true);
            context.Load(query, result =>
            {
                callback(result);
                MakeBusy(false);
            }, null);
        }
    }
}