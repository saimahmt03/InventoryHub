using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using InventoryHubClient;
using InventoryHubClient.Service;
using InventoryHubClient.Service.Storage;
using Blazored.LocalStorage;
using Blazored.SessionStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<ISessionStorage, SessionStorage>();
builder.Services.AddScoped<IResponseStatusHandler, ResponseStatusHandler>();

// This prevent socket exhaustion issue.
// The HttpClient is created and managed by the factory, which handles the underlying socket connections.
builder.Services.AddHttpClient("InventoryHubAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5271/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


await builder.Build().RunAsync();
