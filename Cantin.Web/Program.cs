using Cantin.Data.Extensions;
using Cantin.Service.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using NToastNotify;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddNToastNotifyToastr(new ToastrOptions
{
    PositionClass = ToastPositions.TopRight,
    TimeOut = 5000
});
// Add services to the container.
builder.Services.LoadServiceLayerExtensions();
builder.Services.LoadDataLayerExtensions(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en-US") };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddAuthorization(opt => {
    opt.AddPolicy("AdminOnly", x => {
        x.RequireRole("Admin", "Superadmin");
    });
});

builder.Services.AddSession();
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = new PathString("/Auth/Login");
    config.LogoutPath = new PathString("/Auth/Logout");
    config.Cookie = new CookieBuilder
    {
        Name = "Cantin",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromDays(1);
    config.AccessDeniedPath = new PathString("/Auth/AccessDenied");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(locOptions);
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
}
);
app.Run();
