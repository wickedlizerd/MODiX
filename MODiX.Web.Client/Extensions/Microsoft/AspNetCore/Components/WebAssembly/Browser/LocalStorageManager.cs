using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Microsoft.AspNetCore.Components.WebAssembly.Browser
{
    public interface ILocalStorageManager
    {
        ValueTask SetValueAsync<T>(string key, T value)
            where T : notnull;

        ValueTask<T?> TryGetObjectAsync<T>(string key)
            where T : class;

        ValueTask<T?> TryGetValueAsync<T>(string key)
            where T : struct;

        ValueTask RemoveKeyAsync(string key);
    }

    public class LocalStorageManager
        : ILocalStorageManager
    {
        public LocalStorageManager(IJSRuntime jsRuntime)
            => _jsRuntime = jsRuntime;

        public ValueTask SetValueAsync<T>(string key, T value)
                where T : notnull
            => _jsRuntime.InvokeVoidAsync("window.localStorage.setItem", key, JsonSerializer.Serialize(value));

        public async ValueTask<T?> TryGetObjectAsync<T>(string key)
            where T : class
        {
            var item = await _jsRuntime.InvokeAsync<string?>("window.localStorage.getItem", key);

            return (item == null)
                ? default
                : JsonSerializer.Deserialize<T>(item);
        }

        public async ValueTask<T?> TryGetValueAsync<T>(string key)
            where T : struct
        {
            var item = await _jsRuntime.InvokeAsync<string?>("window.localStorage.getItem", key);

            return (item == null)
                ? default
                : JsonSerializer.Deserialize<T>(item);
        }

        public ValueTask RemoveKeyAsync(string key)
            => _jsRuntime.InvokeVoidAsync("window.localStorage.removeItem", key);

        private readonly IJSRuntime _jsRuntime;
    }
}
