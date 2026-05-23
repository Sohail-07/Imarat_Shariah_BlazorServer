using Imarat_Shariah.Components;
using Imarat_Shariah.Data;
using Imarat_Shariah.Data.Repositories;
using Imarat_Shariah.Services;
using Imarat_Shariah.Services.Interfaces;
using Imarat_Shariah.Services.Logger;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

// ASP.NET Core Identity Services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Blazor Server Authentication Setup
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();

// Register the custom persistent circuit revalidator
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

// Fine-Grained Policies Setup
builder.Services.AddAuthorizationCore(options =>
{
    // Siyajat Dynamic Policies
    options.AddPolicy("CanViewSiyajat", p => p.RequireClaim("Permission", "Permissions.Siyajat.View"));
    options.AddPolicy("CanCreateSiyajat", p => p.RequireClaim("Permission", "Permissions.Siyajat.Create"));
    options.AddPolicy("CanUpdateSiyajat", p => p.RequireClaim("Permission", "Permissions.Siyajat.Update"));
    options.AddPolicy("CanDeleteSiyajat", p => p.RequireClaim("Permission", "Permissions.Siyajat.Delete"));

    // Khula Dynamic Policies
    options.AddPolicy("CanViewKhula", p => p.RequireClaim("Permission", "Permissions.Khula.View"));
    options.AddPolicy("CanCreateKhula", p => p.RequireClaim("Permission", "Permissions.Khula.Create"));
    options.AddPolicy("CanUpdateKhula", p => p.RequireClaim("Permission", "Permissions.Khula.Update"));
    options.AddPolicy("CanDeleteKhula", p => p.RequireClaim("Permission", "Permissions.Khula.Delete"));
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await DatabaseSeeder.SeedAdminUserAsync(app.Services.CreateScope().ServiceProvider);

app.Run();
