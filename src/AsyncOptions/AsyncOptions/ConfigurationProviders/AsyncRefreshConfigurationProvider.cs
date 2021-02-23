using System;
using System.Linq;
using AsyncOptions.ConfigurationSources;
using Microsoft.Extensions.Configuration;

namespace AsyncOptions.ConfigurationProviders
{
    internal class AsyncRefreshConfigurationProvider : ConfigurationProvider
    {
        internal AsyncRefreshConfigurationSource Source { get; }

        public AsyncRefreshConfigurationProvider(
            AsyncRefreshConfigurationSource source,
            IConfigurationBuilder builder)
        {
            Source = source;
            Source.OptionsContext.OnConfigurationUpdate += OptionsContext_OnConfigurationUpdate;
        }

        private void OptionsContext_OnConfigurationUpdate(object sender, EventArgs e) => Load();

        public override void Load()
        {
            if (Source.OptionsContext.Config != null)
            {
                Data = Source.OptionsContext.Config.ToDictionary(x => x.Key, x => x.Value);
            }

            OnReload();
        }
    }
}
