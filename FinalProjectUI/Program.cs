using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FinalProject;
using Syncfusion.Blazor;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBPh8sVXJ9S0d+X1JPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9mSXtTd0ViW35beHZRQWVXU00=;Mgo+DSMBMAY9C3t2XFhhQlJHfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hTH5WdkRjWH1fdXxXQGNdWkZ/;Mzg0MjUwOEAzMjM5MmUzMDJlMzAzYjMyMzkzYmdoa0Zaem1TQ2ZyS0FjaTVkOS9QY1NJbU9PWU53N3VkYTRIcXhNK0FDMmc9");

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSyncfusionBlazor();

builder.Services.AddScoped(sp => new HttpClient 
    { 
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
    }
);

builder.Services.AddScoped(sp => new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5000/api/")
    }
);
builder.Services.AddScoped<IStockDataService, StockDataService>();

await builder.Build().RunAsync();
