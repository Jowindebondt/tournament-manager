using Design.Application.Services;
using Design.Domain;
using Design.Infrastructure.Persistence;
using Design.Infrastructure.Repositories;
using EventBus.Factory;
using EventBus.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

#region Configuration file
var dbConnectionString = builder.Configuration.GetConnectionString("DesignConnection")!;
var eventQueueConnectionString = builder.Configuration.GetConnectionString("EventQueue")!;
var eventQueueType = builder.Configuration.GetValue<EventQueueType>("EventQueueType");
#endregion

builder.Services.AddDbContext<DesignDbContext>(options => options.UseSqlServer(dbConnectionString));
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DI
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<IRoundRepository, RoundRepository>();
builder.Services.AddScoped<IPouleRepository, PouleRepository>();

builder.Services.AddScoped<TournamentService>();
builder.Services.AddScoped<RoundService>();
builder.Services.AddScoped<PouleService>();

builder.Services.AddSingleton<IEventQueue>(provider => EventQueueFactory.CreateEventQueue(eventQueueType, eventQueueConnectionString));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
