using System;
using System.Threading.Tasks;
using AsyncOptions.ConfigurationProviders;
using Microsoft.Extensions.Configuration;

namespace AsyncOptions.ConfigurationSources
{
    internal class AsyncRefreshConfigurationSource : IConfigurationSource
    {
        private readonly Task _optionsUpdatingProcess;

        public AsyncRefreshConfigurationSource(Func<OptionsContext, Task> updateProcess)
        {
            OptionsContext = new OptionsContext();
            _optionsUpdatingProcess = Task.Run(async () => await updateProcess.Invoke(OptionsContext));
        }

        internal OptionsContext OptionsContext { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder) 
            => new AsyncRefreshConfigurationProvider(this);
    }
}
