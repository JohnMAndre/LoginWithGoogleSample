using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc.Razor;

Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration.GetValue<string>("Authentication:Google:client_id");
    options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Google:client_secret");
    options.CallbackPath = "/sigin-google";
    options.SaveTokens = true;
});

builder.Services.AddSession(options =>
{
    /*options.IdleTimeout = TimeSpan.FromMinutes(1);*/ //Default 20 mins
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    /*options.IdleTimeout = TimeSpan.FromMinutes(1);*/ //Default 20 mins
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "script-src 'self' https://accounts.google.com/gsi/client;" +
        " frame-src 'self' https://accounts.google.com/gsi/;" +
        " connect-src 'self' https://accounts.google.com/gsi/");
    context.Response.Headers.Add("Cross-Origin-Opener-Policy", "same-origin-allow-popups");
    //context.Response.Headers.Add("Cross-Origin-Opener-Policy-Report-Only", "same-origin-allow-popups");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapRazorPages();
app.MapControllers();

app.Run();