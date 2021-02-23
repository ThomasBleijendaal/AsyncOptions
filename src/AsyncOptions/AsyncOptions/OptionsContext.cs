using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncOptions
{
    public sealed class OptionsContext
    {
        private readonly TaskCompletionSource<int> _tcs = new();

        public void UpdateConfiguration(IEnumerable<KeyValuePair<string, string>> config)
        {
            Config = config;
            OnConfigurationUpdate?.Invoke(this, new EventArgs());

            if (!_tcs.Task.IsCompleted)
            {
                _tcs.SetResult(1);
            }
        }

        internal IEnumerable<KeyValuePair<string, string>>? Config;

        internal event EventHandler? OnConfigurationUpdate;

        internal Task FirstTimeConfiguring => _tcs.Task;
    }
}
