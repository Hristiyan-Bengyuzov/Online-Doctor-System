using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Data.Seeders;
using OnlineDoctorSystem.Web.Infrastructure.Extensions;
using OnlineDoctorSystem.Services.Data.Interfaces;
using OnlineDoctorSystem.Services.Data;
using OnlineDoctorSystem.Web.Hubs;
using OnlineDoctorSystem.Services.Scraping;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<OnlineDoctorDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 8;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<OnlineDoctorDbContext>();

builder.Services.AddSignalR();

builder.Services.AddApplicationServices(typeof(ITownsService));
builder.Services.AddApplicationServices(typeof(ISpecialtiesService));
builder.Services.AddApplicationServices(typeof(IDoctorsService));
builder.Services.AddApplicationServices(typeof(IPatientsService));
builder.Services.AddApplicationServices(typeof(IConsultationsService));
builder.Services.AddApplicationServices(typeof(IPrescriptionsService));
builder.Services.AddApplicationServices(typeof(IReviewsService));
builder.Services.AddApplicationServices(typeof(IEventsService));
builder.Services.AddApplicationServices(typeof(IDoctorScraperService));
builder.Services.AddHostedService<ConsultationsBackgroundService>();

builder.Services.AddControllersWithViews()
			   .AddMvcOptions(options =>
			   {
				   options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
			   });

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
	var dbContext = serviceScope.ServiceProvider.GetRequiredService<OnlineDoctorDbContext>();
	new OnlineDoctorDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
}

if (app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error/500");
	app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
	//app.UseMigrationsEndPoint();
	//app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error/500");
	app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(config =>
{
	config.MapControllerRoute
	(
		name: "areas",
		pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);

	config.MapHub<ChatHub>("/chat");
	config.MapDefaultControllerRoute();
	config.MapRazorPages();
});


await app.RunAsync();