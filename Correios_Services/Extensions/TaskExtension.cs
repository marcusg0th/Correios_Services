using Correios_Services.Helpers;
using System.Threading.Tasks;


namespace Correios_Services.Extensions
{
    public static class TaskExtensions
    {
        public static void RunSync(this Task task)
        {
            AsyncHelpers.RunSync(() => task);
        }

        public static T RunSync<T>(this Task<T> task)
        {
            return AsyncHelpers.RunSync(() => task);
        }
    }
}
