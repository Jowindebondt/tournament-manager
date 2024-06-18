using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TournamentManager.Application;
using TournamentManager.Application.Repositories;
using TournamentManager.Domain;
using TournamentManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SQLAZURECONNSTR_DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DI
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
builder.Services.AddScoped(typeof(ITournamentRepository), typeof(TournamentRepository));
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<IRoundService, RoundService>();
builder.Services.AddScoped<IPouleService, PouleService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

// Sport services
builder.Services.AddScoped<TableTennisService>();
builder.Services.AddScoped<SportServiceResolver>(serviceProvider => key => key switch
    {
        Sport.TableTennis => serviceProvider.GetService<TableTennisService>(),
        _ => throw new KeyNotFoundException($"Key {key} not found as a sport service"),
    }
);

// TableTennis generators
builder.Services.AddScoped<TableTennisRoundGenerator>();
builder.Services.AddScoped<TableTennisPouleGenerator>();
builder.Services.AddScoped<TableTennisMatchGenerator>();
builder.Services.AddScoped<TableTennisGameGenerator>();

// Poule templates
builder.Services.AddScoped<Poule3PlayerTemplateService>();
builder.Services.AddScoped<Poule4PlayerTemplateService>();
builder.Services.AddScoped<Poule5PlayerTemplateService>();
builder.Services.AddScoped<Poule6PlayerTemplateService>();
builder.Services.AddScoped<Poule7PlayerTemplateService>();
builder.Services.AddScoped<Poule8PlayerTemplateService>();
builder.Services.AddScoped<Poule9PlayerTemplateService>();
builder.Services.AddScoped<Poule10PlayerTemplateService>();
builder.Services.AddScoped<Poule11PlayerTemplateService>();
builder.Services.AddScoped<Poule12PlayerTemplateService>();
builder.Services.AddScoped<PouleTemplateResolver>(serviceProvider => key => key switch
    {
        (int)PoulePlayerTemplates.ThreePlayers => serviceProvider.GetService<Poule3PlayerTemplateService>(),
        (int)PoulePlayerTemplates.FourPlayers => serviceProvider.GetService<Poule4PlayerTemplateService>(),
        (int)PoulePlayerTemplates.FivePlayers => serviceProvider.GetService<Poule5PlayerTemplateService>(),
        (int)PoulePlayerTemplates.SixPlayers => serviceProvider.GetService<Poule6PlayerTemplateService>(),
        (int)PoulePlayerTemplates.SevenPlayers => serviceProvider.GetService<Poule7PlayerTemplateService>(),
        (int)PoulePlayerTemplates.EightPlayers => serviceProvider.GetService<Poule8PlayerTemplateService>(),
        (int)PoulePlayerTemplates.NinePlayers => serviceProvider.GetService<Poule9PlayerTemplateService>(),
        (int)PoulePlayerTemplates.TenPlayers => serviceProvider.GetService<Poule10PlayerTemplateService>(),
        (int)PoulePlayerTemplates.ElevenPlayers => serviceProvider.GetService<Poule11PlayerTemplateService>(),
        (int)PoulePlayerTemplates.TwelvePlayers => serviceProvider.GetService<Poule12PlayerTemplateService>(),
        _ => throw new KeyNotFoundException($"Key {key} not found for poule template"), 
    }
);
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
