using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyWorkDashboard.Client;
using MyWorkDashboard.Shared.Mock;
using MyWorkDashboard.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new ControlService(null));
builder.Services.AddSingleton<PageNavigatingService>();
builder.Services.AddSingleton<JsInteropService>();

var familyRepository = new MockWorkCodeFamilyRepository();
var colorRepository = new MockDutyColorRepository();
var dutyRepository = new MockDutyRepository();
var toDoRepository = new MockToDoRepository();
var preferenceRepository = new MockPreferenceRepository();

builder.Services.AddScoped(sp => new SchedulingServive(dutyRepository, familyRepository, colorRepository, toDoRepository, preferenceRepository));
builder.Services.AddScoped(sp => new UserPreferenceService(preferenceRepository));
builder.Services.AddSingleton<ClipboardService>();

builder.Services.AddAntDesign();

await builder.Build().RunAsync();
