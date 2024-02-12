using ElectoralSystem.Data;
using ElectoralSystem.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ElectoralSystem.Repositories.Interfaces;
using ElectoralSystem.Repositories;
using ElectoralSystem.Models.Entities;
using ElectoralSystem.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("ElectionDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ElectionDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

string connString = "Server=DESKTOP-1CO21O5\\SQLEXPRESS;Database=sample4;Trusted_Connection=True;TrustServerCertificate=True;";
//string connString = "Data Source = mydatabase.db";
//builder.Services.AddDbContext<ElectionDbContext>(options =>
//                options.UseSqlite(connString, b => b.MigrationsAssembly("ElectoralSystem")));
builder.Services.AddDbContext<ElectionDbContext>(options =>
                options.UseSqlServer(connString, b => b.MigrationsAssembly("ElectoralSystem")));


builder.Services.AddScoped<IAsyncRepository<State>, StateRepository>();
builder.Services.AddScoped<IAsyncRepository<Candidate>, CandidateRepository>();
builder.Services.AddScoped<IAsyncRepository<Voter>, VoterRepository>();
builder.Services.AddScoped<IAsyncRepository<Party>, PartyRepository>();
builder.Services.AddScoped<IAsyncRepository<PartySymbol>, PartySymbolRepository>();
builder.Services.AddScoped<IAsyncRepository<CandidateResult>, CandidateResultsRepository>();
builder.Services.AddScoped<IAsyncRepository<ElectionResult>, ElectionResultsRepository>();
builder.Services.AddScoped<IAsyncRepository<VotingSession>, VotingSessionsRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>() // This adds role services
        .AddEntityFrameworkStores<ElectionDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorPagesOptions(options => {
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
}); ;

var app = builder.Build();

app.MigrateDatabase<ElectionDbContext>((context, services) =>
{
   
    ElectionContextSeed
        .SeedAsync(context, services)
        .Wait();
});



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
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "Voters",
    pattern: "{area=Voters}/{controller=Voters}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "ECs",
    pattern: "{area=ECs}/{controller=Home}/{action=Index}/{id?}");

app.MapFallbackToFile("/Identity/Account/Login");

app.Run();
