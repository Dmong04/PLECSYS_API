using APPLICATION.Handlers;
using APPLICATION.Handlers.GPS;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Tracking_visits;
using APPLICATION.Use_cases.GPS.SellerRoutes_case;
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

/// <summary>
/// Punto de entrada principal de la aplicación.
/// Configura los servicios, middlewares y el pipeline HTTP de la API.
/// </summary>

var builder = WebApplication.CreateBuilder(args);

// Http Context
/// <summary>
/// Registra el cliente HTTP para realizar solicitudes externas.
/// </summary>
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();

/// <summary>
/// Configura la conexión al contexto de base de datos SQL Server.
/// Utiliza la cadena de conexión "LocalConnection" definida en appsettings.
/// </summary>
builder.Services.AddDbContext<AppDBContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));

builder.Services.AddFastEndpoints();

/// <summary>
/// Configura la conexión al contexto de base de datos MongoDB.
/// Lee la configuración desde la sección "MongoDbSettings" en appsettings.
/// </summary>
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDbSettings"));

/// <summary>
/// Registra el cliente MongoDB como singleton usando la cadena de conexión configurada.
/// </summary>
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

/// <summary>
/// Registra la base de datos MongoDB como singleton para ser inyectada en los repositorios.
/// </summary>
builder.Services.AddSingleton(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return mongoClient.GetDatabase(settings.DatabaseName);
});

/// <summary>
/// Registra las colecciones de MongoDB como singleton para acceso centralizado.
/// </summary>
builder.Services.AddSingleton<MongoCollections>();

/// <summary>
/// Registra los repositorios del dominio principal con ciclo de vida Scoped.
/// Cada repositorio se asocia a su interfaz correspondiente.
/// </summary>
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IPaymentRecordRepository, PaymentRecordRepository>();
builder.Services.AddScoped<IInvoiceHistoryRepository, InvoiceHistoryRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<ISellerLocationTrackingRepository, SellerLocationTrackingRepository>();
builder.Services.AddScoped<ISellerTrackingConfigRepository, SellerTrackingConfigRepository>();
builder.Services.AddScoped<ISaleOrderRepository, SaleOrderRepository>();
builder.Services.AddScoped<ISaleOrderDetailsRepository, SaleOrderDetailsRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

/// <summary>
/// Registra los handlers de aplicación con ciclo de vida Scoped.
/// Los handlers coordinan la lógica entre los endpoints y los casos de uso.
/// </summary>
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

/// <summary>
/// Registra el servicio JWT como singleton para generación y validación de tokens.
/// </summary>
builder.Services.AddSingleton<JwtService>();

/// <summary>
/// Configura la autenticación mediante JWT Bearer.
/// Valida el emisor, audiencia, tiempo de vida y la firma del token
/// usando los valores definidos en la sección "Jwt" de appsettings.
/// </summary>
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

/// <summary>
/// Configura la política de CORS "AllowAll" que permite cualquier origen,
/// método y encabezado. Útil para desarrollo; restringir en producción.
/// </summary>
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

/// <summary>
/// Registra los repositorios, handlers y DTOs relacionados con rutas de itinerario GPS.
/// </summary>
builder.Services.AddScoped<IItineraryRouteRepository, ItineraryRouteRepository>();
builder.Services.AddScoped<GeoFenceService>();
builder.Services.AddScoped<ItineraryRouteHandler>();
builder.Services.AddScoped<ItineraryRouteRequest>();
builder.Services.AddScoped<ItineraryRouteResponse>();
builder.Services.AddScoped<TrackingItineraryResponse>();
builder.Services.AddScoped<GeoJsonLocationRequest>();
builder.Services.AddScoped<TargetPointItineraryRequest>();

/// <summary>
/// Registra los repositorios, handlers y DTOs relacionados con rutas de vendedores GPS.
/// </summary>
builder.Services.AddScoped<ISellerRouteRepository, SellerRouteRepository>();
builder.Services.AddScoped<SellerRouteHandler>();
builder.Services.AddScoped<SellerRouteRequest>();
builder.Services.AddScoped<SellerRouteResponse>();
builder.Services.AddScoped<TargetLocationRequest>();
builder.Services.AddScoped<TargetPointRequest>();

var app = builder.Build();

/// <summary>
/// Define el endpoint raíz GET "/" que confirma que la API está activa.
/// </summary>
app.MapGet("/", () => Results.Ok("API corriendo correctamente con FastEndpoints"));

/// <summary>
/// Configura el pipeline HTTP: enrutamiento, redirección HTTPS y FastEndpoints.
/// </summary>
app.UseRouting();
app.UseHttpsRedirection();
app.UseFastEndpoints();
app.Run();