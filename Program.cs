using dafsem.Context;
using dafsem.Models;
using dafsem.Services;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Localization servisleri
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Custom Cookie Request Culture Provider
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("tr-TR"),
        new CultureInfo("en-US"),
        new CultureInfo("ar-SA")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // Cookie-based culture provider'ı ekle
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new CookieRequestCultureProvider()
    {
        CookieName = "PanelLanguage"
    });
});

builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders().AddRoles<IdentityRole>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RootRole", policy =>
        policy.RequireRole("RootRole"));
    options.AddPolicy("AdminRole", policy =>
        policy.RequireRole("AdminRole"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Home/AccessDenied";
});

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// Tüm servisler...
builder.Services.AddScoped<SayfaService>();
builder.Services.AddScoped<AyarService, AyarService>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IBaslikService, BaslikService>();
builder.Services.AddScoped<ITarihlerService, TarihlerService>();
builder.Services.AddScoped<IBankaBilgileriService, BankaBilgileriService>();
builder.Services.AddScoped<IUnvanlarService, UnvanlarService>();
builder.Services.AddScoped<IParaBirimiService, ParaBirimiService>();
builder.Services.AddScoped<IUcretlerService, UcretlerService>();
builder.Services.AddScoped<IKonaklamaService, KonaklamaService>();
builder.Services.AddScoped<IOdaTipleriService, OdaTipleriService>();
builder.Services.AddScoped<IHizmetTuruService, HizmetTuruService>();
builder.Services.AddScoped<IHizmetlerService, HizmetlerService>();
builder.Services.AddScoped<IAltSayfaService, AltSayfaService>();
builder.Services.AddScoped<IKuralTuruService, KuralTuruService>();
builder.Services.AddScoped<IKurallarService, KurallarService>();
builder.Services.AddScoped<IBasvuruService, BasvuruService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IIletisimService, IletisimService>();
builder.Services.AddScoped<ITelefonlarService, TelefonlarService>();
builder.Services.AddScoped<IAyarlarService, AyarlarService>();
builder.Services.AddScoped<ISayfalarService, SayfalarService>();
builder.Services.AddScoped<IKurulKategorileriService, KurulKategorileriService>();
builder.Services.AddScoped<IKurulUyeleriService, KurulUyeleriService>();
builder.Services.AddScoped<IAnaSayfaService, AnaSayfaService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IDilService, DilService>();
builder.Services.AddScoped<IEkSayfalarService, EkSayfalarService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Home/Error?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

// Localization middleware'i routing'den ÖNCE ekle
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

app.UseRouting();

// Admin panel erişim kontrolü
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/Admin"))
    {
        if (!context.User.Identity?.IsAuthenticated ?? false)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Roller tanımlama işlemi
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "RootRole", "AdminRole" };

    var task = Task.Run(async () =>
    {
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    });

    task.Wait();

    // Varsayılan kullanıcı ekleme işlemi
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
    var defaultFullName = "Root";
    var defaultEmail = "root@root.com";
    var defaultUserName = "root@root.com";
    var defaultPassword = "root1234";

    var userTask = Task.Run(async () =>
    {
        var user = await userManager.FindByEmailAsync(defaultEmail);
        if (user == null)
        {
            user = new Users
            {
                FullName = defaultFullName,
                UserName = defaultUserName,
                Email = defaultEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, defaultPassword);
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "RootRole");
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    Console.WriteLine($"Hata: {error.Description}");
                }
            }
        }
    });
    userTask.Wait();
}

app.Run();