using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure(maxRetryCount: 5,
		maxRetryDelay: TimeSpan.FromSeconds(10),
		errorNumbersToAdd: null)));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


//change to application use not identity
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>();
//	.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IUnitofWork, UnitofWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//starts seeding
//using (var scope = app.Services.CreateScope())
//{
//	var services = scope.ServiceProvider;
//	var context = services.GetRequiredService<ApplicationDbContext>();
//	context.Database.EnsureCreated();
//	DbInitializer.Initialize(context);
//}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
