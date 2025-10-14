using BotWhatsapp.Application.Features.Whatsapp;
using BotWhatsapp.Application.Features.Whatsapp.MessageSteps;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Services;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Infrastructure.Context;
using BotWhatsapp.Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ICalendarService).Assembly)); // Register all MediatR handlers in the Application assembly

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 32)),
        mySqlOptions => mySqlOptions.MigrationsAssembly("WebApi")
    )
);

// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IClientSessionRepository, ClientSessionRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

// Strategies
builder.Services.AddScoped<MenuPrincipalStep>();
builder.Services.AddScoped<ElegirDiaStep>();
builder.Services.AddScoped<ElegirDuracionStep>();
builder.Services.AddScoped<ElegirHorarioStep>();
builder.Services.AddScoped<ConfirmarTurnoStep>();
builder.Services.AddScoped<ReprogramarTurnoStep>();
builder.Services.AddScoped<CancelarTurnoStep>();
builder.Services.AddScoped<EsperandoNombreStep>();
builder.Services.AddScoped<VerTurnosStep>();
builder.Services.AddScoped<WaitingForCancellationChoiceStep>();
builder.Services.AddScoped<WaitingForRescheduleChoiceStep>();

// Resolver
builder.Services.AddScoped<MessageStepResolver>();

// Service
builder.Services.AddScoped<IWhatsappService, WhatsappService>();
builder.Services.AddScoped<IEvolutionService, EvolutionService>();

// Manager
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IResourceService, ResourcesService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IloginService, LoginService>();
builder.Services.AddScoped<IOpenAIService, OpenAiService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IWhatsappBotService, WhatsappBotService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<ICalendarService, CalendarService>(); // New line

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var accessToken = builder.Configuration["MercadoPago:AccessToken"];
builder.Services.AddSingleton(new MercadoPagoQrService(accessToken));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Antes de builder.Build()
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Cambia al origen de tu frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.EnsureCreatedAsync();

}

app.Run();
