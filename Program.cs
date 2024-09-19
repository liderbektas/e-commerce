using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Servisleri container'a ekleyin.
builder.Services.AddControllersWithViews();

// Session durumunu etkinleştirin
builder.Services.AddDistributedMemoryCache(); // IDistributedCache için varsayılan bellek içi uygulama ekler
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session zaman aşımı süresi
    options.Cookie.HttpOnly = true; // XSS saldırılarına karşı session çerezinin sadece Http üzerinden erişilebilir olmasını sağlar
    options.Cookie.IsEssential = true; // GDPR kapsamında bile çerezin gerekli olduğunu belirtir
});

// Kimlik doğrulama işlemleri için gerekli servis ayarı
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Giriş sayfasının yolu
        options.AccessDeniedPath = "/Home/AccessDenied"; // Erişim izni olmayan sayfa
    });

var app = builder.Build();

// HTTP istek işleme hattını yapılandırın.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Yetkilendirmeden önce session'ı kullanın
app.UseSession(); // Bunun app.UseAuthorization()'dan önce çağrıldığından emin olun

app.UseAuthentication(); // Kimlik doğrulama kullanıyorsanız bunu ekleyin
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();