using Imarat_Shariah.Components;
using Imarat_Shariah.Data.Repositories;
using Imarat_Shariah.Services.Interfaces;
using Imarat_Shariah.Services;
using Imarat_Shariah.Data;
using Microsoft.EntityFrameworkCore;
using Imarat_Shariah.Services.Logger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DATABSE CONNECTION
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DB_CONNECTION")));

// Add the custom logger provider
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new DatabaseLoggerProvider(builder.Services.BuildServiceProvider()));


// REGISTER REPOSITORIES AND SERVICES
builder.Services.AddScoped<ITimeConversion, TimeConversion>();
builder.Services.AddScoped<IKhulaService, KhulaService>();
builder.Services.AddScoped<ISiyajatService, SiyajatService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IKhulaRepository, KhulaRepository>();
builder.Services.AddScoped<ISiyajatRepository, SiyajatRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IFileManagement, FileManagement>();
builder.Services.AddScoped<IStorageService, LocalStorageService>();
builder.Services.AddScoped<FileManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
