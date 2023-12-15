using Microsoft.EntityFrameworkCore;
using Tracker.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<RepairRequestService>();
builder.Services.AddScoped<RepairRequestRepository>();
builder.Services.AddHostedService<RepairRequestGeneratorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "repairRequests",
    pattern: "{action=Index}/{id?}",
    defaults: new { controller = "RepairRequests" });
app.Run();
