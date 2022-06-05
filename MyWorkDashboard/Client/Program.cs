using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyWorkDashboard.Client;
using MyWorkDashboard.Shared.Mock;
using MyWorkDashboard.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new SchedulingServive(
    new MockDutyRepository(),
    new MockWorkCodeFamilyRepository(),
    new MockDutyColorRepository(),
    new MockToDoRepository()));
builder.Services.AddScoped(sp => new ControlService(null));
builder.Services.AddSingleton<PageNavigatingService>();

builder.Services.AddAntDesign();

await builder.Build().RunAsync();
