using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDaemon.Extensions.Logging;
using NetDaemon.Extensions.Tts;
using NetDaemon.Runtime;
using System.Reflection;

try
{
    
    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .UseNetDaemonAppSettings()
        .UseNetDaemonDefaultLogging()
        .UseNetDaemonRuntime()
        .UseNetDaemonTextToSpeech()
        .UseNetDaemonMqttEntityManagement()
        .ConfigureServices((_, services) =>
        {
            services
                .AddAppsFromAssembly(Assembly.GetExecutingAssembly())
                .AddNetDaemonStateManager()
                .AddNetDaemonScheduler()
                .AddHomeAssistantGenerated();
        });


    // adding MVC / WebAPI controllers
    builder.Services.AddControllers();

    // Blazor Server
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();


    // Kestrel on port 10000
    // TODO: SSL
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(10000);
    });

    var app = builder.Build();

    // minimal API
    app.MapGet("/mini", () => "Hello from Minimal API!");


    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    //app.UseHttpsRedirection();

    // use controller routes
    app.MapControllers();

    // configure Blazor
    app.UseStaticFiles();
    app.UseRouting();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    await app.RunAsync();

}
catch (Exception e)
{
    Console.WriteLine($"Failed to start host... {e}");
    throw;
}
