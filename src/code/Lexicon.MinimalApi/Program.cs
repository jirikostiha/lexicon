using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Serilog.Events;
using Serilog;
using System.IO;
using System.Reflection;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Lexicon.DependencyInjection.Autofac;
using Lexicon;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web host.");
    Log.Information("WorkingDir: {0}", Directory.GetCurrentDirectory());
    
    var appPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program))?.Location);
    Log.Information("AppPath:    {0}", appPath);

    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        Args = args
    });

    builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.Sources.Clear();
        var env = hostingContext.HostingEnvironment;
        Log.Information("HostingEnvironment: {0}", env.EnvironmentName);

        config.SetBasePath(appPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: false)
            .AddCommandLine(args);
    });

    builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    {
        //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1#host-configuration
        loggerConfiguration
            .ReadFrom.Configuration(hostingContext.Configuration)
            .Enrich.FromLogContext();
#if DEBUG
        loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
    });

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.Host.ConfigureContainer<ContainerBuilder>((context, builder) =>
    {
        builder.RegisterModule(new CoreModule(context.Configuration));
    });

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "Lexicon API",
                Description = "Words providing service.",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Jiri Kostiha",
                    Email = "ijkdata@gmail.com",
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://github.com/jirikostiha/lexicon/blob/main/license.txt")
                },
            }); ;

        //generate xml docs
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        options.IncludeXmlComments(xmlPath);

        options.CustomOperationIds(apiDescription 
            => apiDescription.TryGetMethodInfo(out MethodInfo mi) ? mi.Name : null);
    });

    builder.Services.AddApiVersioning(o =>
    {
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = ApiVersion.Default;
        o.ReportApiVersions = true;
    });

    builder.Services.AddHealthChecks();

    //https://developers.de/blogs/holger_vetter/archive/2016/11/30/identify-asp-net-core-startup-errors.aspx
    builder.WebHost.UseSetting("detailedErrors", "true");

    builder.WebHost.CaptureStartupErrors(true);

    builder.WebHost.UseKestrel(kestrelOptions =>
    {
        kestrelOptions.Limits.MaxConcurrentConnections = 100;
        kestrelOptions.Limits.MaxConcurrentUpgradedConnections = 100;
        kestrelOptions.Limits.MaxRequestBodySize = 52_428_800;
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lexicon.WebApi v1");
            c.DisplayOperationId();
        });
    }
    else
    {
        app.UseExceptionHandler(errorApp =>
        {
            // Logs unhandled exceptions
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-5.0
            errorApp.Run(async context =>
            {
                // Return machine-readable problem details. See RFC 7807 for details.
                // https://datatracker.ietf.org/doc/html/rfc7807#page-6
                var pd = new ProblemDetails
                {
                    Type = "https://demo.api.com/errors/internal-server-error",
                    Title = "An unrecoverable error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                };
                pd.Extensions.Add("RequestId", context.TraceIdentifier);
                await context.Response.WriteAsJsonAsync(pd, pd.GetType(), null, contentType: "application/problem+json");
            });
        });
    }

    app.UseRouting();

    app.MapControllers();

    app.Run();
}
catch (OperationCanceledException)
{
    Log.Warning("Cancelled.");

    return ExitCode.Canceled;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");

    return ExitCode.GeneralError;
}
finally
{
    Log.CloseAndFlush();
}

return ExitCode.Ok;
