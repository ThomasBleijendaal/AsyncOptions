using System;
using System.Linq;
using System.Threading.Tasks;
using AsyncOptions.ConfigurationSources;
using Microsoft.Extensions.Configuration;

namespace AsyncOptions.DependencyInjection
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAsyncOptions(this IConfigurationBuilder builder, Func<OptionsContext, Task> updateProcess)
            => builder.Add(new AsyncRefreshConfigurationSource(updateProcess));

        public static async Task BlockUntilConfiguredAsync(this IConfigurationBuilder builder)
        {
            var tasks =
                builder.Sources
                    .OfType<AsyncRefreshConfigurationSource>()
                    .Select(x => x.OptionsContext.FirstTimeConfiguring);

            await Task.WhenAll(tasks);
        }
    }
}
