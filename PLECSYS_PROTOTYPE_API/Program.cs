using APPLICATION.Handlers;
using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Tracking_visits;
using APPLICATION.Use_cases.GPS.SellerRoutes_case;
using APPLICATION.Use_cases.Login_case;
using APPLICATION.Utils.GPS;
using APPLICATION.Utils.JWT;
using APPLICATION.Utils.PDFs;
using DOMAIN.Interfaces;
using DOMAIN.Interfaces.GPS;
using FastEndpoints;
using INFRASTRUCTURE.Context;
using INFRASTRUCTURE.Context.Mongo;
using INFRASTRUCTURE.Context.Mongo.Config;
using INFRASTRUCTURE.Repositories;
using INFRASTRUCTURE.Repositories.GPS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Http Context
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();

// Database context connection

builder.Services.AddDbContext<AppDBContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));

builder.Services.AddFastEndpoints();

// MongoDB database context connection

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return mongoClient.GetDatabase(settings.DatabaseName);
});

builder.Services.AddSingleton<MongoCollections>();

// Add services to the container.

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IPaymentMethodRepository,  PaymentMethodRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IPaymentRecordRepository, PaymentRecordRepository>();
builder.Services.AddScoped<IInvoiceHistoryRepository, InvoiceHistoryRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<ISellerLocationTrackingRepository, SellerLocationTrackingRepository>();
builder.Services.AddScoped<ISellerTrackingConfigRepository, SellerTrackingConfigRepository>();
builder.Services.AddScoped<ISaleOrderRepository, SaleOrderRepository>();
builder.Services.AddScoped<ISaleOrderDetailsRepository, SaleOrderDetailsRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<UserHandler>();
builder.Services.AddScoped<InvoiceHandler>();
builder.Services.AddScoped<InvoicePdfService>();
builder.Services.AddScoped<CompanyHandler>();
builder.Services.AddScoped<PaymentMethodHandler>();
builder.Services.AddScoped<CurrencyHandler>();
builder.Services.AddScoped<PaymentRecordHandler>();
builder.Services.AddScoped<InvoiceHistoryHandler>();
builder.Services.AddScoped<ClaimHandler>();
builder.Services.AddScoped<SellerLocationTrackingHandler>();
builder.Services.AddScoped<SellerTrackingConfigHandler>();
builder.Services.AddScoped<SupplierHandler>();
builder.Services.AddSingleton<JwtService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


// itinerary routes
builder.Services.AddScoped<IItineraryRouteRepository, ItineraryRouteRepository>();
builder.Services.AddScoped<GeoFenceService>();
builder.Services.AddScoped<ItineraryRouteHandler>();
builder.Services.AddScoped<ItineraryRouteRequest>();
builder.Services.AddScoped<ItineraryRouteResponse>();
builder.Services.AddScoped<TrackingItineraryResponse>();
builder.Services.AddScoped<GeoJsonLocationRequest>();
builder.Services.AddScoped<TargetPointItineraryRequest>();

// seller routes
builder.Services.AddScoped<ISellerRouteRepository, SellerRouteRepository>();
builder.Services.AddScoped<SellerRouteHandler>();
builder.Services.AddScoped<SellerRouteRequest>();
builder.Services.AddScoped<SellerRouteResponse>();
builder.Services.AddScoped<SellerRouteHandler>();
builder.Services.AddScoped<TargetLocationRequest>();
builder.Services.AddScoped<TargetPointRequest>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => Results.Ok("API corriendo correctamente con FastEndpoints"));

app.UseRouting();
app.UseHttpsRedirection();
app.UseFastEndpoints();
app.Run();