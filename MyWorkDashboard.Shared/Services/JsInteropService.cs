using Microsoft.JSInterop;

namespace MyWorkDashboard.Shared.Services
{

    public class JsInteropService : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public JsInteropService(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/MyWorkDashboard.Shared/mywork.js").AsTask());
        }

        public async Task ScrollScheduleFrame(int scrollValue)
        {
            try
            {
                var module = await moduleTask.Value;
                await module.InvokeVoidAsync("scroll", scrollValue);
            }
            catch
            {
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}