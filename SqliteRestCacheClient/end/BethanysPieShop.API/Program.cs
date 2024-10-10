using System.Security.Claims;
using BethanysPieShop.API.DBContexts;
using BethanysPieShop.API.Repositories;
using BethanysPieShop.API.Repositories.Interfaces;
using BethanysPieShop.API.Services;
using BethanysPieShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// register the DbContext on the container, getting the
// connection string from appSettings   
builder.Services.AddDbContext<BethanysPieShopDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:BethanysPieShopDBConnectionString"]));

var options = new DbContextOptionsBuilder<BethanysPieShopDbContext>()
    .UseSqlite(builder.Configuration["ConnectionStrings:BethanysPieShopDBConnectionString"])
    .Options;

await using var sqliteDbContext = new BethanysPieShopDbContext(options);

builder.Services.AddAuthorization();
builder.Services
    .AddIdentityApiEndpoints<IdentityUser>(opt =>
    {
        opt.Password.RequiredLength = 8;
        opt.User.RequireUniqueEmail = true;
        opt.Password.RequireNonAlphanumeric = false;
        opt.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<BethanysPieShopDbContext>();
builder.Services.AddAuthentication();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPieService, PieService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();

app.MapPost("/Logout", async (SignInManager<IdentityUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return TypedResults.Ok();
}).RequireAuthorization(); // So that only authorized users can use this endpoint

app.MapGet("/useremail", (ClaimsPrincipal user) => Task.FromResult(Results.Ok(user.Identity!.Name)))
    .RequireAuthorization(); // So that only authorized users can use this endpoint

app.MapControllers();

app.Run();