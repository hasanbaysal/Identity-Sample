using IdentityTutorial.Web.ClaimProvider;
using IdentityTutorial.Web.Extentions;
using IdentityTutorial.Repository.Models;
using IdentityTutorial.Core.OptionsModel;
using IdentityTutorial.Web.PolicyRequirement;
using IdentityTutorial.Service.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();






builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthorizationHandler,FreeAccess10DaysRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler,ViolenceRequirementHandler>();

builder.Services.AddScoped<IClaimsTransformation,UserClaimProvider>();

builder.Services.AddAuthorization(opt =>
{
   
    opt.AddPolicy("CityPolicy", policy =>
    {
                               
                               

        policy.RequireClaim("city", "ankara");
    });

    
    opt.AddPolicy("FreeAccessPolicy", policy =>
    {
        policy.AddRequirements(new FreeAccess10DaysRequirement());
    });

    opt.AddPolicy("ViolentContent", policy =>
    {
        policy.AddRequirements(new ViolenceRequirement() { ThresOldAge=18});
    });


});





builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddScoped<IMemberService, MemberManager>();
builder.Services.AddScoped<IUserService, UsersManager>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


builder.Services.AddIdentityConfiguration();



builder.Services.Configure<DataProtectionTokenProviderOptions>(option =>
{
    option.TokenLifespan = TimeSpan.FromHours(2);
});


builder.Services.Configure<SecurityStampValidatorOptions>(option =>
{
    option.ValidationInterval = TimeSpan.FromMinutes(30);
});

builder.Services.ConfigureApplicationCookie(opt =>
{

    opt.LoginPath = new PathString("/Home/Signin");
    opt.Cookie = new CookieBuilder()
    {
       Name="IdentityTutorialCookie",
    };
    opt.ExpireTimeSpan = TimeSpan.FromDays(2);
    opt.SlidingExpiration = true;
    opt.LogoutPath = new PathString("/Member/Logout");
    opt.AccessDeniedPath = new PathString("/Member/AccesDenied");


});

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); 


app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
