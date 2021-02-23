using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AsyncOptions.DependencyInjection;

namespace AsyncOptions.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Config:Static", "thisisstatic" }
                })
                .AddAsyncOptions(async (context) =>
                {
                    do
                    {
                        await Task.Delay(1234);

                        context.UpdateConfiguration(new Dictionary<string, string>
                        {
                            { "Config:Dynamic", $"{DateTime.Now}" }
                        });
                    }
                    while (true);
                });

            var config = configBuilder.Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddOptions<ProgramConfig>().Bind(config.GetSection("Config"));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var fixedOptions = serviceProvider.GetRequiredService<IOptions<ProgramConfig>>();
            var monitorOptions = serviceProvider.GetRequiredService<IOptionsMonitor<ProgramConfig>>();
            var optionsFactory = serviceProvider.GetRequiredService<IOptionsFactory<ProgramConfig>>();
            do
            {
                using var scope = serviceProvider.CreateScope();

                var snapshotOptions = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ProgramConfig>>();
                var factoryOptions = optionsFactory.Create(string.Empty);

                Console.WriteLine($"Fixed config:\t\t| Static: {fixedOptions.Value.Static}\t\t| Dynamic: {fixedOptions.Value.Dynamic}.");
                Console.WriteLine($"Monitor config:\t\t| Static: {monitorOptions.CurrentValue.Static}\t\t| Dynamic: {monitorOptions.CurrentValue.Dynamic}.");
                Console.WriteLine($"Snapshot config:\t| Static: {snapshotOptions.Value.Static}\t\t| Dynamic: {snapshotOptions.Value.Dynamic}.");
                Console.WriteLine($"Factory config:\t\t| Static: {factoryOptions.Static}\t\t| Dynamic: {factoryOptions.Dynamic}.");

                await Task.Delay(500);
            }
            while (true);
        }

        class ProgramConfig
        {
            public string? Static { get; set; }
            public string? Dynamic { get; set; }
        }
    }
}
