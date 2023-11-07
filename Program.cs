using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//ajout du service dbcontext pour la base de donn�e
builder.Services.AddDbContext<NapajDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NapajConnexionString")));
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    // Configuration des r�gles de validation du mot de passe
    options.Password.RequireDigit = true; // N�cessite au moins un chiffre (0-9)
    options.Password.RequireLowercase = true; // N�cessite au moins une minuscule (a-z)
    options.Password.RequireUppercase = true; // N�cessite au moins une majuscule (A-Z)
    options.Password.RequireNonAlphanumeric = true; // N�cessite au moins un caract�re sp�cial
    options.Password.RequiredLength = 8; // Longueur minimale du mot de passe
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NapajDBContext>()
    .AddSignInManager<SignInManager<ApplicationUser>>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
